using System.Collections.Generic;

public interface IInteractable
{
    bool IsActive { get; }
    bool IsInteractable { get; }
    string InteractionText { get; }
    string RequirementText { get; }
    List<InventoryItem> InventoryRequirements { get; }

    void Interact();
    void PlayInteractAnimation();
}
