using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueList : MonoBehaviour 
{
	[SerializeField] private string _buttonText;

	[TextArea(3, 10)]
	[SerializeField] private List<string> _optionText;

	public string ButtonText => _buttonText;
	public IEnumerable<string> OptionText => _optionText;
}
