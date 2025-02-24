using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace SmashKeyboardStudios.NarrativeTool.Editor
{
	/// <summary>
	/// Creates the graph.
	/// </summary>
	public class TreeGraphView : GraphView
	{
		// We must only have one start node, this keeps track of it.
		public StartNode StartNode;
		// public GUID StartNodeGuid;

		// We can use a ctor to initialise our graph view.
		public TreeGraphView()
		{

			AddGridBackground();
			AddStyle();
			AddManipulators();

			Init();
		}

		/// <summary>
		/// Sets up the TreeGraphView.
		/// </summary>
		public void Init()
		{
			if (StartNode == null)
			{
				// magic. WYM Magic? all this does is create the start node if there isn't one initialized.
				StartNode = CreateStartNode(Vector2.zero);
				((GraphView)this).AddElement(StartNode);
			}


		}

		private void AddGridBackground()
		{
			// Create a grid background element
			GridBackground gridBackground = new GridBackground();
			// Since we want to use the entire window, we can stretch it to the
			// size of the parent element.
			gridBackground.StretchToParentSize();

			// This puts the grid into the parent element so it can be drawn
			Insert(0, gridBackground);
		}

		/// <summary>
		/// Load and add the style sheet for this view, most notably the grid colours.
		/// </summary>
		private void AddStyle()
		{
			// trys to get the uss asset in packages, but if that failed, it means its in the assets.
			// if it does not exsists. freak out.
			StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.smashkeyboardstudios.narrativetool.core/Views/GraphStyle.uss");
			if (styleSheet == null)
			{
				styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Views/GraphStyle.uss");
			}

			styleSheets.Add(styleSheet);
		}

		/// <summary>
		/// Add necessary manipulators to the view. Think of these as input handling.
		/// </summary>
		private void AddManipulators()
		{
			// Pan viewport
			this.AddManipulator(new ContentDragger());
			// Zoom viewport, either work but SetupZoom is a wrapper specifically for graph views
			//this.AddManipulator(new ContentZoomer());
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
			// Move selected nodes around, Must be before rectangle selector
			this.AddManipulator(new SelectionDragger());

			// Select nodes using rectangle selection
			this.AddManipulator(new RectangleSelector());
			// Right-click context menu option addition
			this.AddManipulator(CreateNodeContextualMenu());

			this.AddManipulator(CreateStartNodeContextualMenu());
		}

		/// <summary>
		/// Create a new manipulator for creating nodes using the right-click context menu.
		/// </summary>
		private IManipulator CreateNodeContextualMenu()
		{
			// TODO figure out why they are offset by fuck.

			// the menu option will take when selected
			ContextualMenuManipulator manipulator = new ContextualMenuManipulator(
				menuEvent => menuEvent.menu.AppendAction(
					"Add Dialog Node", actionEvent => AddElement(

						CreateDialogNode(actionEvent.eventInfo.localMousePosition)

					)
				)
			);

			return manipulator;
		}

		private IManipulator CreateStartNodeContextualMenu()
		{
			// the menu option will take when selected
			ContextualMenuManipulator manipulator = new ContextualMenuManipulator(
				menuEvent => menuEvent.menu.AppendAction(
					"Add Start Node", actionEvent => AddElement(

						CreateStartNode(actionEvent.eventInfo.localMousePosition)

					)
				)
			);

			return manipulator;
		}

		/// <summary>
		/// Creates a new dialog node to display on the graph.
		/// </summary>
		/// <param name="position">Location of the node on the graph.</param>
		/// <returns>Created node to be used in AddElement(..).</returns>
		public DialogNode CreateDialogNode(Vector2 position)
		{
			DialogNode node = new DialogNode(position);
			return node;
		}

		public DialogNode CreateDialogNode(GUID guid, Vector2 position, string nameOfNode, string dialog)
		{
			DialogNode node = new DialogNode(guid, position, nameOfNode, dialog);
			return node;
		}

		public StartNode CreateStartNode(GUID guid, Vector2 position)
		{
			foreach (var element in graphElements)
			{
				if (element is StartNode)
				{
					RemoveElement(element);
				}
			}

			StartNode node = new StartNode(position);

			if (StartNode == null)
			{
				StartNode = node;
			}
			else
			{
				node.NodeGUID = (StartNode as BaseNode).NodeGUID;
			}

			StartNode = node;

			return node;
		}

		public StartNode CreateStartNode(Vector2 position)
		{


			foreach (var element in graphElements)
			{
				if (element is StartNode)
				{
					RemoveElement(element);
				}
			}

			StartNode node = new StartNode(position);

			if (StartNode == null)
			{
				StartNode = node;
			}
			else
			{
				node.NodeGUID = (StartNode as BaseNode).NodeGUID;
			}

			StartNode = node;


			return node;
		}

		/// <summary>
		/// In order to connect ports, we need to provide a list of compatible ports.
		/// This will be called when we drag out a connection from a node's port.
		/// </summary>
		/// <param name="startPort">The port where the connection began.</param>
		/// <param name="nodeAdapter">Not used.</param>
		/// <returns>List of compatible ports that can be connected to.</returns>
		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
		{
			// List for us to store all compatible ports
			List<Port> compatiblePorts = new List<Port>();
			// Iterate over every other port available
			ports.ForEach(port =>
			{
				// Skip connecting port to itself
				if (startPort == port) return;
				// Skip connecting to other ports on the same node
				if (startPort.node == port.node) return;
				// Skip connecting inputs to inputs and outputs to outputs
				if (startPort.direction == port.direction) return;

				// Add this port to list of compatible ports
				compatiblePorts.Add(port);
			});
			return compatiblePorts;
		}
	}
}