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

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("isActive", true);
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("isActive", false);
        OnDialogueEndCallback.Invoke();
    }
}
