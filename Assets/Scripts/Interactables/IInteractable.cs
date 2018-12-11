public interface IInteractable
{
	bool			IsInteractable { get; }
	string			InteractionText { get; }

	void Interact();
	void PlayInteractAnimation();
}
