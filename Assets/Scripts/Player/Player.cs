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
	private Inventory _inventory;

	public IInteractable CurrentInteractable { get; private set; }

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
		_inventory = GetComponent<Inventory>();
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
			if (CurrentInteractable is InventoryItem)
				InteractWithItem();
			else if (CurrentInteractable is NPC)
				TalkWithNPC();
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

		if (CurrentInteractable is InventoryItem)
		{
			InventoryItem currentItem = CurrentInteractable as InventoryItem;
			if (_inventory.HasRequirements(currentItem))
			{
				_canvasManager.ShowInteractionPanel(CurrentInteractable.InteractionText);
			}
			else if (currentItem)
			{
				_canvasManager.ShowInteractionPanel(currentItem.RequirementText);
			}
		}
		else if (CurrentInteractable is NPC)
			_canvasManager.ShowInteractionPanel(CurrentInteractable.InteractionText);
	}

	private void ClearInteractable()
	{
		CurrentInteractable = null;

		_canvasManager.HideInteractionPanel();
	}


	private void InteractWithItem()
	{
		if (_inventory.HasRequirements(CurrentInteractable as InventoryItem))
			foreach (InventoryItem i in (CurrentInteractable as InventoryItem).InventoryRequirements)
				_inventory.RemoveFromInventory(i);

		_inventory.AddToInventory(CurrentInteractable as InventoryItem);

		OnInteracted(CurrentInteractable);
		// Interact with current detected interactable
		CurrentInteractable.Interact();
	}

	private void TalkWithNPC()
	{
		CurrentInteractable.Interact();
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