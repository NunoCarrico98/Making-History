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

	public void CheckForCompletion(IInteractable interactable)
	{
		if (!Completed)
		{
			CheckForType(interactable);
		}
	}

	public void CheckForType(IInteractable interactable)
	{
		switch (_goalType)
		{
			case GoalType.Collect:
				CollectComplete(interactable);
				break;
			case GoalType.Use:
				UseComplete(interactable);
				break;
		}
	}

	private void CollectComplete(IInteractable interactable)
	{
		if (interactable is InventoryItem)
		{
			InventoryItem item = interactable as InventoryItem;
			if (item.ID == _itemID)
			{
				_currentAmmount++;
				if (_currentAmmount == _requiredAmmount)
				{
					Completed = true;
				}
			}
		}
	}

	private void UseComplete(IInteractable interactable)
	{
		if (interactable is StaticInteractable)
			if (Player.Instance.Inventory.HasRequirements(interactable))
				Completed = true;
	}

	public enum GoalType
	{
		Collect,
		Use,
	}

	public static string[] GetEnumValuesAsStrings()
	{
		return Enum.GetNames(typeof(GoalType));
	}
}
