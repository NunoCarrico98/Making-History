using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

/// <summary>
/// Class that defines player actions.
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// Variable to define the maximum distance the player can interact with 
    /// objects.
    /// </summary>
    [SerializeField]
    private float maxInteractionDistance;
    /// <summary>
    /// Variable to define the inventory size.
    /// </summary>
    [SerializeField]
    private int inventorySize;

    /// <summary>
    /// Variable that contains the canvas manager instance.
    /// </summary>
    private CanvasManager canvasManager;
    private DialogueManager dialogueManager;
    /// <summary>
    /// Variable that contains the player camera.
    /// </summary>
    private Camera cam;
    /// <summary>
    /// Variable to hold information about the object the raycast is detecting.
    /// </summary>
    private RaycastHit raycastHit;
    /// <summary>
    /// Variable to hold the current detected object.
    /// </summary>
    private Interactable currentInteractable;
    /// <summary>
    /// List to hold the objects the player picks up.
    /// </summary>
    private List<Interactable> inventory;

    /// <summary>
    /// Unity method called when the program begins.
    /// </summary>
    void Start()
    {
        // Initialise canvas manager
        canvasManager = CanvasManager.Instance;
        dialogueManager = DialogueManager.Instance;
        // Initialise camera
        cam = GetComponentInChildren<Camera>();
        // Initialise current interactable
        currentInteractable = null;
        // Initialise inventory
        inventory = new List<Interactable>(inventorySize);
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
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            if (currentInteractable.GetIsPickable())
            {
                AddToInventory();
            }
            else if (currentInteractable.GetIsTalkable())
            {
                TalkWithNPC();
            }
            else if (HasRequirements())
            {
                Interact();
            }
        }
    }

    /// <summary>
    /// Method that checks if the player is in front of an interactable.
    /// </summary>
    private void CheckForInteractable()
    {
        if (Physics.Raycast(cam.transform.position,
            cam.transform.forward, out raycastHit, maxInteractionDistance))
        {
            Interactable newInteractable =
                raycastHit.collider.GetComponent<Interactable>();

            if (newInteractable != null && newInteractable.GetIsInteractable())
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
    /// Method that sets
    /// </summary>
    /// <param name="newInteractable">Dete </param>
    private void SetInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;

        if (HasRequirements())
        {
            canvasManager.ShowInteractionPanel(currentInteractable.GetInteractionText());
        }
        else
        {
            canvasManager.ShowInteractionPanel(currentInteractable.GetRequirementText());
        }
    }

    private void ClearInteractable()
    {
        currentInteractable = null;

        canvasManager.HideInteractionPanel();
    }

    private bool HasRequirements()
    {
        foreach (Interactable i in currentInteractable.GetInventoryRequirements())
        {
            if (!HasInInventory(i))
            {
                return false;
            }
        }
        return true;
    }

    private void Interact()
    {
        foreach (Interactable i in currentInteractable.GetInventoryRequirements())
        {
            RemoveFromInventory(i);
        }

        currentInteractable.Interact();
    }

    private void AddToInventory()
    {
        if (inventory.Count < inventorySize)
        {
            inventory.Add(currentInteractable);
            currentInteractable.gameObject.SetActive(false);
        }
        canvasManager.ManageInventoryItemsImages(inventory);
    }

    private bool HasInInventory(Interactable pickable)
    {
        return inventory.Contains(pickable);
    }

    private void RemoveFromInventory(Interactable pickable)
    {
        inventory.Remove(pickable);
        canvasManager.ManageInventoryItemsImages(inventory);
    }

    private void TalkWithNPC()
    {
        currentInteractable.ActivateDialogue();
        DisablePlayerMovement();
        dialogueManager.OnDialogueEndCallback += EnablePlayerMovement;
    }

    private void EnablePlayerMovement()
    {
        GetComponent<FirstPersonController>().enabled = true;
    }

    private void DisablePlayerMovement()
    {
        GetComponent<FirstPersonController>().enabled = false;
    }
}
