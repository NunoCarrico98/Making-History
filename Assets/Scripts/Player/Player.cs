using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
	[SerializeField] private float _maxInteractionDistance;
	[SerializeField] float _maxNpcLookDistance;

	private CanvasManager _canvasManager;
	private DialogueManager _dialogueManager;
	private Camera _cam;
	private RaycastHit _raycastHit;
	private List<Quest> _activeQuests;
	private FirstPersonController _fpc;
	private AudioSource _audioSource;

	private float[] _saveMouseSensitivities;

	public float MaxNpcLookDistance => _maxNpcLookDistance;
	public IInteractable CurrentInteractable { get; private set; }
	public Inventory Inventory { get; private set; }

	public event Action<IInteractable> Interacted;

	public static Player Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != null)
			Destroy(gameObject);

		_canvasManager = FindObjectOfType<CanvasManager>();
		_dialogueManager = FindObjectOfType<DialogueManager>();
		Inventory = GetComponent<Inventory>();
		_cam = GetComponentInChildren<Camera>();
		_fpc = gameObject.GetComponent<FirstPersonController>();
		_saveMouseSensitivities = new float[2];
		_audioSource = transform.GetChild(1).GetComponent<AudioSource>();
	}

	private void Start()
	{
		CurrentInteractable = null;
	}

	private void OnEnable()
	{
		_dialogueManager.DialogueBegin += DisablePlayerControls;
		_dialogueManager.DialogueEnded += EnablePlayerControls;
	}

	private void OnDisable()
	{
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

		StopAllCoroutines();
		_canvasManager.ShowInventory();
		_canvasManager.SetInteractionText(CurrentInteractable, Inventory);
	}

	private void ClearInteractable()
	{
		_canvasManager.HideInteractionPanel();
		StartCoroutine(_canvasManager.HideInventory());

		CurrentInteractable = null;
	}


	private void Interact()
	{

		if (CurrentInteractable is InventoryItem)
		{
			Inventory.AddToInventory(CurrentInteractable as InventoryItem);
			_audioSource.Play();
		}

		//Remove item from inventory
		if (Inventory.HasRequirements(CurrentInteractable))
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
				Inventory.RemoveFromInventory(i);
		else if ((CurrentInteractable as StaticInteractable).ConsumeItem)
			foreach (InventoryItem i in CurrentInteractable.InventoryRequirements)
				Inventory.RemoveFromInventory(i);
		else
			(CurrentInteractable as StaticInteractable).ChangeInteractionText();
	}

	private void InteractWithNPC()
	{
		CurrentInteractable.Interact();
		OnInteracted(CurrentInteractable);
	}

	public void EnablePlayerControls()
	{
		// Enable First Person Controller script
		_fpc.enabled = true;
	}

	public void DisablePlayerControls()
	{
		// Enable First Person Controller script
		_fpc.enabled = false;
	}

	public void EnableMouseLock()
	{
		_fpc.MouseLook.SetCursorLock(false);
		_saveMouseSensitivities[0] = _fpc.MouseLook.XSensitivity;
		_saveMouseSensitivities[1] = _fpc.MouseLook.YSensitivity;
		_fpc.MouseLook.XSensitivity = 0;
		_fpc.MouseLook.YSensitivity = 0;
	}

	public void DisableMouseLock()
	{
		_fpc.MouseLook.SetCursorLock(true);
		_fpc.MouseLook.XSensitivity = _saveMouseSensitivities[0];
		_fpc.MouseLook.YSensitivity = _saveMouseSensitivities[1];
	}

	protected virtual void OnInteracted(IInteractable obj)
	{
		Interacted?.Invoke(obj);
	}
}
