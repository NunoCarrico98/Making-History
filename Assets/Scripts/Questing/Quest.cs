using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Quest
{
	[SerializeField] private string _questName;
	[SerializeField] private bool _isActive;
	[SerializeField] private bool _needsNPCToComplete = false;
	[SerializeField] private StaticInteractable[] _staticObjectsToActivate;
	[SerializeField] private InventoryItem[] _questRewards;
	[SerializeField] private QuestGiver[] _unlockQuests;
	[SerializeField] private GameObject[] _destroyAfterQuest;
	[SerializeField] private GameObject[] _enableAfterQuest;
	[SerializeField] private List<QuestGoal> _questGoals;
	[SerializeField] private DialogueOnly _questDialogue;

	public bool Completed { get; set; }
	public bool IsActive
	{
		get { return _isActive; }
		set { _isActive = value; }
	}
	public bool NeedsNPCToComplete => _needsNPCToComplete;
	public List<QuestGoal> Goals => _questGoals;
	public DialogueOnly QuestDialogue => _questDialogue;
	public GameObject[] DestroyAfterQuest => _destroyAfterQuest;
	public GameObject[] EnableAfterQuest => _enableAfterQuest;

	public void CheckForCompletion(IInteractable interactable)
	{
		if (!Completed)
		{
			foreach (QuestGoal goal in Goals)
			{
				goal.CheckForCompletion(interactable);
			}

			// Completed is true if all goals are true
			Completed = Goals.All(g => g.Completed);
		}

		IsComplete();
	}

	public void IsComplete()
	{
		if (Completed)
		{
			Player.Instance.Interacted -= CheckForCompletion;
			if (!_needsNPCToComplete)
			{
				GiveQuestRewards();
				UpdateStaticObjects();
				UnlockQuests();
				ManageObjectsAfterQuest();
			}
		}
	}

	private void ChangeScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void UnlockQuests()
	{
		if (_unlockQuests != null)
			foreach (QuestGiver q in _unlockQuests)
				q.NPCQuest.IsActive = true;
	}

	public void ActivateStaticObjects()
	{
		if (_staticObjectsToActivate != null)
			foreach (StaticInteractable si in _staticObjectsToActivate)
				si.IsActive = true;
	}

	public void UpdateStaticObjects()
	{
		if (_staticObjectsToActivate != null)
			foreach (StaticInteractable si in _staticObjectsToActivate)
				si.AfterQuest = true;
	}

	public void GiveQuestRewards()
	{
		if (_questRewards != null)
			foreach (InventoryItem si in _questRewards)
				Player.Instance.InventoryItems.AddToInventory(si);
	}

	public void ManageObjectsAfterQuest()
	{
		DestroyObjectsAfterQuest();
		EnableObjectsAfterQuest();
	}

	private void DestroyObjectsAfterQuest()
	{
		if (DestroyAfterQuest != null)
			foreach (GameObject go in DestroyAfterQuest)
				Object.Destroy(go);
	}

	private void EnableObjectsAfterQuest()
	{
		if (EnableAfterQuest != null)
			foreach (GameObject go in EnableAfterQuest)
				go.SetActive(true);
	}
}
