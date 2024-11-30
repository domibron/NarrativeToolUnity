using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SmashKeyboardStudios.NarrativeTool
{
	public class OptionItem : MonoBehaviour
	{
		public TMP_Text OptionText;

		public int Option;

		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}

		public void SetUpItem(int option, string optionText)
		{
			Option = option;
			OptionText.text = optionText;
		}

		public void OnClicked()
		{
			DialogManager.Instance.SelectOption(Option);
		}


	}
}