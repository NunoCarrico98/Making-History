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

/// <summary>
/// Class that creates a list of strings to be able to visualize them on the 
/// inpector.
/// </summary>
[System.Serializable]
public class DialogueList
{
	/// <summary>
	/// Text of the button.
	/// </summary>
	[SerializeField] private string _buttonText;

	/// <summary>
	/// List of the sentences to write in one interaction.
	/// </summary>
	[TextArea(3, 10)]
	[SerializeField] private List<string> _optionText;

	/// <summary>
	/// Method that returnthe button text.
	/// </summary>
    public string ButtonText => _buttonText;
	/// <summary>
	/// Method that returns the speaking text to write on screen.
	/// </summary>
    public IEnumerable<string> OptionText => _optionText;
}
/// <summary>
/// Class that creates a list of strings to be able to visualize them on the 
/// inpector.
/// </summary>
[System.Serializable]
public class DialogueOnlyList
{
	/// <summary>
	/// List of the sentences to write in one interaction.
	/// </summary>
	[TextArea(3, 10)]
	[SerializeField] private List<string> _onlyDialogue;

	/// <summary>
	/// List of the texts of all the quet states.
	/// </summary>
	public IEnumerable<string> OnlyDialogue => _onlyDialogue;
}

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

