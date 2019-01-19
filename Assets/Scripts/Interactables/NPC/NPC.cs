using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
	[SerializeField] private int _id;
	[SerializeField] private string _name;
	[SerializeField] private string _interactionText;
	[SerializeField] private bool _isSceneChanger;
	[SerializeField] private List<InventoryItem> _inventoryRequirements;

	[Header("Non-Quest Dialogue")]
	[SerializeField] private Dialogue _dialogue;

	protected DialogueManager _dialogueManager;

	public int ID => _id;
	public string Name => _name;
	public string InteractionText => _interactionText;
	public bool IsSceneChanger => _isSceneChanger;
	public Dialogue Dialogue => _dialogue;
	public List<InventoryItem> InventoryRequirements => _inventoryRequirements;

    public bool IsInteractable { get; private set; } = true;
    public bool IsActive { get; private set; } = true;
    public string RequirementText => "";

	private void Awake()
	{
		_dialogueManager = FindObjectOfType<DialogueManager>();
	}

	private void OnEnable()
	{
		_dialogueManager.DialogueBegin += DisableNPCInteraction;
		_dialogueManager.DialogueEnded += EnableNPCInteraction;
	}

	private void OnDisable()
	{
		_dialogueManager.DialogueBegin -= DisableNPCInteraction;
		_dialogueManager.DialogueEnded -= EnableNPCInteraction;
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
	}

	private void EnableNPCInteraction()
	{
		IsActive = true;
	}

	private void DisableNPCInteraction()
	{
		IsActive = false;
	}

	public virtual List<string> GetDialogue(int i) => Dialogue.GetDialogue(i);
	public virtual string GetButtonText(int i) => Dialogue.GetButtonText(i);
}
