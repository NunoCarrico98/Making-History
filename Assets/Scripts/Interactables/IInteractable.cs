using System.Collections.Generic;

/// <summary>
/// Interface that defines what is an interactable object.
/// </summary>
public interface IInteractable
{
	/// <summary>
	/// Defines if the object is currently active.
	/// </summary>
    bool IsActive { get; }
	/// <summary>
	/// Defines if the obeject is currently interactable.
	/// </summary>
    bool IsInteractable { get; }
	/// <summary>
	/// Defines the text shown to player if player meets the requirements.
	/// </summary>
    string InteractionText { get; }
	/// <summary>
	/// Defines the text shown to plyer if player does not meet requirements.
	/// </summary>
    string RequirementText { get; }
	/// <summary>
	/// Defines the inventory requirements to be able to interact with the object.
	/// </summary>
    List<InventoryItem> InventoryRequirements { get; }

	/// <summary>
	/// Defines the interaction with the object.
	/// </summary>
    void Interact();
}
