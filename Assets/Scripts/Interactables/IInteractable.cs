public interface IInteractable
{
    bool IsActive { get; }
    bool IsInteractable { get; }
    string InteractionText { get; }
    string RequirementText { get; }
    InventoryItem[] InventoryRequirements { get; }

    void Interact();
    void PlayInteractAnimation();
}
