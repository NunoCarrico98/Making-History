using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private KeyCode key;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text dialogueText;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private int maxNumberOfSentences;

    private Queue<string> sentences;

    public static DialogueManager Instance { get; private set; }

    public delegate void OnDialogueEnd();
    public OnDialogueEnd OnDialogueEndCallback;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Use this for initialization
    void Start()
    {
        sentences = new Queue<string>();
    }

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            continueButton.onClick.Invoke();
        }
    }

    public void StartDialogue(Dialogue dialogue, NPCState state)
    {
        // Activate the Dialogue Box
        animator.SetBool("isActive", true);

        // Set NPC name on UI
        nameText.text = dialogue.name;

        // Clear the queue
        sentences.Clear();

        foreach (string sentence in SetDialogue(dialogue, state))
        {
            // Queue sentences to write
            sentences.Enqueue(sentence);
        }

        // Write Next Sentence
        DisplayNextSentence();
    }

    public string[] SetDialogue(Dialogue dialogue, NPCState state)
    {
        string[] sentencesToType = new string[maxNumberOfSentences];

        // Set NPC dialogue according to his state
        switch (state)
        {
            case NPCState.Neutral:
                sentencesToType = dialogue.neutralsentences;
                break;
            case NPCState.InQuestNoItems:
                sentencesToType = dialogue.inQuestNoItemSentences;
                break;
            case NPCState.InQuestWithItems:
                sentencesToType = dialogue.inQuestWithItemSentences;
                break;
            case NPCState.AfterQuest:
                sentencesToType = dialogue.afterQuestSentences;
                break;
        }
        // Return dialogue
        return sentencesToType;
    }

    public void DisplayNextSentence()
    {
        // If there no more sentences to write
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Dequeue sentence that's going to be written
        string sentence = sentences.Dequeue();

        // Stop typing if player presses continue while sentence is not complete
        StopAllCoroutines();

        // Type sentence
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        // Initialise UI text with no letters
        dialogueText.text = "";

        // Go through the string and write each letter with delay
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        // Deactivate the Dialogue Box
        animator.SetBool("isActive", false);

        OnDialogueEndCallback.Invoke();
    }
}
