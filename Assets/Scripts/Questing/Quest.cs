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

	[Header("Quest Consequences")]
	[SerializeField] private GameObject[] _destroyAfterQuest;
	[SerializeField] private GameObject[] _enableAfterQuest;

	public bool Completed { get; set; }
	public List<QuestGoal> Goals => _goals;
	public DialogueOnly QuestDialogue => _questDialogue;
	public GameObject[] DestroyAfterQuest => _destroyAfterQuest;
	public GameObject[] EnableAfterQuest => _enableAfterQuest;

	public void CheckForCompletion(IInteractable interactable)
	{
        if (!Completed)
        {
            Debug.Log("hello");
            foreach (QuestGoal goal in Goals)
            {
                Debug.Log(Goals.Count);
                goal.CheckForCompletion(interactable);
            }

            // Completed is true if all goals are true
            Completed = Goals.All(g => g.Completed);
        }
	}
}
