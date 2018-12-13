using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField] private StaticInteractable[] _staticObjectsToActivate;
    [SerializeField] private Quest _quest;

    [Header("Testing Only")]
    [SerializeField] private bool _assignedQuest;
    [SerializeField] private bool _completedQuest;
    private Player player;

    public bool CompletedQuest => _completedQuest;
    public bool AfterQuest { get; private set; }
    public Quest NPCQuest => _quest;

    private void Awake()
    {
        player = Player.Instance;
    }

    private void OnEnable()
    {
       // player.Interacted += _quest.CheckForCompletion;
    }

    private void OnDisable()
    {
        //player.Interacted -= _quest.CheckForCompletion;
    }

    public override List<string> GetDialogue(int dialogueChosen)
    {
        List<string> questDialogue = new List<string>();

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

	public void AssignQuest()
    {
        if (DialogueManager.Instance.DialogueChosen == 1)
        {
            if (!_assignedQuest)
            {
				player.Interacted += _quest.CheckForCompletion;
                _assignedQuest = true;
                ActivateRequirements();
                ActivateStaticObjects();
            }
        }
    }

    public void CompleteQuest()
    {
        if (DialogueManager.Instance.DialogueChosen == 1)
        {
            if (_quest.Completed && player.InventoryItems.HasRequirements(this))
            {
                _completedQuest = true;
				UpdateStaticObjects();
                DestroyRequirements();
				ManageObjectsAfterQuest();
            }
        }
    }

    public void DestroyRequirements()
    {
		for (int i = 0; i < player.InventoryItems.InventoryItems.Count; i++)
			foreach(InventoryItem item in InventoryRequirements)
				if (player.InventoryItems.InventoryItems[i] == item)
				{
					player.InventoryItems.RemoveFromInventory(item);
				}
    }

    private void ManageObjectsAfterQuest()
    {
        DestroyObjectsAfterQuest();
        EnableObjectsAfterQuest();
    }

    private void DestroyObjectsAfterQuest()
    {
        if (_quest.DestroyAfterQuest != null)
            foreach (GameObject go in _quest.DestroyAfterQuest)
                Destroy(go);
    }

    private void EnableObjectsAfterQuest()
    {
        if (_quest.EnableAfterQuest != null)
            foreach (GameObject go in _quest.EnableAfterQuest)
                go.SetActive(true);
    }

}
