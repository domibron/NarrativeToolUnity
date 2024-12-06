using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace SmashKeyboardStudios.NarrativeTool.Data
{


	/// <summary>
	/// Holds all the data of the dialog tree. This contains all the dialog nodes.
	/// Use the DialogNodes to get individual node data.
	/// </summary>
	public class DialogTreeData : ScriptableObject
	{
		/// <summary>
		/// All the nodes in the asset.
		/// </summary>
		[HideInInspector]
		public List<DialogNodeSaveData> DialogNodes = new List<DialogNodeSaveData>();

		/// <summary>
		/// The starting node so we know the node we start with.
		/// </summary>
		[HideInInspector]
		public DialogNodeSaveData StartNode;

		// ? might cause build error.
		/// <summary>
		/// Finds the node with that GUID.
		/// </summary>
		/// <param name="guid">The GUID of the node.</param>
		/// <returns>The data of the node as DialogNodeSaveData.</returns>
		public DialogNodeSaveData FindByGuid(GUID guid)
		{
			foreach (var dialogNode in DialogNodes)
				if (dialogNode.DialogNodeGUID == guid.ToString())
					return dialogNode;
			return null;
		}


	}

	// TODO Should be its own file.
	/// <summary>
	/// Holds all the data of the dialog node.
	/// </summary>
	[System.Serializable]
	public class DialogNodeSaveData
	{
		[HideInInspector] public string DialogNodeGUID;
		[HideInInspector] public string DialogNodeOptionText;
		[HideInInspector] public string Dialog;
		[HideInInspector] public Vector2 Position;
		[HideInInspector] public List<string> InputConnectedGUIDs = new List<string>();
		[HideInInspector] public List<string> OutputConnectedGUIDs = new List<string>();
		[HideInInspector] public bool IsStart = false;

		/// <summary>
		/// Sets the data of this node with the given data.
		/// </summary>
		/// <param name="guid">The GIUD of the node.</param>
		/// <param name="pos">The position on the graph view.</param>
		/// <param name="dialogOptionText">The name box / text to display for the option.</param>
		/// <param name="dialog">The dialog of the dialog node if the player selects this option.</param>
		/// <param name="isStart">Weather this is a start node.</param>
		public void Init(GUID guid, Vector2 pos, string dialogOptionText, string dialog, bool isStart = false)
		{
			DialogNodeGUID = guid.ToString();
			Position = pos;
			DialogNodeOptionText = dialogOptionText;
			Dialog = dialog;
			IsStart = isStart;
		}

		/// <summary>
		/// It converts the list of GUIDs into a list of strings. Used mainly for saving.
		/// </summary>
		/// <param name="listToConvert">The target list to convert the data.</param>
		/// <returns>A list of GUIDs as strings.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when there is nothing in the provided list.</exception>
		public List<string> ConvertGUIDListToString(List<GUID> listToConvert)
		{
			// TODO, dont do this here, but when saving the assets we can just convert the GUID directly instad of this madness.

			if (listToConvert.Count <= 0) throw new ArgumentOutOfRangeException(nameof(listToConvert));

			List<string> returnedList = new List<string>();

			foreach (var guid in listToConvert)
			{
				returnedList.Add(guid.ToString());
			}

			return returnedList;
		}

		/// <summary>
		/// Sets the input continer node string GUID list with the provided string GUID list.
		/// </summary>
		/// <param name="inputConnectedGUIDs">The list of connected node GUIDs as strings in a list.</param>
		public void SetInput(List<string> inputConnectedGUIDs)
		{
			InputConnectedGUIDs = inputConnectedGUIDs;
		}

		/// <summary>
		/// Sets the input continer node string GUID list with the given GUID list by first converting it.
		/// </summary>
		/// <param name="inputConnectedGUIDs">The list of connected node's GUIDs.</param>
		public void SetInput(List<GUID> inputConnectedGUIDs)
		{
			InputConnectedGUIDs = ConvertGUIDListToString(inputConnectedGUIDs);
		}

		/// <summary>
		/// Sets the output continer node string GUID list with the provided string GUID list.
		/// </summary>
		/// <param name="outputConnectedGUIDs">The list of connected node GUIDs as strings in a list.</param>
		public void SetOutput(List<string> outputConnectedGUIDs)
		{
			OutputConnectedGUIDs = outputConnectedGUIDs;
		}

		/// <summary>
		/// Sets the output continer node string GUID list with the given GUID list by first converting it.
		/// </summary>
		/// <param name="outputConnectedGUIDs">The list of connected node's GUIDs.</param>
		public void SetOutput(List<GUID> outputConnectedGUIDs)
		{
			OutputConnectedGUIDs = ConvertGUIDListToString(outputConnectedGUIDs);
		}

		/// <summary>
		/// Sets one output node GUID.
		/// </summary>
		/// <param name="outputConnectedGUID">The GUID of the node as the single out.</param>
		public void SetOutput(GUID outputConnectedGUID)
		{
			// ?  not sure why this function exsists but the one for input does not???

			OutputConnectedGUIDs.Clear();
			OutputConnectedGUIDs.Add(outputConnectedGUID.ToString());
		}
	}
}
