using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Sprite image;

    [SerializeField]
    private string requirementText;
    [SerializeField]
    private string interactionText;
    [SerializeField]
    private bool isInteractable;
    [SerializeField]
    private bool allowsMultipleInteractions;
    [SerializeField]
    private bool isActive;
    [SerializeField]
    private bool isPickable;
    [SerializeField]
    private bool isTalkable;
    [SerializeField]
    private Interactable[] inventoryRequirements;
    [SerializeField]
    private Interactable[] indirectInteractables;
    [SerializeField]
    private Interactable[] indirectActivations;
    [SerializeField]
    private Dialogue dialogue;

    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = DialogueManager.Instance;
    }

    public void Interact()
    {
        PlayInteractAnimation();

        if (isActive)
        {
            InteractIndirects();

            ActivateIndirects();

            if (!allowsMultipleInteractions)
            {
                isInteractable = false;
            }
        }
    }

    private void PlayInteractAnimation()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            GetComponent<Animator>().SetTrigger("Interact");
        }
    }

    private void PlayActivateAnimation()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            GetComponent<Animator>().SetTrigger("Activate");
        }
    }

    private void InteractIndirects()
    {
        if (indirectInteractables != null)
        {
            foreach (Interactable i in indirectInteractables)
            {
                i.Interact();
            }
        }
    }

    private void ActivateIndirects()
    {
        if (indirectInteractables != null)
        {
            foreach (Interactable i in indirectActivations)
            {
                i.Activate();
            }
        }
    }

    public void Activate()
    {
        PlayActivateAnimation();
        isActive = true;
    }

    public void ActivateDialogue()
    {
        isInteractable = false;
        dialogueManager.StartDialogue(dialogue);
    }

    #region Getters for private variables
    /* ******************************
     * Getters for private variables
     * ******************************/


    public string GetInteractionText()
    {
        return interactionText;
    }

    public string GetRequirementText()
    {
        return requirementText;
    }

    public Interactable[] GetInventoryRequirements()
    {
        return inventoryRequirements;
    }

    public bool GetIsPickable()
    {
        return isPickable;
    }

    public bool GetIsInteractable()
    {
        return isInteractable;
    }

    public bool GetIsTalkable()
    {
        return isTalkable;
    }

    #endregion
}
