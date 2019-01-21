using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that deals with dialogue options.
/// </summary>
[System.Serializable]
public class Dialogue
{
    /* Create array of DialogueList, which is a class that holds the button text
     * and the dialogue text. It is done this way because creating an array of arrays
     * would not appear in the inspector. */
    [SerializeField] private DialogueList[] _options;

	/// <summary>
	/// Method that returns all the texts of the options.
	/// </summary>
	public DialogueList[] Options => _options;
	/// <summary>
	/// Method that returns the speaking text according to the button pressed.
	/// </summary>
	/// <param name="i">Int that defines the speaking text to return.</param>
	/// <returns>Speaking Text to write on the screen.</returns>
	public IEnumerable<string> GetDialogue(int i) => Options[i - 1].OptionText;
	/// <summary>
	/// Method that return the button text.
	/// </summary>
	/// <param name="i">Int that defines the button text to return.</param>
	/// <returns>Button Text to write on the screen.</returns>
	public string GetButtonText(int i) => Options[i].ButtonText;
}

