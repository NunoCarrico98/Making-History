using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    [SerializeField] private GoalType goalType;

    public bool Completed { get; set; }

    public virtual void CheckForCompletion(IInteractable obj)
    {
    }

    private enum GoalType
    {
        Collect,
        Speak
    }
}
