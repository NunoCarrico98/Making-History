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
}

[System.Serializable]
public class DialogueList
{
    [SerializeField] private string _buttonText;
    [SerializeField] [TextArea(3, 10)] private List<string> _optionText;

    public string ButtonText => _buttonText;
    public List<string> OptionText => _optionText;


}

