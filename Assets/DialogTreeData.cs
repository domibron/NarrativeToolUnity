using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogSO
{


	// ! REMOVE THIS, USER CANNOT MAKE THIS ASSET! IT MUST BE GENERATED!
	[CreateAssetMenu(menuName = "NarrativeToolUnity/DialogTreeData")]
	public class DialogTreeData : ScriptableObject
	{
		[HideInInspector]
		public List<DialogSaveData> DialogNodes = new List<DialogSaveData>();

		[HideInInspector]
		public DialogSaveData StartNode;

		// ? might cause build error.
		public DialogSaveData FindByGuid(GUID guid)
		{
			foreach (var dialogNode in DialogNodes)
				if (dialogNode.DialogNodeGUID == guid.ToString())
					return dialogNode;
			return null;
		}
	}

	[System.Serializable]
	public class DialogSaveData
	{
		[HideInInspector] public string DialogNodeGUID;
		[HideInInspector] public string DialogNodeName;
		[HideInInspector] public string Dialog;
		[HideInInspector] public Vector2 Position;
		[HideInInspector] public List<string> InputConnectedGUIDs = new List<string>();
		[HideInInspector] public List<string> OutputConnectedGUIDs = new List<string>();
		[HideInInspector] public bool IsStart = false;

		public void Init(GUID guid, Vector2 pos, string dialogName, string dialog, bool isStart = false)
		{
			DialogNodeGUID = guid.ToString();
			Position = pos;
			DialogNodeName = dialogName;
			Dialog = dialog;
			IsStart = isStart;
		}

		public List<string> ConvertGUIDListToString(List<GUID> listToConvert)
		{
			if (listToConvert.Count <= 0) throw new ArgumentOutOfRangeException(nameof(listToConvert));

			List<string> returnedList = new List<string>();

			foreach (var guid in listToConvert)
			{
				returnedList.Add(guid.ToString());
			}

			return returnedList;
		}

		public void SetInput(List<string> inputConnectedGUIDs)
		{
			InputConnectedGUIDs = inputConnectedGUIDs;
		}

		public void SetInput(List<GUID> inputConnectedGUIDs)
		{
			InputConnectedGUIDs = ConvertGUIDListToString(inputConnectedGUIDs);
		}

		public void SetOutput(List<string> outputConnectedGUIDs)
		{
			OutputConnectedGUIDs = outputConnectedGUIDs;
		}

		public void SetOutput(List<GUID> outputConnectedGUIDs)
		{
			OutputConnectedGUIDs = ConvertGUIDListToString(outputConnectedGUIDs);
		}

		public void SetOutput(GUID outputConnectedGUID)
		{
			OutputConnectedGUIDs.Clear();
			OutputConnectedGUIDs.Add(outputConnectedGUID.ToString());
		}
	}
}
