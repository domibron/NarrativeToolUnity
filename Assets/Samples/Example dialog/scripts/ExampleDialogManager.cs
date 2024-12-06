using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using TMPro;
using SmashKeyboardStudios.NarrativeTool.Data;

namespace SmashKeyboardStudios.NarrativeTool.Samples.ExampleDialog
{
	public class ExampleDialogManager : MonoBehaviour
	{
		// Static

		// Single so option prefabs can just call this singleton rather than getting the component.
		public static ExampleDialogManager Instance { get; private set; }


		// Public

		// That all mighty dialog data, this holds the text data for your dialog.
		public DialogTreeData Data;

		// The parent to get all childrent to clear as well as target parent to spawn options.
		public Transform OptionsParent;
		// The prefab for the options so we can add / remove how many we need.
		public GameObject OptionsPrefab;

		// The text box to display the current dialog.
		public TMP_Text DialogDisplay;


		// Private viariables.

		// We store the current node data so we can access it later.
		private DialogNodeSaveData _currentNode;



		#region Awake
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
		#endregion


		#region Start
		// Start is called before the first frame update
		void Start()
		{
			SetUpDialog();
		}
		#endregion


		#region SetUpDialog
		/// <summary>
		/// Sets up everything in the dialog manager.
		/// </summary>
		private void SetUpDialog()
		{
			_currentNode = Data.StartNode;

			UpdateDialog();
		}
		#endregion


		#region UpdateDialog
		/// <summary>
		/// Updates the dialog text and the options. This is called when there is a change and we need to display it.
		/// </summary>
		private void UpdateDialog()
		{
			// using the current node we get the dialog.
			DialogDisplay.text = _currentNode.Dialog;

			// we clear the children so we can then regenerate the option.
			Transform[] children = OptionsParent.GetComponentsInChildren<Transform>();
			foreach (Transform child in children)
			{
				if (child != OptionsParent)
				{
					Destroy(child.gameObject);
				}
			}

			// If there are not other dialog nodes, we know we have reached the end.
			// We create a special option so the player can exit the dialog.
			if (_currentNode.OutputConnectedGUIDs.Count <= 0)
			{
				GameObject option = Instantiate(OptionsPrefab, OptionsParent);

				option.GetComponent<OptionItem>().SetUpItem(-1, "DONE");

				// we then exit out as we dont want to attempt to generate any other options as there are none.
				return; // *    <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<    Return
			}

			// We create a new option prefab for every node this current node has connected outputs.
			// We also display the node name as the node option.
			for (int i = 0; i < _currentNode.OutputConnectedGUIDs.Count; i++)
			{
				GameObject option = Instantiate(OptionsPrefab, OptionsParent);

				option.GetComponent<OptionItem>().SetUpItem(i, Data.FindByGuid(StringToGUID(_currentNode.OutputConnectedGUIDs[i])).DialogNodeOptionText);
			}


		}
		#endregion


		#region SelectOption
		/// <summary>
		/// Used by the buttons to select the option it represents, 
		/// the int is stored on the button when they are generated.
		/// </summary>
		/// <param name="option">The number index for the option. 0 is first. -1 to call exit.</param>
		public void SelectOption(int option)
		{
			// Added a addition option for exiting the dialog.
			if (option == -1)
			{
				ExitDialog();

				return;
			}

			// we get the next not using the index int and store it as we need to use that data else where.
			_currentNode = Data.FindByGuid(StringToGUID(_currentNode.OutputConnectedGUIDs[option]));

			// Make sure we update the dialog text.
			UpdateDialog();
		}
		#endregion


		#region GUID StringToGUID
		/// <summary>
		/// Must needed function to turn dialog GUID strings into GUID which are used for getting nodes.
		/// </summary>
		/// <param name="str">The GUID as a string.</param>
		/// <returns>The GUID from the string.</returns>
		/// <exception cref="ArgumentException">Thrown when it cannot convert the sting into a GUID</exception>
		public static GUID StringToGUID(string str)
		{
			GUID guid;
			if (!GUID.TryParse(str, out guid))
			{
				throw new ArgumentException("Invalid GUID, cannot convert from string to GUID.");
			}
			return guid;
		}
		#endregion


		#region ExitDialog
		/// <summary>
		/// Used to close the dialog when it is over.
		/// </summary>
		public void ExitDialog()
		{
			// TODO Add functionality here.
			print("EXITED DIALOG");
		}
		#endregion
	}
}