using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogTreeEditor : EditorWindow
{
	[MenuItem("Window/DialogTreeEditor")]
	public static void ShowExample()
	{
		DialogTreeEditor wnd = GetWindow<DialogTreeEditor>();
		wnd.titleContent = new GUIContent("Dialog Tree Editor");
	}

	public void CreateGUI()
	{
		// Each editor window contains a root VisualElement object
		VisualElement root = rootVisualElement;

		// VisualElements objects can contain other VisualElement following a tree hierarchy.
		// VisualElement label = new Label("Hello World! From C#");
		// root.Add(label);

		SetUpGraph();
	}

	private void SetUpGraph()
	{
		TreeGraphView graphView = new TreeGraphView();
		graphView.StretchToParentSize();
		rootVisualElement.Add(graphView);
	}
}
