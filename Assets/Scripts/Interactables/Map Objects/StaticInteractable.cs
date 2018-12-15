using System.Collections.Generic;
using UnityEngine;

public class StaticInteractable : MonoBehaviour, IInteractable
{
	[SerializeField] private bool _isActive = false;
	[SerializeField] private bool _consumeItem;
	[SerializeField] private string _interactionText;
	[SerializeField] private string _requirementText;
	[SerializeField] private string _textAfterQuest;
	[SerializeField] private List<InventoryItem> _inventoryRequirements;

	public bool IsInteractable { get; private set; } = true;
	public bool ConsumeItem
	{
		get { return _consumeItem; }
		set { _consumeItem = value; }
	}
	public string RequirementText => _requirementText;
	public string InteractionText => _interactionText;
	public string TextAfterQuest => _textAfterQuest;
	public List<InventoryItem> InventoryRequirements => _inventoryRequirements;
	public bool AfterQuest { get; set; }
	public bool IsActive
	{
		get { return _isActive; }
		set { _isActive = value; }
	}

	public void Interact()
	{
		ChangeRequirementText();
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

	public void ChangeRequirementText()
	{
		_requirementText = _textAfterQuest;
	}

	public void ChangeInteractionText()
	{
		_interactionText = _textAfterQuest;
	}
}
