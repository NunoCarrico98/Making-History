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
	private IInteractable _currentInteractable;
	private Inventory _inventory;

	public IInteractable CurrentInteractable => _currentInteractable;

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
		_currentInteractable = null;
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
		if (Input.GetKeyDown(KeyCode.E) && _currentInteractable != null)
		{
			if (_currentInteractable is InventoryItem)
				InteractWithItem();
			else if (_currentInteractable is NPC)
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
		_currentInteractable = newInteractable;

		if (_currentInteractable is InventoryItem)
		{
			InventoryItem currentItem = _currentInteractable as InventoryItem;
			if (_inventory.HasRequirements(currentItem))
			{
				_canvasManager.ShowInteractionPanel(_currentInteractable.InteractionText);
			}
			else if (currentItem)
			{
				_canvasManager.ShowInteractionPanel(currentItem.RequirementText);
			}
		}
		else if (_currentInteractable is NPC)
			_canvasManager.ShowInteractionPanel(_currentInteractable.InteractionText);
	}

	private void ClearInteractable()
	{
		_currentInteractable = null;

		_canvasManager.HideInteractionPanel();
	}


	private void InteractWithItem()
	{
		if (_inventory.HasRequirements(_currentInteractable as InventoryItem))
			foreach (InventoryItem i in (_currentInteractable as InventoryItem).InventoryRequirements)
				_inventory.RemoveFromInventory(i);

		_inventory.AddToInventory(_currentInteractable as InventoryItem);

		OnInteracted(_currentInteractable);
		// Interact with current detected interactable
		_currentInteractable.Interact();
	}

	private void TalkWithNPC()
	{
		_currentInteractable.Interact();
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
