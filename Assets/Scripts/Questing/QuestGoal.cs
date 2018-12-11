using UnityEngine;
using System;

[System.Serializable]
public class QuestGoal
{
	/*[SerializeField] */public GoalType _goalType;
	/*[SerializeField] */public int _itemID;
	/*[SerializeField] */public int _requiredAmmount;

	private int _currentAmmount = 0;

	public bool Completed { get; set; }
	public GoalType Type
	{
		get { return _goalType; }
		set { _goalType = value; }
	}
	public int ItemID => _itemID;
	public int RequiredAmmount => _requiredAmmount;

	public virtual void CheckForCompletion(IInteractable obj)
	{
	}

	public enum GoalType
	{
		Collect,
		Speak
	}

	public static string[] GetEnumValuesAsStrings()
	{
		return Enum.GetNames(typeof(GoalType));
	}
}
