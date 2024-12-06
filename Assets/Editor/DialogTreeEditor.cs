using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using SmashKeyboardStudios.NarrativeTool.Data;

namespace SmashKeyboardStudios.NarrativeTool.Editor
{
	/// <summary>
	/// The base for the dialog tree editor.
	/// </summary>
	public class DialogTreeEditor : EditorWindow
	{
		TreeGraphView _graphView;

		private string _graphName = "New Dialog Tree";

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
			_graphView = new TreeGraphView();
			_graphView.StretchToParentSize();
			rootVisualElement.Add(_graphView);
			_graphName = "New Dialog Tree";
			_graphView.Init();
		}

		private void AddToolbar()
		{
			// Text entry field that we can enter a name for
			TextField filenameTextField = new TextField()
			{
				value = _graphName,
				label = "Filename"
			};

			// Button that invokes a save procedure when pressed
			Button saveButton = new Button()
			{
				text = "Save",
				clickable = new Clickable(() =>
				DialogTreeSavingAndLoading.Save(filenameTextField.value, _graphView)),
			};

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
			string name = DialogTreeSavingAndLoading.Load(_graphView);
			_graphName = name;
		}

		private void OnClear()
		{
			rootVisualElement.Clear();
			_graphName = "New Dialog Tree";
			CreateGUI();
		}

		public void LoadData(string path)
		{
			DialogTreeData saveData = AssetDatabase.LoadAssetAtPath<DialogTreeData>(path);

			foreach (var item in rootVisualElement.Children())
			{
				if (item.GetType() == typeof(Toolbar))
				{
					foreach (var item2 in item.Children())
					{
						if (item2.GetType() == typeof(TextField))
						{
							((TextField)item2).value = saveData.name;


						}
					}
				}
			}


			DialogTreeSavingAndLoading.PopulateGraphViewFromAsset(_graphView, saveData);
		}
	}
}