using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	[SerializeField] private int _inventorySize;

	private CanvasManager _canvasManager;

	public List<InventoryItem> InventoryItems { get; private set; }

	// Use this for initialization
	private void Awake()
	{
		_canvasManager = CanvasManager.Instance;
	}

	private void Start()
	{
		InventoryItems = new List<InventoryItem>();
	}

	public void AddToInventory(InventoryItem item)
	{
		if (InventoryItems.Count < _inventorySize)
		{
			InventoryItems.Add(item);
			item.gameObject.SetActive(false);

			UpdateInventoryIcons();
		}
	}

	public void RemoveFromInventory(IInteractable interactable)
	{
		// Remove item from inventory
		InventoryItems.Remove(interactable as InventoryItem);

		// Update inventory UI
		UpdateInventoryIcons();
	}

	public bool HasRequirements(IInteractable interactable)
	{
		foreach (InventoryItem i in interactable.InventoryRequirements)
		{
			if (!HasInInventory(i))
			{
				return false;
			}
		}
		return true;
	}

	public bool HasInInventory(InventoryItem pickable)
	{
		// Verify if given item is in inventory
		return InventoryItems.Contains(pickable);
	}

	private void UpdateInventoryIcons()
	{
		for (int i = 0; i < _inventorySize; ++i)
		{
			if (i < InventoryItems.Count)
				_canvasManager.SetInventorySlotIcon(i, InventoryItems[i].InventoryIcon);
			else
				_canvasManager.ClearInventorySlotIcon(i);
		}
	}
}
