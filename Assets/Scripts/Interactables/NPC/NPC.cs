using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
	[SerializeField] private bool _isInteractable = true;
	[SerializeField] private string _NPCName;
	[SerializeField] private string _interactionText;

	[Header("Non-Quest Dialogue")]
	[SerializeField] private Dialogue _dialogue;

	protected DialogueManager _dialogueManager;

	public string NPCName => _NPCName;
	public string InteractionText => _interactionText;
	public Dialogue Dialogue => _dialogue;

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

	private void OnDialogueEnd()
	{
		IsInteractable = true;
	}

	public virtual List<string> GetDialogue(int i)
	{
		return Dialogue.GetDialogue(i);
	}

	public virtual string GetButtonText(int i)
	{
		return Dialogue.GetButtonText(i);
	}
}
