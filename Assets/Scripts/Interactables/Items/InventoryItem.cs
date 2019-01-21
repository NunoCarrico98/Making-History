using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that defines an inventory item.
/// </summary>
public class InventoryItem : MonoBehaviour, IInteractable
{
	/// <summary>
	/// Variable that defines if the gameobject is active.
	/// </summary>
	[SerializeField] private bool _isActive = true;
	/// <summary>
	/// Variable that defines Item ID.
	/// </summary>
	[SerializeField] private int _ID;
	/// <summary>
	/// Variable that defines Item name.
	/// </summary>
	[SerializeField] private string _itemName;
	/// <summary>
	/// Variable that defines the interaction text of the item.
	/// </summary>
	[SerializeField] private string _interactionText;
	/// <summary>
	/// Variable that defines the requirement Text of the item.
	/// </summary>
	[SerializeField] private string _requirementText;
	/// <summary>
	/// Variable that defines the inventory icon of the item.
	/// </summary>
	[SerializeField] private Sprite _inventoryIcon;
	/// <summary>
	/// Variable that defines inventory requirements of the item.
	/// </summary>
	[SerializeField] private List<InventoryItem> _inventoryRequirements;

	/// <summary>
	/// Property that returns the item ID.
	/// </summary>
	public int ID => _ID;
	/// <summary>
	/// Property that returns the interaction text.
	/// </summary>
	public string InteractionText => _interactionText;
	/// <summary>
	/// Property that returns the requirement text.
	/// </summary>
	public string RequirementText => _requirementText;
	/// <summary>
	/// Property that returns the inventory icon of the item.
	/// </summary>
	public Sprite InventoryIcon => _inventoryIcon;
	/// <summary>
	/// Property that returns the inventory requirements of the item.
	/// </summary>
	public List<InventoryItem> InventoryRequirements => _inventoryRequirements;

	/// <summary>
	/// Property that defines if the item is interactable.
	/// </summary>
	public bool IsInteractable { get; private set; } = true;

	/// <summary>
	/// Property that defines if the item is active. Can be activated after a 
	/// quest if not active.
	/// </summary>
	public bool IsActive
	{
		get { return _isActive; }
		set { _isActive = value; }
	}

	/// <summary>
	/// Method that defines the interaction with the static interactable.
	/// </summary>
	public void Interact()
	{
	}
}
