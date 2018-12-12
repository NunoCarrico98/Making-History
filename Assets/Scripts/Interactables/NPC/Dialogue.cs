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

	public List<string> GetDialogue(int i)
	{
		return Options[i - 1].OptionText;
	}

	public string GetButtonText(int i)
	{
		return Options[i].ButtonText;
	}
}

[System.Serializable]
public class DialogueList
{
    [SerializeField] private string _buttonText;

	[TextArea(3, 10)]
	[SerializeField] private List<string> _optionText;

    public string ButtonText => _buttonText;
    public List<string> OptionText => _optionText;
}

[System.Serializable]
public class DialogueOnlyList
{
	[TextArea(3, 10)]
	[SerializeField] private List<string> _onlyDialogue;

	public List<string> OnlyDialogue => _onlyDialogue;
}

[System.Serializable]
public class DialogueOnly
{
	[SerializeField] private DialogueOnlyList[] _questText;

	public DialogueOnlyList[] QuestText => _questText;

	public List<string> GetQuestDialogue(int i)
	{
		return QuestText[i].OnlyDialogue;
	}
}

