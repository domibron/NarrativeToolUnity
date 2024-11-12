using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DialogSO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public static class DialogTreeSavingAndLoading
{
	public static void Save(string filename, TreeGraphView graphView)
	{
		if (graphView == null) return;

		// Prompt user to save the file and store their selected path
		string path = EditorUtility.SaveFilePanelInProject("Save Dialog Tree", filename, "asset", "");
		if (path == "") return;

		// Create an instance of the SO that we will fill with data before saving
		DialogTreeData saveData = ScriptableObject.CreateInstance("DialogTreeData") as DialogTreeData;

		GUID? startNodeGUID = null;

		graphView.graphElements.ForEach(e =>
		{
			if (e is DialogNode dialogNode)
			{
				// Create and initialise save data for this weapon
				DialogSaveData dialog = new DialogSaveData();
				dialog.Init(dialogNode.NodeGUID, dialogNode.GetPosition().position, dialogNode.NameOfNode, dialogNode.Dialog);

				List<GUID> inputDialogNodes = new List<GUID>();
				List<GUID> outputDialogNodes = new List<GUID>();
				// If node has a predecessor, grab it's GUID
				try
				{
					inputDialogNodes = GetInputNodes(dialogNode);

					if (inputDialogNodes.Count > 0)
					{
						dialog.SetInput(inputDialogNodes);
					}
				}
				catch (ArgumentOutOfRangeException)
				{
					// do nothing
				}

				try
				{
					outputDialogNodes = GetOutputNodes(dialogNode);


					if (inputDialogNodes.Count > 0)
					{
						dialog.SetOutput(outputDialogNodes);
					}
				}
				catch (ArgumentOutOfRangeException)
				{
					// do nothing
				}

				// Add this weapon data to the SO
				saveData.DialogNodes.Add(dialog);
				return;
			}
			else if (e is StartNode startNode)
			{
				DialogSaveData startDialogData = new DialogSaveData();
				startDialogData.Init(startNode.NodeGUID, startNode.GetPosition().position, "", "", true);

				saveData.DialogNodes.Add(startDialogData);


				Port outputPort = startNode.outputContainer.ElementAt(0) as Port;



				foreach (UnityEditor.Experimental.GraphView.Edge edge in outputPort.connections)
				{
					startNodeGUID = (edge.input.node as BaseNode).NodeGUID;
				}



				if (startNodeGUID.HasValue)
				{
					startDialogData.SetOutput(startNodeGUID.Value);
				}
			}
		});

		if (startNodeGUID.HasValue)
		{
			saveData.StartNode = saveData.FindByGuid(startNodeGUID.Value);
		}

		// Save the asset
		AssetDatabase.CreateAsset(saveData, path);
		AssetDatabase.Refresh();
	}

	/// <summary>
	/// Load the WeaponTreeAsset and populate a graph view with the data.
	/// </summary>
	public static void Load(TreeGraphView graphView)
	{
		try
		{
			PopulateGraphViewFromAsset(graphView, PromptUserForSavedAsset());
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
	}

#nullable enable
	private static DialogTreeData PromptUserForSavedAsset()
	{

		// Load scriptable object
		string path = EditorUtility.OpenFilePanel("Load Dialog Tree", "Assets", "asset");
		if (path == "") throw new NullReferenceException("Cannot use a invalid asset!");
		path = ToRelativePath(path);
		return AssetDatabase.LoadAssetAtPath<DialogTreeData>(path);
	}
#nullable restore

	private static void PopulateGraphViewFromAsset(TreeGraphView graphView, DialogTreeData saveData)
	{
		// Temporarily store all nodes from SO here
		Dictionary<GUID, DialogNode> nodes = new Dictionary<GUID, DialogNode>();
		StartNode startNode = null;

		// Extract all nodes and their base info
		foreach (var dialogNode in saveData.DialogNodes)
		{


			GUID dialogGUID = StringToGUID(dialogNode.DialogNodeGUID);

			if (dialogNode.IsStart)
			{
				startNode = graphView.CreateStartNode(dialogGUID, dialogNode.Position);

				graphView.AddElement(startNode);

				continue;
			}

			DialogNode node = graphView.CreateDialogNode(dialogGUID, dialogNode.Position, dialogNode.DialogNodeName, dialogNode.Dialog);

			graphView.AddElement(node);
			nodes.Add(dialogGUID, node);
		}

		// Find node connections by checking each node's previous node GUID
		// Connect the nodes and add the edge to the graph
		foreach (var node in nodes)
		{
			Port inputPort = node.Value.inputContainer.ElementAt(0) as Port;

			List<string> inputGUIDasStrings = saveData.FindByGuid(node.Key).InputConnectedGUIDs;
			if (inputGUIDasStrings.Count <= 0)
				continue;

			foreach (var inputGUID in inputGUIDasStrings)
			{
				if (!nodes.ContainsKey(StringToGUID(inputGUID)))
				{
					Port startOutPort = startNode.outputContainer.ElementAt(0) as Port;
					Edge startNewEdge = inputPort.ConnectTo(startOutPort);
					graphView.Add(startNewEdge);

					continue;
				}

				DialogNode prevNode = nodes[StringToGUID(inputGUID)];

				Port outputPort = prevNode.outputContainer.ElementAt(0) as Port;
				Edge newEdge = inputPort.ConnectTo(outputPort);
				graphView.Add(newEdge);
			}
		}
	}

	private static List<GUID> GetInputNodes(BaseNode node)
	{
		// Here we are following some pre-established rules from the rest of the
		// code to try to step back to a previous node.

		// First, get the input port (left one) from the node. We know that [0]
		// is going to be the input port in the input container because we defined
		// it in WeaponNode.Draw()
		Port inputPort = node.inputContainer.ElementAt(0) as Port;

		List<GUID> guids = new List<GUID>();

		// BaseNode prevNode = null;
		GUID nodeGUID = new GUID();
		// Now go through every edge on this port. There should only be 0 or 1
		// because we made input ports single capacity.
		foreach (Edge edge in inputPort.connections)
		{
			// If there is a port, the following line will extract the connected
			// node by first going to the edge's output port, then to it's node.
			//                                         ||
			//                                         \/
			//                           output port  []------[]  input port

			// GAAHHHHHH YOU COMMENTS ARE GETTING IN THEH WAYYHYYSSYY!!!!!!!!!

			nodeGUID = (edge.output.node as BaseNode).NodeGUID;
			guids.Add(nodeGUID);
		}



		return guids;
	}

	private static List<GUID> GetOutputNodes(BaseNode node)
	{
		// Here we are following some pre-established rules from the rest of the
		// code to try to step back to a previous node.

		// First, get the input port (left one) from the node. We know that [0]
		// is going to be the input port in the input container because we defined
		// it in WeaponNode.Draw()
		Port outputPort = node.outputContainer.ElementAt(0) as Port;

		List<GUID> guids = new List<GUID>();

		// BaseNode prevNode = null;
		GUID nodeGUID = new GUID();
		// Now go through every edge on this port. There should only be 0 or 1
		// because we made input ports single capacity.
		foreach (Edge edge in outputPort.connections)
		{
			// If there is a port, the following line will extract the connected
			// node by first going to the edge's output port, then to it's node.
			//                                         ||
			//                                         \/
			//                           output port  []------[]  input port

			// GAAHHHHHH YOU COMMENTS ARE GETTING IN THEH WAYYHYYSSYY!!!!!!!!!

			nodeGUID = (edge.input.node as BaseNode).NodeGUID;
			guids.Add(nodeGUID);
		}



		return guids;
	}

	/// <summary>
	/// Convert an absolute path to an Assets folder relative path.
	/// </summary>
	public static string ToRelativePath(string absolutePath)
	{
		if (absolutePath.StartsWith(Application.dataPath))
		{
			return "Assets" + absolutePath.Substring(Application.dataPath.Length);
		}
		else
		{
			throw new System.ArgumentException(
				"Full path does not contain the current project's Assets folder", "absolutePath");
		}
	}

	/// <summary>
	/// Helper function to convert a string into a GUID.
	/// </summary>
	public static GUID StringToGUID(string str)
	{
		GUID guid;
		if (!GUID.TryParse(str, out guid))
		{
			throw new ArgumentException("Invalid GUID, cannot convert from string to GUID.");
		}
		return guid;
	}
}
