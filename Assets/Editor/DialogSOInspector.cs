using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using SmashKeyboardStudios.NarrativeTool.Data;
using UnityEngine;
using System.IO;

namespace SmashKeyboardStudios.NarrativeTool.Editor
{
	[CustomEditor(typeof(DialogTreeData))]
	public class DialogOSInspector : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (GUILayout.Button("Open"))
			{
				EditorApplication.ExecuteMenuItem("Window/DialogTreeEditor");

				EditorWindow DialogEditorWindow = EditorWindow.GetWindow(typeof(DialogTreeEditor));

				DialogTreeEditor DialogEditor = DialogEditorWindow as DialogTreeEditor;

				string path = AssetDatabase.GetAssetPath(target);

				DialogEditor.LoadData(path);
			}
		}
	}
}