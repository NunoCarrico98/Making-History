using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
	[SerializeField] private float _maxInteractionDistance;

	private CanvasManager _canvasManager;
	private DialogueManager _dialogueManager;
	private Camera _cam;
	private RaycastHit _raycastHit;
	private List<Quest> _activeQuests;

	public IInteractable CurrentInteractable { get; private set; }
	public Inventory InventoryItems { get; private set; }

	public event Action<IInteractable> Interacted;

	public static Player Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != null)
			Destroy(gameObject);

		_canvasManager = CanvasManager.Instance;
		_dialogueManager = DialogueManager.Instance;
		InventoryItems = GetComponent<Inventory>();
		_cam = GetComponentInChildren<Camera>();
	}

	private void Start()
	{
		CurrentInteractable = null;
	}

	private void OnEnable()
	{
		_dialogueManager.DialogueBegin += OnDialogueBegin;
		_dialogueManager.DialogueEnded += OnDialogueEnd;
	}

	private void OnDisable()
	{
		_dialogueManager.DialogueBegin -= OnDialogueBegin;
		_dialogueManager.DialogueEnded -= OnDialogueEnd;
	}

	/// <summary>
	/// Method that is called once per frame.
	/// </summary>
	void Update()
	{
		// Check if the player is in front of any interactable
		CheckForInteractable();

		// Check if the player wants to interact the detected item
		CheckForInteraction();
	}

	private void CheckForInteraction()
	{
		if (Input.GetKeyDown(KeyCode.E) && CurrentInteractable != null)
		{
			if (CurrentInteractable is NPC)
				InteractWithNPC();
			else if (CurrentInteractable.IsActive)
				Interact();

		}
	}

	private void CheckForInteractable()
	{
		if (Physics.Raycast(_cam.transform.position,
			_cam.transform.forward, out _raycastHit, _maxInteractionDistance))
		{
			IInteractable newInteractable =
				_raycastHit.collider.GetComponent<IInteractable>();

			if (newInteractable != null && newInteractable.IsInteractable)
			{
				SetInteractable(newInteractable);
			}
			else
			{
				ClearInteractable();
			}

		}
		else
		{
			ClearInteractable();
		}
	}

	/// <summary>
	/// Method that sets the interactable the player is interacting with
	/// </summary>
	/// <param name="newInteractable">Dete </param>
	private void SetInteractable(IInteractable newInteractable)
	{
		CurrentInteractable = newInteractable;

		// If interactable is NPC
		if (CurrentInteractable is NPC)
			_canvasManager.ShowInteractionPanel(CurrentInteractable.InteractionText);
		else if (CurrentInteractable is StaticInteractable)
		{
			if ((CurrentInteractable as StaticInteractable).AfterQuest)
				_canvasManager.ShowInteractionPanel(
					(CurrentInteractable as StaticInteractable).TextAfterQuest);
			else if (InventoryItems.HasRequirements(CurrentInteractable)
					&& CurrentInteractable.IsActive)
				_canvasManager.ShowInteractionPanel(CurrentInteractable.InteractionText);
			else
				_canvasManager.ShowInteractionPanel(CurrentInteractable.RequirementText);
		}
		// If interactable is a map object
		else
		{
			if (InventoryItems.HasRequirements(CurrentInteractable)
				&& CurrentInteractable.IsActive)
				_canvasManager.ShowInteractionPanel(CurrentInteractable.InteractionText);
			else if (!InventoryItems.HasRequirements(CurrentInteractable)
					 || !CurrentInteractable.IsActive)
				_canvasManager.ShowInteractionPanel(CurrentInteractable.RequirementText);
		}
	}

	private void ClearInteractable()
	{
		CurrentInteractable = null;

		_canvasManager.HideInteractionPanel();
	}


	private void Interact()
	{

		if (CurrentInteractable is InventoryItem)
			InventoryItems.AddToInventory(CurrentInteractable as InventoryItem);

		//Remove item from inventory
		if (InventoryItems.HasRequirements(CurrentInteractable))
		{
			// Interact with current detected interactable
			CurrentInteractable.Interact();
			OnInteracted(CurrentInteractable);
			RemoveItemFromInventory();
		}
	}

	private void RemoveItemFromInventory()
	{
		if (CurrentInteractable is InventoryItem)
			foreach (InventoryItem i in CurrentInteractable.InventoryRequirements)
				InventoryItems.RemoveFromInventory(i);
		else if ((CurrentInteractable as StaticInteractable).ConsumeItem)
			foreach (InventoryItem i in CurrentInteractable.InventoryRequirements)
				InventoryItems.RemoveFromInventory(i);
		else
			(CurrentInteractable as StaticInteractable).ChangeInteractionText();
	}

	private void InteractWithNPC()
	{
		CurrentInteractable.Interact();
		OnInteracted(CurrentInteractable);
	}

	private void OnDialogueEnd()
	{
		// Enable First Person Controller script
		GetComponent<FirstPersonController>().enabled = true;
	}

	private void OnDialogueBegin()
	{
		// Enable First Person Controller script
		GetComponent<FirstPersonController>().enabled = false;
	}

	protected virtual void OnInteracted(IInteractable obj)
	{
		Interacted?.Invoke(obj);
	}
}
