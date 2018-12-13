using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isActive = false;
    [SerializeField] private string _interactionText;
    [SerializeField] private string _requirementText;
    [SerializeField] private InventoryItem[] _inventoryRequirements;

    public bool IsInteractable { get; private set; } = true;
    public string RequirementText => _requirementText;
    public string InteractionText => _interactionText;
    public InventoryItem[] InventoryRequirements => _inventoryRequirements;
    public bool IsActive
    {
        get { return _isActive; }
        set { _isActive = value; }
    }

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
