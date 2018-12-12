public interface IInteractable
{
	bool			IsInteractable { get; }
	string			InteractionText { get; }
	InventoryItem[] InventoryRequirements { get; }

	void Interact();
	void PlayInteractAnimation();
}
