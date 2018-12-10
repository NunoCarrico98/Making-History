using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField] private string _questName;
    [SerializeField] private List<IQuestGoal> _goals;

    public bool Completed { get; set; }
    public List<IQuestGoal> Goals => _goals;

    public void CheckForCompletion()
    {
        // Completed is true if all goals are true
        Completed = Goals.TrueForAll(g => g.Completed);
    }
    
}
