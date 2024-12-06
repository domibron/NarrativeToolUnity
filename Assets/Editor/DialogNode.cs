using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using UnityEditor.UIElements;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace SmashKeyboardStudios.NarrativeTool.Editor
{

	// [Serializable, Obsolete("Not used.")]
	// public struct DialogNodeData
	// {
	// 	public string OptionTextForThisNode;
	// 	public string Dialog;


	// 	public DialogNodeData(string optionTextForThisNode, string dialog)
	// 	{
	// 		OptionTextForThisNode = optionTextForThisNode;
	// 		Dialog = dialog;
	// 	}

	// 	public static DialogNodeData GenerateEmptyDialogData()
	// 	{
	// 		return new DialogNodeData("Option text", "SAMPLE TEXT");
	// 	}

	// 	// create new dialog data rather then a refernce to this obejct.
	// 	[Obsolete("Not used.")]
	// 	public DialogNodeData GetNodeDataAsNew()
	// 	{
	// 		// TODO fix array as it will pass a refernce. Will fix if this func is needed.
	// 		return new DialogNodeData(OptionTextForThisNode, Dialog);
	// 	}
	// }

	public class DialogNode : BaseNode
	{

		// internal DialogNodeData _nodeData { get; private set; }

		internal string OptionTextForThisNode = "Option text";
		internal string Dialog = "";


		public DialogNode(GUID guid, Vector2 pos, string optionTextForThisNode, string dialog)
		{
			NodeGUID = guid;
			OptionTextForThisNode = optionTextForThisNode;
			Dialog = dialog;
			SetPosition(new Rect(pos, Vector2.zero));
			Draw();
		}

		public DialogNode(Vector2 pos)
		{
			NodeGUID = GUID.Generate();
			SetPosition(new Rect(pos, new Vector2(0, 0)));

			Draw();
		}

		/// <summary>
		/// Fills node containers with content, rules for drawing.
		/// </summary>
		public void Draw()
		{

			// Title container
			titleContainer.Clear();
			TextField nameTextField = new TextField()
			{
				value = string.IsNullOrEmpty(OptionTextForThisNode) ? "Name error" : OptionTextForThisNode,
				// isReadOnly = true,
			};
			titleContainer.Insert(0, nameTextField);
			nameTextField.RegisterValueChangedCallback(e => OnNameChanged(e));

			// Input port
			Port inputPort = InstantiatePort(
				Orientation.Horizontal,
				Direction.Input,
				Port.Capacity.Multi,
				null);
			inputPort.portName = "Input";
			inputContainer.Add(inputPort);

			// Output port
			Port outputPort = InstantiatePort(
				Orientation.Horizontal,
				Direction.Output,
				Port.Capacity.Multi,
				null);
			outputPort.portName = "Output";
			outputContainer.Add(outputPort);

			// Extension container
			VisualElement customContainer = new VisualElement();

			TextField textField = new TextField();
			textField.label = "Dialog";
			textField.textEdition.placeholder = "DIALOG TEXT HERE";
			textField.multiline = true;
			textField.verticalScrollerVisibility = ScrollerVisibility.AlwaysVisible;

			textField.value = Dialog;


			customContainer.Add(textField);

			textField.RegisterValueChangedCallback(e =>
			OnDialogChanged(e));

			// inputf objectField = new ObjectField("Dialog")
			// {
			// 	objectType = typeof(string),
			// 	value = string.IsNullOrEmpty(_nodeData.Dialog) ? null : _nodeData.Dialog,
			// };
			// customContainer.Add(objectField);
			// objectField.RegisterValueChangedCallback(
			// 	e => OnDialogChanged(e));

			// Foldout textFoldout = new Foldout()
			// {
			//     text = "Icon",
			// };
			// icon = new Image();
			// // UpdateIcon();
			// textFoldout.Add(icon);
			// customContainer.Add(textFoldout);
			extensionContainer.Add(customContainer);

			// Refresh extension container contents
			RefreshExpandedState();
		}

		/// <summary>
		/// Update the name of the node.
		/// </summary>
		[Obsolete("No need for this function anymore.")]
		private void UpdateTitleContainer()
		{
			titleContainer.Clear();
			TextField nameTextField = new TextField()
			{
				value = string.IsNullOrEmpty(OptionTextForThisNode) ? "Name error" : OptionTextForThisNode,
				// isReadOnly = true,
			};
			titleContainer.Insert(0, nameTextField);
		}


		/// <summary>
		/// Dialog changed
		/// </summary>
		/// <param name="e">Event data.</param>
		private void OnDialogChanged(ChangeEvent<string> e)
		{

			Dialog = e.newValue;

			// UpdateTitleContainer();
			// UpdateIcon();
		}

		private void OnNameChanged(ChangeEvent<string> e)
		{
			OptionTextForThisNode = e.newValue;
		}
	}
}