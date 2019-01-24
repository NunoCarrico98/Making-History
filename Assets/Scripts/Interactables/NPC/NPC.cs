using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class NPC : MonoBehaviour, IInteractable
{
	/// <summary>
	/// Variable that defines NPC ID.
	/// </summary>
	[SerializeField] private int _id;
	/// <summary>
	/// Variable that defines NPC name.
	/// </summary>
	[SerializeField] private string _name;
	/// <summary>
	/// Variable that defines the interaction text of the NPC.
	/// </summary>
	[SerializeField] private string _interactionText;
	/// <summary>
	/// Variable that defines if NPC is a scene changer.
	/// </summary>
	[SerializeField] private bool _isSceneChanger;
	/// <summary>
	/// Variable that defines inventory requirements of the NPC.
	/// </summary>
	[SerializeField] private List<InventoryItem> _inventoryRequirements;
	/// <summary>
	/// Variable that defines all NPC dialogue.
	/// </summary>
	[Header("Non-Quest Dialogue")]
	[SerializeField] private Dialogue _dialogue;

	/// <summary>
	/// Reference to the player gameobject
	/// </summary>
	protected Player player;
	/// <summary>
	/// Reference to the dialogue Manager gamebject.
	/// </summary>
	protected DialogueManager _dialogueManager;

	/// <summary>
	/// Property that returns the NPC ID.
	/// </summary>
	public int ID => _id;
	/// <summary>
	/// Property that return the NPC name.
	/// </summary>
	public string Name => _name;
	/// <summary>
	/// Property that returns the NPC interaction text.
	/// </summary>
	public string InteractionText => _interactionText;
	/// <summary>
	/// Property that returns if the NPC is a scene changer.
	/// </summary>
	public bool IsSceneChanger => _isSceneChanger;
	/// <summary>
	/// Property that returns the NPC dialogue.
	/// </summary>
	public Dialogue Dialogue => _dialogue;
	/// <summary>
	/// Property that returns the inventory requirements of the item.
	/// </summary>
	public List<InventoryItem> InventoryRequirements => _inventoryRequirements;
	/// <summary>
	/// Property that defines if the NPC is interactable.
	/// </summary>
	public bool IsInteractable { get; private set; } = true;
	/// <summary>
	/// Property that defines if the item is active. Can be activated after a 
	/// quest if not active.
	/// </summary>
	public bool IsActive { get; private set; } = true;
	/// <summary>
	/// Property that defines the requirement Text of the NPC.
	/// </summary>
	public string RequirementText => "";

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
	/// Unity Enable Method.
	/// </summary>
	private void OnEnable()
	{
		// Add listeners to certain events.
		_dialogueManager.DialogueBegin += OnDialogueBegin;
		_dialogueManager.DialogueEnded += OnDialogueEnd;
	}

	/// <summary>
	/// Unity OnDisable Method.
	/// </summary>
	private void OnDisable()
	{
		// Remove listeners to certain events.
		_dialogueManager.DialogueBegin -= OnDialogueBegin;
		_dialogueManager.DialogueEnded -= OnDialogueEnd;
	}

	/// <summary>
	/// Method that defines the interaction with the NPC.
	/// </summary>
	public void Interact() => _dialogueManager.ActivateDialogue(this);

	/// <summary>
	/// Method that activates the NPC.
	/// </summary>
	private void OnDialogueEnd() => IsActive = true;

	/// <summary>
	/// Method that deactivates the NPC.
	/// </summary>
	private void OnDialogueBegin() => IsActive = false;

	/// <summary>
	/// Method that returns the required dialogue to write on screen.
	/// </summary>
	/// <param name="i">Index of dialogue to return.</param>
	/// <returns>Returns the button text to write on screen.</returns>
	public virtual IEnumerable<string> GetDialogue(int i) => Dialogue.GetDialogue(i);

	/// <summary>
	/// Method that returns the required button text to write on screen.
	/// </summary>
	/// <param name="i">Index of button text to return.</param>
	/// <returns>Returns the button text to write on screen.</returns>
	public virtual string GetButtonText(int i) => Dialogue.GetButtonText(i);
}
