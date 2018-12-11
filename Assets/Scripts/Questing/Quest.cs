using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

[System.Serializable]
public class Quest
{
	public string _questName;
	public List<QuestGoal> _goals;

	public bool Completed { get; set; }
	public List<QuestGoal> Goals => _goals;

	public void CheckForCompletion()
	{
		foreach (QuestGoal goal in Goals)
		{
			//goal.CheckForCompletion
		}
		// Completed is true if all goals are true
		Completed = Goals.All(g => g.Completed);
	}
}
