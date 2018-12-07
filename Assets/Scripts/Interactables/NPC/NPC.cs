using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
	#region Variables
	[SerializeField] private bool				_isInteractable = true;
	[SerializeField] private string				_interactionText;
	[SerializeField] private string				_requirementText;
	[SerializeField] private InventoryItem[]	_inventoryRequirements;
	[SerializeField] private Dialogue			_normalDialogue;
	[SerializeField] private Quest				_quest;

	private bool			_activateInventoryRequirements = false;
	private DialogueManager _dialogueManager;

	public string InteractionText => _interactionText;
	public string RequirementText => _requirementText;
	public InventoryItem[] InventoryRequirements => _inventoryRequirements;
	public Dialogue Dialogue => _normalDialogue;
	public NPCState NPCState { get; set; }

	public bool IsInteractable
	{
		get
		{
			return _isInteractable;
		}
		private set
		{
			_isInteractable = value;
		}
	}
	#endregion

	private void Awake()
	{
		_dialogueManager = DialogueManager.Instance;
	}

	private void OnEnable()
	{
		_dialogueManager.DialogueEnded += OnDialogueEnd;
	}

	private void OnDisable()
	{
		_dialogueManager.DialogueEnded -= OnDialogueEnd;
	}

	public void Interact()
	{
		Talk();
		PlayInteractAnimation();
	}

	public void PlayInteractAnimation()
	{
		Animator animator = GetComponent<Animator>();
		if (animator != null)
		{
			GetComponent<Animator>().SetTrigger("Interact");
		}
	}

	public void Talk()
	{
		// Start dialogue
		_dialogueManager.StartDialogue(Dialogue, NPCState);

		// Make sure player can't reinteract with the NPC
		IsInteractable = false;

		// Activate quest
		_activateInventoryRequirements = true;
	}

	public void SetNPCState()
	{
		if (NPCState == NPCState.Neutral)
			NPCState = NPCState.InQuestNoItems;
		else if (NPCState == NPCState.InQuestWithItems)
			NPCState = NPCState.AfterQuest;
		else if (NPCState == NPCState.InQuestNoItems)
			NPCState = NPCState.InQuestWithItems;
	}

	private void ManageObjectsAfterQuest()
	{
		DestroyObjectsAfterQuest();
		EnableObjectsAfterQuest();
	}

	private void DestroyObjectsAfterQuest()
	{
		if (_quest.destroyAfterQuest != null && NPCState == NPCState.AfterQuest)
			foreach(GameObject go in _quest.destroyAfterQuest)
				Destroy(go);
	}

	private void EnableObjectsAfterQuest()
	{
		if (_quest.enableAfterQuest != null && NPCState == NPCState.AfterQuest)
			foreach (GameObject go in _quest.enableAfterQuest)
				go.SetActive(true);
	}

	private void OnDialogueEnd()
	{
		IsInteractable = true;
		ManageObjectsAfterQuest();
	}
}
