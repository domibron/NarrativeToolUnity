using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SmashKeyboardStudios.NarrativeTool.Editor
{
	/// <summary>
	/// Not used, was going to be the original way of saving node dialog, but decided to take another method.
	/// </summary>
	[Obsolete("This is no longer needed or used.", true), CreateAssetMenu(fileName = "New Dialog", menuName = "NarrativeToolUnity/SO_Dialog")]
	public class SO_Dialog : ScriptableObject
	{
		[SerializeField] string _dialogName;
		[SerializeField, TextArea(5, 500)] string _dialog;

		public string DialogName { get { return _dialogName; } }
		public string Dialog { get { return _dialog; } }
	}
}
