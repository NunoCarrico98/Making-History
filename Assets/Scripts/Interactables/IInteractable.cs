using System.Collections.Generic;

/// <summary>
/// Interface that defines a gameobject interactable.
/// </summary>
public interface IInteractable
{
	/// <summary>
	/// Variable that defines a interactable is active.
	/// </summary>
    bool IsActive { get; }
	/// <summary>
	/// Variable that defines a interactable is interactable.
	/// </summary>
	bool IsInteractable { get; }
	/// <summary>
	/// Variable that defines the interaction text.
	/// </summary>
	string InteractionText { get; }
	/// <summary>
	/// Variable that defines the requirement text.
	/// </summary>
	string RequirementText { get; }
	/// <summary>
	/// Variable that defines the inventory requirements.
	/// </summary>
	List<InventoryItem> InventoryRequirements { get; }

	/// <summary>
	/// Method that defines the interaction with the interactable.
	/// </summary>
	void Interact();
	/// <summary>
	/// Method that plays the interactable animation if it has one.
	/// </summary>
	void PlayInteractAnimation();
}
