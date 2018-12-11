using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Quest
{
	[SerializeField] private string _questName;
	[SerializeField] private List<QuestGoal> _goals;

	[Header("Quest Dialogue")]
	[SerializeField] private DialogueOnly _questDialogue;

	[Header("Testing Only")]
	[SerializeField] private bool completed;

	public bool Completed { get; set; }
	public List<QuestGoal> Goals => _goals;
	public DialogueOnly QuestDialogue => _questDialogue;

	public void CheckForCompletion(IInteractable interactable)
	{
		foreach (QuestGoal goal in Goals)
		{
			goal.CheckForCompletion(interactable);
		}
		// Completed is true if all goals are true
		Completed = Goals.All(g => g.Completed);
		completed = Completed;
	}
}
