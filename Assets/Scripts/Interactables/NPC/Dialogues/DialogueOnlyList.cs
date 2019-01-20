using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueOnlyList
{
	[TextArea(3, 10)]
	[SerializeField] private List<string> _onlyDialogue;

	public IEnumerable<string> OnlyDialogue => _onlyDialogue;
}
