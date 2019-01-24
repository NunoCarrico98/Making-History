using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that defines all quest giver operations. Inherits from NPC.
/// </summary>
public class QuestGiver : NPC
{
	/// <summary>
	/// Variable that defines the NPC quest.
	/// </summary>
	[SerializeField] private Quest _quest;

	/// <summary>
	/// Property that defines if the quest is assigned.
	/// </summary>
	public bool IsQuestAssigned { get; private set; }
	/// <summary>
	/// Property that defines if quest in NPC is completed.
	/// </summary>
	public bool CompletedQuest { get; private set; }
	/// <summary>
	/// Property that returns the quest of the NPC.
	/// </summary>
	public Quest NPCQuest => _quest;

	/// <summary>
	/// Unity Awake Method.
	/// </summary>
	private void Awake()
	{
		// Find references
		player = FindObjectOfType<Player>();
		_dialogueManager = FindObjectOfType<DialogueManager>();
	}

	/// <summary>
	/// Method that sets the quest dialogue to be written.
	/// </summary>
	/// <param name="dialogueChosen">Index of the dialogue to be written.</param>
	/// <returns>Returns the dialogue to be written.</returns>
	public override IEnumerable<string> GetDialogue(int dialogueChosen)
	{
		// Collection to cache the dialogue before returning it
		IEnumerable<string> questDialogue = new List<string>();

		// If player chooses the quest dialogue option
		if (dialogueChosen == 1)
		{
			// Verify if quest is completed
			IsQuestCompleted();
			
			// If the quest is not active and not completed
			if (!IsQuestAssigned && !CompletedQuest)
			{
				// Get respective quest dialogue
				questDialogue = _quest.QuestDialogue.GetQuestDialogue(0);
				AssignQuest();
			}
			// If quest is active but not completed
			else if (IsQuestAssigned && !CompletedQuest)
				// Get respective quest dialogue
				questDialogue = _quest.QuestDialogue.GetQuestDialogue(1);
			// If quest is active and completed
			else if (IsQuestAssigned && CompletedQuest)
				// Get respective quest dialogue
				questDialogue = _quest.QuestDialogue.GetQuestDialogue(2);
		}
		// If player does not choose quest option, get dialogue normally
		else questDialogue = base.GetDialogue(dialogueChosen);

		// Return the dialogue
		return questDialogue;
	}

	/// <summary>
	/// Method that activates the gameobjects in the inventory requirements.
	/// </summary>
	public void ActivateRequirements()
	{
		if (InventoryRequirements != null)
			foreach (InventoryItem i in InventoryRequirements)
				i.IsActive = true;
	}

	/// <summary>
	/// Method that assigns the quest of the NPC.
	/// </summary>
	public void AssignQuest()
	{
		// If quest is not active
		if (!IsQuestAssigned)
		{
			// Add listener to player interaction event
			player.Interacted += _quest.IsComplete;
			IsQuestAssigned = true;
			// Activate inventory requirements
			ActivateRequirements();
			// Activate all static objects necessary fot the quest
			_quest.ActivateStaticObjects();
		}
	}

	/// <summary>
	/// Method that checks if the 
	/// </summary>
	public void IsQuestCompleted()
	{
		// if quest is completed
		if (_quest.Completed)
		{
			CompletedQuest = true;
			// Remove items from inventory
			DestroyRequirements();
			// If the NPC is needed to complete
			if (_quest.NeedsNPCToComplete)
				// Manage everything after the quest completion
				_quest.ManageAfterQuest();
		}
	}

	/// <summary>
	/// Method that removes the items required by the NPC from the 
	/// player inventory.
	/// </summary>
	public void DestroyRequirements()
	{
		// Cycle through the inventory
		for (int i = 0; i < player.Inventory.InventoryItems.Count; i++)
			// Cycle through the requirements of the NPC
			foreach (InventoryItem item in InventoryRequirements)
				// If the item is the same
				if (player.Inventory.InventoryItems[i] == item)
					// Remove from the inventory
					player.Inventory.RemoveFromInventory(item);
	}
}
