using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
	[SerializeField] private bool				_isInteractable = true;
	[SerializeField] private string				_NPCName;
	[SerializeField] private string				_interactionText;
	[SerializeField] private string				_requirementText;
	[SerializeField] private InventoryItem[]	_inventoryRequirements;

	[Header("Non-Quest Dialogue")]
	[SerializeField] private Dialogue _dialogue;

	[Header("Multiple Choices Buttons Text")]
	[SerializeField] private string[] _optionsText;

	private DialogueManager _dialogueManager;

	public string NPCName						 => _NPCName;
	public string InteractionText				 => _interactionText;
	public string RequirementText				 => _requirementText;
	public InventoryItem[] InventoryRequirements => _inventoryRequirements;
	public Dialogue Dialogue					 => _dialogue;

	public string[] OptionsText					 => _optionsText;

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
		_dialogueManager.ActivateDialogue(this);

		// Make sure player can't reinteract with the NPC
		IsInteractable = false;
	}

	public void SetNPCState()
	{
		//if (NPCState == NPCState.Neutral)
		//	NPCState = NPCState.InQuestNoItems;
		//else if (NPCState == NPCState.InQuestWithItems)
		//	NPCState = NPCState.AfterQuest;
		//else if (NPCState == NPCState.InQuestNoItems)
		//	NPCState = NPCState.InQuestWithItems;
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

	private void OnDialogueEnd()
	{
		IsInteractable = true;
		ManageObjectsAfterQuest();
	}
}
