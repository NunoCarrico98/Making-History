using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
