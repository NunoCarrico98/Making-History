using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    /* Create array of DialogueList, which is a class that holds the button text
     * and the dialogue text. It is done this way because creating an array of arrays
     * would not appear in the inspector. */
    [SerializeField] private DialogueList[] _options;

    public DialogueList[] Options => _options;

	public IEnumerable<string> GetDialogue(int i) => Options[i - 1].OptionText;
	public string GetButtonText(int i) => Options[i].ButtonText;
}

