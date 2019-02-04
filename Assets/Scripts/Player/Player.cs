using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

/// <summary>
/// Class that deals with all player operations.
/// </summary>
public class Player : MonoBehaviour
{
	/// <summary>
	/// Max interaction distance (Raycast range).
	/// </summary>
	[SerializeField] private float _maxInteractionDistance;

	/// <summary>
	/// Reference to the canvas manager.
	/// </summary>
	private CanvasManager _canvasManager;
	/// <summary>
	/// Reference to the dialogue manager.
	/// </summary>
	private DialogueManager _dialogueManager;
	/// <summary>
	/// Reference to the main camera.
	/// </summary>
	private Camera _cam;
	/// <summary>
	/// Reference to the raycast that detects interactables.
	/// </summary>
	private RaycastHit _raycastHit;
	/// <summary>
	/// Reference to the First Person Controller.
	/// </summary>
	private FirstPersonController _fpc;
	/// <summary>
	/// Reference to the Audio Source for interaction sound.
	/// </summary>
	private AudioSource _audioSource;
	/// <summary>
	/// Reference to the script that saves the data to then send to the database.
	/// </summary>
	private DatabaseData _dbData;
	/// <summary>
	/// Caches the mouse sensitivities.
	/// </summary>
	private float[] _saveMouseSensitivities;

	/// <summary>
	/// The current interactable detected.
	/// </summary>
	public IInteractable CurrentInteractable { get; private set; }
	/// <summary>
	/// Defines player inventory.
	/// </summary>
	public Inventory Inventory { get; private set; }

	/// <summary>
	/// Event called when player interacts with an interactable.
	/// </summary>
	public event Action<IInteractable> Interacted;

	/// <summary>
	/// Singleton Player Instance.
	/// </summary>
	public static Player Instance { get; private set; }

	/// <summary>
	/// Unity Awake Method.
	/// </summary>
	private void Awake()
	{
		// Singleton Instance
		if (Instance == null)
			Instance = this;
		else if (Instance != null)
			Destroy(gameObject);

		// Find References
		_canvasManager = FindObjectOfType<CanvasManager>();
		_dialogueManager = FindObjectOfType<DialogueManager>();
		Inventory = GetComponent<Inventory>();
		_cam = GetComponentInChildren<Camera>();
		_fpc = gameObject.GetComponent<FirstPersonController>();
		_audioSource = transform.GetChild(1).GetComponent<AudioSource>();
		// Save mouse sensitivity
		_saveMouseSensitivities = new float[2];
		_saveMouseSensitivities[0] = _fpc.MouseLook.XSensitivity;
		_saveMouseSensitivities[1] = _fpc.MouseLook.YSensitivity;
		_dbData = FindObjectOfType<DatabaseData>();
	}

	/// <summary>
	/// Unity Start Method.
	/// </summary>
	private void Start()
	{
		CurrentInteractable = null;
	}

	/// <summary>
	/// Unity Enable Method.
	/// </summary>
	private void OnEnable()
	{
		// Add listeners to certain events.
		_dialogueManager.DialogueBegin += DisablePlayerControls;
		_dialogueManager.DialogueEnded += EnablePlayerControls;
	}

	/// <summary>
	/// Unity OnDisable Method.
	/// </summary>
	private void OnDisable()
	{
		// Remove listeners to certain events.
		_dialogueManager.DialogueBegin -= DisablePlayerControls;
		_dialogueManager.DialogueEnded -= EnablePlayerControls;
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
	/// Check if player wants to interact with interactable.
	/// </summary>
	private void CheckForInteraction()
	{
		// If player presses E and there is an interactable detected
		if (Input.GetKeyDown(KeyCode.E) && CurrentInteractable != null)
		{
			// Interact
			if (CurrentInteractable is NPC)
				InteractWithNPC();
			else if (CurrentInteractable.IsActive)
				Interact();
		}
	}

	/// <summary>
	/// Check if there is an interactable in the raycast range.
	/// </summary>
	private void CheckForInteractable()
	{
		// Cast a raycast and check if there a collider
		if (Physics.Raycast(_cam.transform.position,
			_cam.transform.forward, out _raycastHit, _maxInteractionDistance))
		{
			// Save interactable
			IInteractable newInteractable =
				_raycastHit.collider.GetComponent<IInteractable>();

			// If there is an interctable in range
			if (newInteractable != null && newInteractable.IsInteractable)
				// Set new interactable
				SetInteractable(newInteractable);
			else
				// Clear old interactable
				ClearInteractable();

		}
		else
			// Clear old interactable
			ClearInteractable();
	}

	/// <summary>
	/// Method that sets the interactable the player is interacting with.
	/// </summary>
	/// <param name="newInteractable">Detected interactable.</param>
	private void SetInteractable(IInteractable newInteractable)
	{
		// Set new interactable
		CurrentInteractable = newInteractable;

		// Stop all coroutines
		StopAllCoroutines();
		// Show inventory
		_canvasManager.ShowInventory();
		// Show interaction text
		_canvasManager.SetInteractionText(CurrentInteractable, Inventory);
	}

	/// <summary>
	/// Method that clears the interactable.
	/// </summary>
	private void ClearInteractable()
	{
		// Hide interaction panel
		_canvasManager.HideInteractionPanel();
		// Start coroutine that hides the inventory
		StartCoroutine(_canvasManager.HideInventory());
		
		CurrentInteractable = null;
	}

	/// <summary>
	/// Method that defines interaction with an inventory item and a static 
	/// interctable.
	/// </summary>
	private void Interact()
	{
		// If interaction is with an inventory item
		if (CurrentInteractable is InventoryItem)
		{
			// Add item to inventory
			Inventory.AddToInventory(CurrentInteractable as InventoryItem);
			// Play pick up sound
			_audioSource.Play();
		}

		//Remove item from inventory
		if (Inventory.HasRequirements(CurrentInteractable))
		{
			// Interact with current detected interactable
			CurrentInteractable.Interact();
			OnInteracted(CurrentInteractable);
			// Remove items from inventory
			RemoveItemFromInventory();
		}
	}

	/// <summary>
	/// Method that removes an item from the inventory.
	/// </summary>
	private void RemoveItemFromInventory()
	{
		if (CurrentInteractable is InventoryItem)
			foreach (InventoryItem i in CurrentInteractable.InventoryRequirements)
				Inventory.RemoveFromInventory(i);
		else if ((CurrentInteractable as StaticInteractable).ConsumeItem)
			foreach (InventoryItem i in CurrentInteractable.InventoryRequirements)
				Inventory.RemoveFromInventory(i);
		else
			(CurrentInteractable as StaticInteractable).ChangeInteractionText();
	}

	/// <summary>
	/// Method that deals with NPC interaction.
	/// </summary>
	private void InteractWithNPC()
	{
		// Interact with NPC
		CurrentInteractable.Interact();
		// Call interaction event
		OnInteracted(CurrentInteractable);
		_dbData.IncrementToTimesSpokenWithNPCs();
	}

	/// <summary>
	/// Method that enables player control.
	/// </summary>
	public void EnablePlayerControls() => _fpc.enabled = true;

	/// <summary>
	/// Method that disables player movement.
	/// </summary>
	public void DisablePlayerControls() => _fpc.enabled = false;

	/// <summary>
	/// Method that enables the mouse lock disabling camera controls.
	/// </summary>
	public void EnableMouseLock()
	{
		// Unlock cursor from middle of screen and make it visible
		_fpc.MouseLook.SetCursorLock(false);
		// Disable mouse controls
		_fpc.MouseLook.XSensitivity = 0;
		_fpc.MouseLook.YSensitivity = 0;
	}

	/// <summary>
	/// Method that disables the mouse lock enabling camera controls.
	/// </summary>
	public void DisableMouseLock()
	{
		// Lock cursor in the middle of screen and make it invisible
		_fpc.MouseLook.SetCursorLock(true);
		// Enable mouse controls with saved sensitivities
		_fpc.MouseLook.XSensitivity = _saveMouseSensitivities[0];
		_fpc.MouseLook.YSensitivity = _saveMouseSensitivities[1];
	}

	/// <summary>
	/// Method that invokes the event when player interacts.
	/// </summary>
	/// <param name="obj">Currently detected interactable.</param>
	protected virtual void OnInteracted(IInteractable obj) => Interacted?.Invoke(obj);
}
