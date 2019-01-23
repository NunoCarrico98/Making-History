using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
	[SerializeField] private Quest _quest;

	[Header("Testing Only")]
	[SerializeField] private bool _assignedQuest;
	[SerializeField] private bool _completedQuest;

	public bool CompletedQuest => _completedQuest;
	public bool AfterQuest { get; private set; }
	public Quest NPCQuest => _quest;

	private void Awake()
	{
		player = FindObjectOfType<Player>();
		_dialogueManager = FindObjectOfType<DialogueManager>();
	}

	public override IEnumerable<string> GetDialogue(int dialogueChosen)
	{
		IEnumerable<string> questDialogue = new List<string>();

		if (dialogueChosen == 1)
		{
			CompleteQuest();

			if (!_assignedQuest && !_completedQuest)
			{
				questDialogue = _quest.QuestDialogue.GetQuestDialogue(0);
				AssignQuest();
			}
			else if (_assignedQuest && !_completedQuest)
				questDialogue = _quest.QuestDialogue.GetQuestDialogue(1);
			else if (_assignedQuest && _completedQuest)
			{
				questDialogue = _quest.QuestDialogue.GetQuestDialogue(2);
				AfterQuest = true;
			}
		}
		else questDialogue = base.GetDialogue(dialogueChosen);

		return questDialogue;
	}

	public void ActivateRequirements()
	{
		if (InventoryRequirements != null)
			foreach (InventoryItem i in InventoryRequirements)
				i.IsActive = true;
	}

	public void AssignQuest()
	{
		if (!_assignedQuest)
		{
			player.Interacted += _quest.IsComplete;
			_assignedQuest = true;
			ActivateRequirements();
			_quest.ActivateStaticObjects();
		}
	}

	public void CompleteQuest()
	{
		if (_quest.Completed)
		{
			_completedQuest = true;
			DestroyRequirements();
			if (_quest.NeedsNPCToComplete)
			{
				_quest.GiveQuestRewards();
				_quest.UpdateStaticObjects();
				_quest.UnlockQuests();
				_quest.ManageObjectsAfterQuest();
			}
		}
	}

	public void DestroyRequirements()
	{
		for (int i = 0; i < player.Inventory.InventoryItems.Count; i++)
			foreach (InventoryItem item in InventoryRequirements)
				if (player.Inventory.InventoryItems[i] == item)
					player.Inventory.RemoveFromInventory(item);
	}
}
