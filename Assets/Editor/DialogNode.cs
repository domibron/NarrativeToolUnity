using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using UnityEditor.UIElements;
using UnityEditor;
using System.IO;

public class DialogNode : Node
{
    // Used for identifying which node is which
    internal GUID guid { get; private set; }

    // The weapon information that is currently set
    internal SO_Dialog dialogData;
    // For ease of use, we store the weapon icon element here to update easier on weapon changes.
    Image icon;

    public DialogNode(GUID guid, Vector2 pos, SO_Dialog data)
    {
        this.guid = guid;
        dialogData = data;
        SetPosition(new Rect(pos, Vector2.zero));
        Draw();
    }

    public DialogNode(Vector2 pos)
    {
        guid = GUID.Generate();
        SetPosition(new Rect(pos, Vector2.zero));
        Draw();
    }

    /// <summary>
    /// Fills node containers with content, rules for drawing.
    /// </summary>
    public void Draw()
    {
        // Title container
        UpdateTitleContainer();

        // Input port
        Port inputPort = InstantiatePort(
            Orientation.Horizontal,
            Direction.Input,
            Port.Capacity.Single,
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

        ObjectField objectField = new ObjectField("Dialog")
        {
            objectType = typeof(SO_Dialog),
            value = dialogData ? dialogData : null,
        };
        customContainer.Add(objectField);
        objectField.RegisterValueChangedCallback(
            e => OnDialogChanged(e));

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
    /// Updates the title container to match the selected weapon.
    /// </summary>
    private void UpdateTitleContainer()
    {
        titleContainer.Clear();
        TextField nameTextField = new TextField()
        {
            value = dialogData == null ? "Empty" : dialogData.name,
            isReadOnly = true,
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
    /// When the weapon is changed, we use this callback to update the shown content.
    /// </summary>
    /// <param name="e">Event data.</param>
    private void OnDialogChanged(ChangeEvent<UnityEngine.Object> e)
    {
        dialogData = e.newValue as SO_Dialog;
        UpdateTitleContainer();
        // UpdateIcon();
    }
}
