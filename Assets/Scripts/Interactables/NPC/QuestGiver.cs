using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
	[SerializeField] private Quest _quest;

	[Header("Testing Only")]
	[SerializeField] private bool _assigned;
	[SerializeField] private bool _completed;
	private Player player;

	public Quest NPCQuest => _quest;

	private void Awake()
	{
		player = Player.Instance;
	}

	private void OnEnable()
	{
		player.Interacted += _quest.CheckForCompletion;
	}

	private void OnDisable()
	{
		player.Interacted -= _quest.CheckForCompletion;
	}

	public void CheckQuestState()
	{
		if (!_assigned && _dialogueManager.DialogueChosen == 1)
		{
			_assigned = true;
		}
		else if (_quest.Completed)
		{
			_completed = true;
			_quest = null;
		}
	}

	public override List<string> GetDialogue(int dialogueChosen)
	{
		List<string> questDialogue = new List<string>();

		if (dialogueChosen == 1 || _quest != null)
		{
			if (!_assigned && !_completed)
			{
				questDialogue = _quest.QuestDialogue.GetQuestDialogue(0);
			}
			else if (_assigned && !_completed)
			{
				questDialogue = _quest.QuestDialogue.GetQuestDialogue(1);
			}
			else if (_assigned && _completed)
			{
				questDialogue = _quest.QuestDialogue.GetQuestDialogue(2);
			}
			else if (_quest == null)
			{
				questDialogue = _quest.QuestDialogue.GetQuestDialogue(3);
			}
		}
		else questDialogue = base.GetDialogue(dialogueChosen);

		return questDialogue;
	}

	private void ManageObjectsAfterQuest()
	{
		DestroyObjectsAfterQuest();
		EnableObjectsAfterQuest();
	}

	private void DestroyObjectsAfterQuest()
	{
		//if (_quest.destroyAfterQuest != null && NPCState == NPCState.AfterQuest)
		//	foreach(GameObject go in _quest.destroyAfterQuest)
		//		Destroy(go);
	}

	private void EnableObjectsAfterQuest()
	{
		//if (_quest.enableAfterQuest != null && NPCState == NPCState.AfterQuest)
		//	foreach (GameObject go in _quest.enableAfterQuest)
		//		go.SetActive(true);
	}
}
