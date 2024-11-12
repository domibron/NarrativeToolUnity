using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using UnityEditor.UIElements;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

[Serializable]
public struct DialogNodeData
{
	public string NameOfNode;
	public string Dialog;


	public DialogNodeData(string nameOfNode, string dialog)
	{
		NameOfNode = nameOfNode;
		Dialog = dialog;
	}

	public static DialogNodeData GenerateEmptyDialogData()
	{
		return new DialogNodeData("New Dialog Node", "SAMPLE TEXT");
	}

	// create new dialog data rather then a refernce to this obejct.
	public DialogNodeData GetNodeDataAsNew()
	{
		// TODO fix array as it will pass a refernce. Will fix if this func is needed.
		return new DialogNodeData(NameOfNode, Dialog);
	}
}

public class DialogNode : BaseNode
{

	// internal DialogNodeData _nodeData { get; private set; }

	internal string NameOfNode = "New Dialog Node";
	internal string Dialog = "";


	public DialogNode(GUID guid, Vector2 pos, string nameOfNode, string dialog)
	{
		NodeGUID = guid;
		NameOfNode = nameOfNode;
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
			value = string.IsNullOrEmpty(NameOfNode) ? "Name error" : NameOfNode,
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
	/// nuh uh.
	/// </summary>
	private void UpdateTitleContainer()
	{
		titleContainer.Clear();
		TextField nameTextField = new TextField()
		{
			value = string.IsNullOrEmpty(NameOfNode) ? "Name error" : NameOfNode,
			// isReadOnly = true,
		};
		titleContainer.Insert(0, nameTextField);
	}

	/// <summary>
	/// Updates the icon 
	/// </summary>
	// private void UpdateIcon()
	// {
	//     const float width = 250;

	//     // Set sprite
	//     // icon.sprite = dialogData == null ? null : dialogData.name;
	//     // Some size calculations to limit image sizes
	//     icon.style.width = width;
	//     if (icon.sprite != null)
	//     {
	//         icon.style.maxHeight = icon.sprite.textureRect.height *
	//             (width / icon.sprite.textureRect.width);
	//     }
	// }

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
		NameOfNode = e.newValue;
	}
}
