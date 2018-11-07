using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float maxInteractionDistance;
    [SerializeField]
    private int InventorySize;

    private CanvasManager canvasManager;
    private Camera cam;
    private RaycastHit raycastHit;
    private Interactable currentInteractable;
    private List<Interactable> inventory;

    private void Start()
    {
        canvasManager = CanvasManager.Instance;
        cam = GetComponentInChildren<Camera>();
        currentInteractable = null;
        inventory = new List<Interactable>(InventorySize);
    }

    // Update is called once per frame
    private void Update()
    {
        CheckForInteractable();
        CheckForInteractionClick();
    }

    private void CheckForInteractionClick()
    {
        if (Input.GetMouseButtonDown(0) && currentInteractable != null)
        {
            if (currentInteractable.GetIsPickable())
            {
                AddToInventory(currentInteractable);
            }
            else if (HasRequirements(currentInteractable))
            {
                Interact(currentInteractable);
            }
        }
    }

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

    private void SetInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;

        if (HasRequirements(currentInteractable))
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

    private bool HasRequirements(Interactable interactable)
    {
        foreach(Interactable i in interactable.GetInventoryRequirements())
        {
            if (!HasInInventory(i))
            {
                return false;
            }
        }
        return true;
    }

    private void Interact(Interactable interactable)
    {

        foreach (Interactable i in interactable.GetInventoryRequirements())
        {
            RemoveFromInventory(i);
        }

        interactable.Interact();
    }

    private void AddToInventory(Interactable pickable)
    {
        if (inventory.Count < InventorySize)
        {
            inventory.Add(pickable);
            pickable.gameObject.SetActive(false);
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
}
