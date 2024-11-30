using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SmashKeyboardStudios.NarrativeTool.Editor
{
	[CreateAssetMenu(fileName = "New Dialog", menuName = "NarrativeToolUnity/SO_Dialog")]
	public class SO_Dialog : ScriptableObject
	{
		[SerializeField] string _dialogName;
		[SerializeField, TextArea(5, 500)] string _dialog;

		public string DialogName { get { return _dialogName; } }
		public string Dialog { get { return _dialog; } }
	}
}
