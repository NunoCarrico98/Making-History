using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class that manages all quest operations.
/// </summary>
[System.Serializable]
public class Quest
{
	/// <summary>
	/// String indicating the quest name.
	/// </summary>
	[SerializeField] private string _questName;
	/// <summary>
	/// Bool that indicates if the quest is active.
	/// </summary>
	[SerializeField] private bool _isActive;
	/// <summary>
	/// Bool that indicates if an NPC is needed to complete the quest.
	/// </summary>
	[SerializeField] private bool _needsNPCToComplete = false;
	/// <summary>
	/// All static interactable to be activated after quest activation and 
	/// updated after quest completion.
	/// </summary>
	[SerializeField] private StaticInteractable[] _staticObjectsToActivate;
	/// <summary>
	/// All rewards to be given to the player after quest completion.
	/// </summary>
	[SerializeField] private InventoryItem[] _questRewards;
	/// <summary>
	/// All other quests to be unlocked after quest completion.
	/// </summary>
	[SerializeField] private QuestGiver[] _unlockQuests;
	/// <summary>
	/// All gameobjects to destroy after the quest completion.
	/// </summary>
	[SerializeField] private GameObject[] _destroyAfterQuest;
	/// <summary>
	/// All gameobjects to enable after the quest completion.
	/// </summary>
	[SerializeField] private GameObject[] _enableAfterQuest;
	/// <summary>
	/// All quest goals to complete.
	/// </summary>
	[SerializeField] private List<QuestGoal> _questGoals;
	/// <summary>
	/// All Quest Dialogue.
	/// </summary>
	[SerializeField] private DialogueOnly _questDialogue;

	/// <summary>
	/// Property that returns if the quest is completed.
	/// </summary>
	public bool Completed { get; set; }
	/// <summary>
	/// Property that defines if the quest is active.
	/// </summary>
	public bool IsActive
	{
		get { return _isActive; }
		set { _isActive = value; }
	}
	/// <summary>
	/// Property that returns if an NPC is needed to complete the quest.
	/// </summary>
	public bool NeedsNPCToComplete => _needsNPCToComplete;
	/// <summary>
	/// Property that returns the list with all quest goals
	/// </summary>
	public List<QuestGoal> Goals => _questGoals;
	/// <summary>
	/// Property that returns the quest dialogue
	/// </summary>
	public DialogueOnly QuestDialogue => _questDialogue;
	/// <summary>
	/// Property that returns all Gameobjects to destroy after quest.
	/// </summary>
	public GameObject[] DestroyAfterQuest => _destroyAfterQuest;
	/// <summary>
	/// Property that returns all Gameobjects to enable after quest.
	/// </summary>
	public GameObject[] EnableAfterQuest => _enableAfterQuest;

	/// <summary>
	/// Method that checks if quest is complete or not.
	/// </summary>
	/// <param name="interactable">Current interactable.</param>
	public void IsComplete(IInteractable interactable)
	{
		if (!Completed)
			CheckForCompletion(interactable);
	}

	/// <summary>
	/// Method that checks quest completion, checking every quest goal.
	/// </summary>
	/// <param name="interactable">Current interactable.</param>
	public void CheckForCompletion(IInteractable interactable)
	{
		foreach (QuestGoal goal in Goals)
			goal.CheckForCompletion(interactable);

		// Completed is true if all goals are true
		Completed = Goals.All(g => g.Completed);

		// Manage quest consequences
		ManageAfterQuest();
	}

	/// <summary>
	/// Method that deals with quest aftermath.
	/// </summary>
	public void ManageAfterQuest()
	{
		//if (Completed)
		//{
			if (!_needsNPCToComplete)
			{
				// Remove the quest from being a listener to the player interactions
				Player.Instance.Interacted -= IsComplete;
				// Give rewards
				GiveQuestRewards();
				// Unlock other quests
				UnlockQuests();
				// Manage objects after quest
				ManageObjectsAfterQuest();
			}
		//}
	}

	/// <summary>
	/// Method that enables other quest after quest completion.
	/// </summary>
	public void UnlockQuests()
	{
		if (_unlockQuests != null)
			foreach (QuestGiver q in _unlockQuests)
				q.NPCQuest.IsActive = true;
	}

	/// <summary>
	/// Method that activates given objects after quest activation.
	/// </summary>
	public void ActivateStaticObjects()
	{
		if (_staticObjectsToActivate != null)
			foreach (StaticInteractable si in _staticObjectsToActivate)
				si.IsActive = true;
	}

	/// <summary>
	/// Method that gives the quest rewards to the player after quest completion.
	/// </summary>
	public void GiveQuestRewards()
	{
		if (_questRewards != null)
			foreach (InventoryItem si in _questRewards)
				Player.Instance.Inventory.AddToInventory(si);
	}

	/// <summary>
	/// Method that manages the given objects after a quest is completed.
	/// </summary>
	public void ManageObjectsAfterQuest()
	{
		UpdateStaticObjects();
		DestroyObjectsAfterQuest();
		EnableObjectsAfterQuest();
	}

	/// <summary>
	/// Method that updates all given objects after quest completion.
	/// </summary>
	private void UpdateStaticObjects()
	{
		if (_staticObjectsToActivate != null)
			foreach (StaticInteractable si in _staticObjectsToActivate)
				si.AfterQuest = true;
	}

	/// <summary>
	/// Method that destroys all given objects after quest completion.
	/// </summary>
	private void DestroyObjectsAfterQuest()
	{
		if (DestroyAfterQuest != null)
			foreach (GameObject go in DestroyAfterQuest)
				Object.Destroy(go);
	}

	/// <summary>
	/// Method that enables all given objects after quest completion.
	/// </summary>
	private void EnableObjectsAfterQuest()
	{
		if (EnableAfterQuest != null)
			foreach (GameObject go in EnableAfterQuest)
				go.SetActive(true);
	}
}
