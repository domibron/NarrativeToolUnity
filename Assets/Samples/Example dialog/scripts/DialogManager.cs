using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using TMPro;
using SmashKeyboardStudios.NarrativeTool.Data;

namespace SmashKeyboardStudios.NarrativeTool
{
	public class DialogManager : MonoBehaviour
	{
		public static DialogManager Instance { get; private set; }

		public DialogTreeData data;

		public Transform OptionsParent;
		public GameObject OptionsPrefab;

		public TMP_Text DialogDisplay;

		private DialogSaveData _currentNode;

		// private bool _moreThanOneOption = false;

		// private List<string> GUIDOfOptions = new List<string>();

		void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(this);
			}
			else
			{
				Instance = this;
			}
		}

		// Start is called before the first frame update
		void Start()
		{
			SetUpDialog();
		}

		// Update is called once per frame
		void Update()
		{

		}

		public void SetUpDialog()
		{
			_currentNode = data.StartNode;

			UpdateDialog();
		}


		public void UpdateDialog()
		{
			// GUIDOfOptions.Clear();

			DialogDisplay.text = _currentNode.Dialog;

			Transform[] children = OptionsParent.GetComponentsInChildren<Transform>();


			foreach (Transform child in children)
			{
				if (child != OptionsParent)
				{
					Destroy(child.gameObject);
				}
			}

			if (_currentNode.OutputConnectedGUIDs.Count <= 0)
			{
				GameObject option = Instantiate(OptionsPrefab, OptionsParent);

				option.GetComponent<OptionItem>().SetUpItem(-1, "DONE");

				return; // *    <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<    Return
			}

			for (int i = 0; i < _currentNode.OutputConnectedGUIDs.Count; i++)
			{
				GameObject option = Instantiate(OptionsPrefab, OptionsParent);

				option.GetComponent<OptionItem>().SetUpItem(i, data.FindByGuid(StringToGUID(_currentNode.OutputConnectedGUIDs[i])).DialogNodeName);
			}


		}

		public void SelectOption(int option)
		{

			if (option == -1)
			{
				ExitDialog();

				return;
			}

			_currentNode = data.FindByGuid(StringToGUID(_currentNode.OutputConnectedGUIDs[option]));

			UpdateDialog();
		}

		public static GUID StringToGUID(string str)
		{
			GUID guid;
			if (!GUID.TryParse(str, out guid))
			{
				throw new ArgumentException("Invalid GUID, cannot convert from string to GUID.");
			}
			return guid;
		}

		public void ExitDialog()
		{
			print("EXITED DIALOG");
		}
	}
}