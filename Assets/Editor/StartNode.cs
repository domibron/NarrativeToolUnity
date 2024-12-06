using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using UnityEditor.UIElements;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace SmashKeyboardStudios.NarrativeTool.Editor
{
	/// <summary>
	/// Start node data.
	/// </summary>
	public class StartNode : BaseNode
	{
		/// <summary>
		/// Creates a start node with the given GUID and position on gird.
		/// </summary>
		/// <param name="guid">Start node GUID.</param>
		/// <param name="pos">Grid position.</param>
		public StartNode(GUID guid, Vector2 pos)
		{
			NodeGUID = guid;

			// This stops the user from deleting / removing the start node.
			this.capabilities = Capabilities.Selectable | Capabilities.Movable | Capabilities.Ascendable | Capabilities.Copiable | Capabilities.Snappable | Capabilities.Groupable;

			// _nodeData = DialogNodeData.GenerateEmptyDialogData();
			SetPosition(new Rect(pos, new Vector2(0, 0)));

			Draw();
		}

		/// <summary>
		/// Creates a start node with just the grid position.
		/// </summary>
		/// <param name="pos">Position on the grid.</param>
		public StartNode(Vector2 pos)
		{
			NodeGUID = GUID.Generate();

			// This stops the user from deleting / removing the start node.
			this.capabilities = Capabilities.Selectable | Capabilities.Movable | Capabilities.Ascendable | Capabilities.Copiable | Capabilities.Snappable | Capabilities.Groupable;

			// _nodeData = DialogNodeData.GenerateEmptyDialogData();
			SetPosition(new Rect(pos, new Vector2(0, 0)));

			Draw();
		}

		/// <summary>
		/// Generate the GUI elements of the start node.
		/// </summary>
		public void Draw()
		{
			// Title container
			titleContainer.Clear();
			TextField nameTextField = new TextField()
			{
				value = "Start Node",
				isReadOnly = true,
			};
			titleContainer.Insert(0, nameTextField);


			// Output port
			Port outputPort = InstantiatePort(
				Orientation.Horizontal,
				Direction.Output,
				Port.Capacity.Single,
				typeof(DialogNode));
			outputPort.portName = "Output";
			outputContainer.Add(outputPort);



			// Refresh extension container contents
			RefreshExpandedState();
		}
	}
}