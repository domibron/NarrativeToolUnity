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
	public class StartNode : BaseNode
	{
		public StartNode(GUID guid, Vector2 pos)
		{
			NodeGUID = guid;


			this.capabilities = Capabilities.Selectable | Capabilities.Movable | Capabilities.Ascendable | Capabilities.Copiable | Capabilities.Snappable | Capabilities.Groupable;

			// _nodeData = DialogNodeData.GenerateEmptyDialogData();
			SetPosition(new Rect(pos, new Vector2(0, 0)));

			Draw();
		}

		public StartNode(Vector2 pos)
		{
			NodeGUID = GUID.Generate();


			this.capabilities = Capabilities.Selectable | Capabilities.Movable | Capabilities.Ascendable | Capabilities.Copiable | Capabilities.Snappable | Capabilities.Groupable;

			// _nodeData = DialogNodeData.GenerateEmptyDialogData();
			SetPosition(new Rect(pos, new Vector2(0, 0)));

			Draw();
		}

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