using UnityEngine;
using System;

[System.Serializable]
public class QuestGoal
{
	[SerializeField] private GoalType _goalType;
	[SerializeField] private int _itemID;
	[SerializeField] private int _requiredAmmount;

	private int _currentAmmount = 0;

	public bool Completed { get; set; }
	public int ItemID => _itemID;
	public int RequiredAmmount => _requiredAmmount;

	public void CheckForCompletion(IInteractable interactable)
	{
		CheckForType(interactable);
	}

	public void CheckForType(IInteractable interactable)
	{
		switch (_goalType)
		{
			case GoalType.Collect:
				InventoryItem item = interactable as InventoryItem;
				if (item.ID == ItemID)
				{
					_currentAmmount++;

					if (_currentAmmount == _requiredAmmount)
						Completed = true;
				}
				break;
			case GoalType.Speak:
				break;
		}
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
