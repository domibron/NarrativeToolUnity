using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SmashKeyboardStudios.NarrativeTool.Samples.ExampleDialog
{
	public class OptionItem : MonoBehaviour
	{
		public TMP_Text OptionText;

		public int Option;



		public void SetUpItem(int option, string optionText)
		{
			Option = option;
			OptionText.text = optionText;
		}

		public void OnClicked()
		{
			ExampleDialogManager.Instance.SelectOption(Option);
		}


	}
}