using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueOnly
{
	/* Create array of DialogueOnlyList, which is a class that holds the button text
     * and the dialogue text. It is done this way because creating an array of arrays
     * would not appear in the inspector. */
	[SerializeField] private DialogueOnlyList[] _questText;

	public DialogueOnlyList[] QuestText => _questText;
	public IEnumerable<string> GetQuestDialogue(int i) => QuestText[i].OnlyDialogue;
}