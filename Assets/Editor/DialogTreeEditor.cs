using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using DialogSO;

public class DialogTreeEditor : EditorWindow
{
	TreeGraphView graphView;

	[MenuItem("Window/DialogTreeEditor")]
	public static void ShowExample()
	{
		DialogTreeEditor wnd = GetWindow<DialogTreeEditor>();
		wnd.titleContent = new GUIContent("Dialog Tree Editor");
	}

	public void CreateGUI()
	{
		// Each editor window contains a root VisualElement object
		// VisualElement root = rootVisualElement;

		// VisualElements objects can contain other VisualElement following a tree hierarchy.
		// VisualElement label = new Label("Hello World! From C#");
		// root.Add(label);

		SetUpGraph();
		AddToolbar();
	}

	private void SetUpGraph()
	{
		graphView = new TreeGraphView();
		graphView.StretchToParentSize();
		rootVisualElement.Add(graphView);
		graphView.Init();
	}

	private void AddToolbar()
	{
		// Text entry field that we can enter a name for
		TextField filenameTextField = new TextField()
		{
			value = "Graph Name",
			label = "Filename"
		};

		// Button that invokes a save procedure when pressed
		Button saveButton = new Button()
		{
			text = "Save",
			clickable = new Clickable(() =>
			DialogTreeSavingAndLoading.Save(filenameTextField.value, graphView)),
		};
		// Alternative click event handling
		//saveButton.clicked += () => WeaponTreeSaveUtils.Save(filenameTextField.value);

		// Button that invokes a load procedure when pressed
		Button loadButton = new Button()
		{
			text = "Load",
			clickable = new Clickable(() =>
			OnLoad()),
		};

		// Button that invokes a clear procedure when pressed
		Button clearButton = new Button()
		{
			text = "Clear",
			clickable = new Clickable(() =>
			OnClear()),
		};

		// To add this to the top of the window, we can use a toolbar object
		Toolbar toolbar = new Toolbar();
		toolbar.Add(filenameTextField);
		toolbar.Add(saveButton);
		toolbar.Add(loadButton);
		toolbar.Add(clearButton);

		// Add the toolbar to the window
		rootVisualElement.Add(toolbar);
	}

	private void OnLoad()
	{
		OnClear();
		DialogTreeSavingAndLoading.Load(graphView);
	}

	private void OnClear()
	{
		rootVisualElement.Clear();
		CreateGUI();
	}

	public void LoadData(string path)
	{
		DialogTreeData saveData = AssetDatabase.LoadAssetAtPath<DialogTreeData>(path);


		DialogTreeSavingAndLoading.PopulateGraphViewFromAsset(graphView, saveData);
	}
}
