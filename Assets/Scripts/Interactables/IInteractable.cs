public interface IInteractable
{
	bool isInteractable { get; set; }
	string interactionText { get; set; }
	string requirementText { get; set; }
	IInteractable[] inventoryRequirements { get; set; }
}
