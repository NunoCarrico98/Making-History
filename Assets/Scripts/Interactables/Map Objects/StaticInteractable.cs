using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that defines a static interactable: item that doesn't move and can't 
/// be picked up.
/// </summary>
public class StaticInteractable : MonoBehaviour, IInteractable
{
	/// <summary>
	/// Variable that defines if the gameobject is active.
	/// </summary>
	[SerializeField] private bool _isActive = false;
	/// <summary>
	/// Variable that defines if the item is consumed after an interaction 
	/// with the object.
	/// </summary>
	[SerializeField] private bool _consumeItem;
	/// <summary>
	/// Variable that defines the interaction text of the item.
	/// </summary>
	[SerializeField] private string _interactionText;
	/// <summary>
	/// Variable that defines the requirement Text of the item.
	/// </summary>
	[SerializeField] private string _requirementText;
	/// <summary>
	/// Variable that defines the text that appears after completing a quest.
	/// </summary>
	[SerializeField] private string _textAfterQuest;
	/// <summary>
	/// Variable that defines inventory requirements of the item.
	/// </summary>
	[SerializeField] private List<InventoryItem> _inventoryRequirements;

	/// <summary>
	/// Property that returns the interaction text.
	/// </summary>
	public string InteractionText => _interactionText;
	/// <summary>
	/// Property that returns the requirement text.
	/// </summary>
	public string RequirementText => _requirementText;
	/// <summary>
	/// Property that returns the text after completing a quest.
	/// </summary>
	public string TextAfterQuest => _textAfterQuest;
	/// <summary>
	/// Property that returns the inventory requirements of the item.
	/// </summary>
	public List<InventoryItem> InventoryRequirements => _inventoryRequirements;
	/// <summary>
	/// Property that defines if the quest was completed for the gameobject.
	/// </summary>
	public bool AfterQuest { get; set; }
	/// <summary>
	/// Property that defines if the item is interactable.
	/// </summary>
	public bool IsInteractable { get; private set; } = true;
	/// <summary>
	/// Property that defines the the corresponding variable to be able to 
	/// consume item or not. 
	/// </summary>
	public bool ConsumeItem
	{
		get { return _consumeItem; }
		set { _consumeItem = value; }
	}
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
		// Change text to text after quest
		ChangeRequirementText();
		// Play animation if there is one
		PlayInteractAnimation();
	}

	public void PlayInteractAnimation()
	{
		Animator animator = GetComponent<Animator>();
		if (animator != null)
			animator.SetTrigger("Interact");
	}

	/// <summary>
	/// Method to change the requirement text to the text after 
	/// completing the quest.
	/// </summary>
	public void ChangeRequirementText()
	{
		_requirementText = _textAfterQuest;
	}

	/// <summary>
	/// Method to change the interaction text to the text after 
	/// completing the quest.
	/// </summary>
	public void ChangeInteractionText()
	{
		_interactionText = _textAfterQuest;
	}
}
