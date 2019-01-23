using UnityEngine;
using System;

/// <summary>
/// Class that handles the all goal types and it's completions.
/// </summary>
[System.Serializable]
public class QuestGoal
{
	/// <summary>
	/// Variabe that indicates the type of goal this is.
	/// </summary>
	[SerializeField] private GoalType _goalType;
	/// <summary>
	/// Item ID that identifies the item necessary for the collection goal 
	/// completion.
	/// </summary>
	[SerializeField] private int _itemID;
	/// <summary>
	/// Required ammount of necessary items for the colection goal.
	/// </summary>
	[SerializeField] private int _requiredAmmount;

	/// <summary>
	/// Current ammount of necessary items the player has.
	/// </summary>
	private int _currentAmmount = 0;

	/// <summary>
	/// Indicates if goal is completed or not.
	/// </summary>
	public bool Completed { get; set; }

	/// <summary>
	/// Method that checks if Goal is completed.
	/// </summary>
	/// <param name="interactable">Interactable to check if goals are 
	/// complete.</param>
	public void CheckForCompletion(IInteractable interactable)
	{
		// If goal is not completed
		if (!Completed)
		{
			// Check for completion
			CheckForType(interactable);
		}
	}

	/// <summary>
	/// Method that checks the goal type from the quest.
	/// </summary>
	/// <param name="interactable">Interactable to check if goals are 
	/// complete.</param>
	public void CheckForType(IInteractable interactable)
	{
		// Verify goal type
		switch (_goalType)
		{
			case GoalType.Collect:
				// Check for collection completion
				CollectComplete(interactable);
				break;
			case GoalType.Use:
				// Check for use completion
				UseComplete(interactable);
				break;
		}
	}

	/// <summary>
	/// Method that deals with the colection goal.
	/// </summary>
	/// <param name="interactable">Interactable to check if collection goal is 
	/// complete.</param>
	private void CollectComplete(IInteractable interactable)
	{
		if (interactable is InventoryItem)
		{
			// Get interactable as Item
			InventoryItem item = interactable as InventoryItem;
			// If the item has the ID the quest goal is looking for
			if (item.ID == _itemID)
			{
				// Increase ammount
				_currentAmmount++;
				// If the required ammount has been caught
				if (_currentAmmount == _requiredAmmount)
					// Complete goal
					Completed = true;
			}
		}
	}

	/// <summary>
	/// Method that deals with the use goal.
	/// </summary>
	/// <param name="interactable">Interactable to check if use goal is 
	/// complete.</param>
	private void UseComplete(IInteractable interactable)
	{
		if (interactable is StaticInteractable)
			// If player has the required items to interact with the interactable
			if (Player.Instance.Inventory.HasRequirements(interactable))
				// Complete goal
				Completed = true;
	}

	/// <summary>
	/// Method to get the names of all of the GoalType options.
	/// </summary>
	/// <returns>Return the names of al goal types in an array.</returns>
	public static string[] GetEnumValuesAsStrings()
	{
		// Returns the names of al goal types in an array
		return Enum.GetNames(typeof(GoalType));
	}
}
