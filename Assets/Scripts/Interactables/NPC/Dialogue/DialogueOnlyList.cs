using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
