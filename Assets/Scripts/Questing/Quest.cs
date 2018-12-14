using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Quest
{
    [SerializeField] private string _questName;
    [SerializeField] private bool _isActive;
    [SerializeField] private QuestGiver[] _unlockQuests;
    [Header("Quest Consequences")]
    [SerializeField] private GameObject[] _destroyAfterQuest;
    [SerializeField] private GameObject[] _enableAfterQuest;

    [Header("Quest Goals")]
    [SerializeField] private List<QuestGoal> _goals;

    [Header("Quest Dialogue")]
    [SerializeField] private DialogueOnly _questDialogue;

    public bool Completed { get; set; }
    public bool IsActive
    {
        get { return _isActive; }
        set { _isActive = value; }
    }
    public QuestGiver[] UnlockQuests => _unlockQuests;
    public List<QuestGoal> Goals => _goals;
    public DialogueOnly QuestDialogue => _questDialogue;
    public GameObject[] DestroyAfterQuest => _destroyAfterQuest;
    public GameObject[] EnableAfterQuest => _enableAfterQuest;

    public void CheckForCompletion(IInteractable interactable)
    {
        if (!Completed)
        {
            foreach (QuestGoal goal in Goals)
            {
                goal.CheckForCompletion(interactable);
            }

            // Completed is true if all goals are true
            Completed = Goals.All(g => g.Completed);
        }

        IsComplete();
    }

    public void IsComplete()
    {
        if (Completed)
            Player.Instance.Interacted -= CheckForCompletion;
    }
}
