using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
	[SerializeField] private float					_maxInteractionDistance;
	[SerializeField] private int					_inventorySize;
	[SerializeField] private List<InventoryItem>	_inventory;

	private CanvasManager	_canvasManager;
	private DialogueManager _dialogueManager;
	private Camera			_cam;
	private RaycastHit		_raycastHit;
	private IInteractable	_currentInteractable;

	private void Awake()
	{
		_canvasManager = CanvasManager.Instance;
		_dialogueManager = DialogueManager.Instance;
		_cam = GetComponentInChildren<Camera>();
	}

	private void Start()
	{
		_currentInteractable = null;
		_inventory = new List<InventoryItem>(_inventorySize);
	}

	private void OnEnable()
	{
		_dialogueManager.DialogueBegun += OnDialogueBegin;
		_dialogueManager.DialogueEnded += OnDialogueEnd;
	}

	private void OnDisable()
	{
		_dialogueManager.DialogueBegun -= OnDialogueBegin;
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

	/// <summary>
	/// Method that checks for player input.
	/// </summary>
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

	/// <summary>
	/// Method that checks if the player is in front of an interactable.
	/// </summary>
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

		if (HasRequirements())
		{
			_canvasManager.ShowInteractionPanel(_currentInteractable.InteractionText);
		}
		else
		{
			_canvasManager.ShowInteractionPanel(_currentInteractable.RequirementText);
		}
	}

	private void ClearInteractable()
	{
		_currentInteractable = null;

		_canvasManager.HideInteractionPanel();
	}

	private bool HasRequirements()
	{
		foreach (InventoryItem i in _currentInteractable.InventoryRequirements)
		{
			if (!HasInInventory(i))
			{
				return false;
			}
		}
		return true;
	}

	private void InteractWithItem()
	{
		if (HasRequirements())
			foreach (InventoryItem i in _currentInteractable.InventoryRequirements)
				RemoveFromInventory(i);

		AddToInventory();

		// Interact with current detected interactable
		_currentInteractable.Interact();
	}

	private void AddToInventory()
	{
		if (_inventory.Count < _inventorySize)
		{
			_inventory.Add(_currentInteractable as InventoryItem);
			(_currentInteractable as InventoryItem).gameObject.SetActive(false);

			UpdateInventoryIcons();
		}
	}

	private bool HasInInventory(InventoryItem pickable)
	{
		// Verify if given item is in inventory
		return _inventory.Contains(pickable);
	}

	private void RemoveFromInventory(InventoryItem pickable)
	{
		// Remove item from inventory
		_inventory.Remove(pickable);

		// Update inventory UI
		UpdateInventoryIcons();
	}

	private void TalkWithNPC()
	{
		if (HasRequirements())
			foreach (InventoryItem i in _currentInteractable.InventoryRequirements)
				RemoveFromInventory(i);

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

	private void UpdateInventoryIcons()
	{
		for (int i = 0; i < _inventorySize; ++i)
		{
			if (i < _inventory.Count)
				_canvasManager.SetInventorySlotIcon(i, _inventory[i].InventoryIcon);
			else
				_canvasManager.ClearInventorySlotIcon(i);
		}
	}
}
