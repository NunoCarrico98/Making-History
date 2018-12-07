using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string requirementText;
    [SerializeField] private string interactionText;
    [SerializeField] private bool isInteractable;
    [SerializeField] private bool allowsMultipleInteractions;
    [SerializeField] private bool isActive;
    [SerializeField] private bool isPickable;
    [SerializeField] private bool isTalkable;
    [SerializeField] private bool activateInventoryRequirements = true;
    [SerializeField] private Interactable[] inventoryRequirements;
    [SerializeField] private Interactable[] indirectInteractables;
    [SerializeField] private Interactable[] indirectActivations;
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private GameObject destroyAfterQuest;
    [SerializeField] private GameObject enableAfterQuest;

    private DialogueManager dialogueManager;

    public Sprite inventoryIcon;
    public NPCState npcState;

    private void Start()
    {
        dialogueManager = DialogueManager.Instance;
        npcState = NPCState.Neutral;
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
        isInteractable = true;
    }

    public void ActivateDialogue()
    {
        // Start dialogue
        dialogueManager.StartDialogue(dialogue, npcState);

        // Make sure player can't reinteract with the NPC
        isInteractable = false;

        // Activate quest
        activateInventoryRequirements = true;

        // Set the state of the NPC
        SetNPCState();

        // Reactivate NPC when the dialogue ends
        dialogueManager.DialogueEnded += ReActivateNPC;

        // Enable / Destroy objects after quest
        dialogueManager.DialogueEnded += ManageObjectAfterQuest;
    }

    private void ReActivateNPC()
    {
        isInteractable = true;
    }

    public void SetNPCState()
    {
        if (npcState == NPCState.Neutral)
        {
            npcState = NPCState.InQuestNoItems;
        }
        else if (npcState == NPCState.InQuestWithItems)
        {
            npcState = NPCState.AfterQuest;
        }
    }

    public void DestroyObjectAfterQuest()
    {
        if (destroyAfterQuest != null && npcState == NPCState.AfterQuest)
        {
            Destroy(destroyAfterQuest);
        }
    }

    public void EnableObjectAfterQuest()
    {
        if (enableAfterQuest != null && npcState == NPCState.AfterQuest)
        {
            enableAfterQuest.SetActive(true);
        }
    }

    public void ManageObjectAfterQuest()
    {
        DestroyObjectAfterQuest();
        EnableObjectAfterQuest();
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

    public bool GetInventoryRequirementesAreActive()
    {
        return activateInventoryRequirements;
    }

    #endregion
}
