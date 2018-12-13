using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour, IInteractable
{
	[SerializeField] private bool _isActive = true;
	[SerializeField] private int _ID;
	[SerializeField] private string _itemName;
	[SerializeField] private string _interactionText;
	[SerializeField] private string _requirementText;
	[SerializeField] private Sprite _inventoryIcon;
	[SerializeField] private List<InventoryItem> _inventoryRequirements;

	public int ID => _ID;
	public string InteractionText => _interactionText;
	public string RequirementText => _requirementText;
	public Sprite InventoryIcon => _inventoryIcon;
	public List<InventoryItem> InventoryRequirements => _inventoryRequirements;

    public bool IsInteractable { get; private set; } = true;

	public bool IsActive
	{
		get { return _isActive; }
		set { _isActive = value; }
	}

	public void Interact()
	{
		PlayInteractAnimation();
	}

	public void PlayInteractAnimation()
	{
		Animator animator = GetComponent<Animator>();
		if (animator != null)
		{
			GetComponent<Animator>().SetTrigger("Interact");
		}
	}
}
