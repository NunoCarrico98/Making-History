using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that deals with the quest dialogue options.
/// </summary>
[System.Serializable]
public class DialogueOnly
{
	/* Create array of DialogueOnlyList, which is a class that holds the button text
     * and the dialogue text. It is done this way because creating an array of arrays
     * would not appear in the inspector. */
	[SerializeField] private DialogueOnlyList[] _questText;

	/// <summary>
	/// Method that returns all the texts of the quest.
	/// </summary>
	public DialogueOnlyList[] QuestText => _questText;
	/// <summary>
	/// Method that return the text according to the current state of the quest.
	/// </summary>
	/// <param name="i">Int that defines the text to return.</param>
	/// <returns>Text to write on the screen.</returns>
	public IEnumerable<string> GetQuestDialogue(int i) => QuestText[i].OnlyDialogue;
}
