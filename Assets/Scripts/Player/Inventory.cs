using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour 
{
	[SerializeField] private int _inventorySize;

	private List<InventoryItem> _inventoryItems;
	private CanvasManager _canvasManager;

	// Use this for initialization
	private void Awake () 
	{
		_canvasManager = CanvasManager.Instance;
	}

	private void Start()
	{
		_inventoryItems = new List<InventoryItem>();
	}

	public void AddToInventory(InventoryItem item)
	{
		if (_inventoryItems.Count < _inventorySize)
		{
			_inventoryItems.Add(item);
			item.gameObject.SetActive(false);

			UpdateInventoryIcons();
		}
	}

	public void RemoveFromInventory(InventoryItem pickable)
	{
		// Remove item from inventory
		_inventoryItems.Remove(pickable);

		// Update inventory UI
		UpdateInventoryIcons();
	}

	public bool HasRequirements(InventoryItem interactable)
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
		return _inventoryItems.Contains(pickable);
	}

	private void UpdateInventoryIcons()
	{
		for (int i = 0; i < _inventorySize; ++i)
		{
			if (i < _inventoryItems.Count)
				_canvasManager.SetInventorySlotIcon(i, _inventoryItems[i].InventoryIcon);
			else
				_canvasManager.ClearInventorySlotIcon(i);
		}
	}
}
