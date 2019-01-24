using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Method that deals with all inventory operations.
/// </summary>
public class Inventory : MonoBehaviour
{
	/// <summary>
	/// Defines inventory size.
	/// </summary>
    [SerializeField] private int _inventorySize;

	/// <summary>
	/// Reference to the Canvas Manager.
	/// </summary>
    private CanvasManager _canvasManager;

	/// <summary>
	/// List with all inventory items.
	/// </summary>
    public List<InventoryItem> InventoryItems { get; private set; }

    /// <summary>
	/// Unity Awake Method.
	/// </summary>
    private void Awake() =>  _canvasManager = FindObjectOfType<CanvasManager>();

	/// <summary>
	/// Unity Start Method.
	/// </summary>
    private void Start() => // Initialise inventory
        InventoryItems = new List<InventoryItem>();

	/// <summary>
	/// Methd that adds a new item to the inventory.
	/// </summary>
	/// <param name="item">Item to be added to inventory.</param>
    public void AddToInventory(InventoryItem item)
    {
		// If inventory is not full
        if (InventoryItems.Count < _inventorySize)
        {
			// Add the item to the inventory
            InventoryItems.Add(item);
			// Disabe item
            item.gameObject.SetActive(false);

			// Update inventory UI
            UpdateInventoryIcons();
        }
    }

	/// <summary>
	/// Method that removes an item from the inventory.
	/// </summary>
	/// <param name="interactable">Item to be removed from inventory</param>
    public void RemoveFromInventory(IInteractable interactable)
    {
        // Remove item from inventory
        InventoryItems.Remove(interactable as InventoryItem);

        // Update inventory UI
        UpdateInventoryIcons();
    }

	/// <summary>
	/// Method that checks if player has the requirements to interact with the 
	/// current interactable.
	/// </summary>
	/// <param name="interactable">Interactable where we that has the 
	/// requirements.</param>
	/// <returns>Return true if player has the requirements.</returns>
    public bool HasRequirements(IInteractable interactable)
    {
        if (interactable.InventoryRequirements != null)
            foreach (InventoryItem i in interactable.InventoryRequirements)
                if (!HasInInventory(i))
                    return false;
        return true;
    }

	/// <summary>
	/// Method that verifies if an item is contained in the inventory.
	/// </summary>
	/// <param name="item">Item to be checked if is on inventory.</param>
	/// <returns>Return true if item is in inventory.</returns>
    public bool HasInInventory(InventoryItem item) => 
		// Verify if given item is in inventory
        InventoryItems.Contains(item);

	/// <summary>
	/// Method that updates the inventory UI.
	/// </summary>
    private void UpdateInventoryIcons()
    {
		// For the entire inventory
        for (int i = 0; i < _inventorySize; ++i)
        {
			// If there is still items not checked on inventory
            if (i < InventoryItems.Count)
				// Update UI accordingly
                _canvasManager.SetInventorySlotIcon(i, InventoryItems[i].InventoryIcon);
            else
				// Update all last spots with no items with an empty UI
                _canvasManager.ClearInventorySlotIcon(i);
        }
    }
}
