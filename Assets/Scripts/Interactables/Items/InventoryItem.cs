using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour, IInteractable
{

	#region Variables
	[SerializeField] private bool				_isInteractable = true;
	[SerializeField] private string				_interactionText;
	[SerializeField] private string				_requirementText;
	[SerializeField] private Sprite				_inventoryIcon;
	[SerializeField] private InventoryItem[]	_inventoryRequirements;

	public string			InteractionText			=> _interactionText;
	public string			RequirementText			=> _requirementText;
	public Sprite			InventoryIcon			=> _inventoryIcon;
	public InventoryItem[]	InventoryRequirements	=> _inventoryRequirements;

	public bool IsInteractable
	{
		get
		{
			return _isInteractable;
		}
		private set
		{
			_isInteractable = value;
		}
	}
	#endregion

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
