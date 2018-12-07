using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Button		_continueButton;
    [SerializeField] private KeyCode	_continueKey;
    [SerializeField] private Text		_nameText;
    [SerializeField] private Text		_dialogueText;
    [SerializeField] private Animator	_animator;
    [SerializeField] private int		_maxNumberOfSentences;

    private Queue<string> _sentences;

    public event Action DialogueEnded;
    public event Action DialogueBegun;

	public static DialogueManager Instance { get; private set; }

	private void Awake()
    {
		if (Instance == null)
			Instance = this;
		else if (Instance != null)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

    // Use this for initialization
    void Start()
    {
        _sentences = new Queue<string>();
    }

    void Update()
    {
        if (Input.GetKeyDown(_continueKey))
        {
            _continueButton.onClick.Invoke();
        }
    }

    public void StartDialogue(Dialogue dialogue, NPCState state)
    {
        // Activate the Dialogue Box
        _animator.SetBool("isActive", true);

        // Set NPC name on UI
        _nameText.text = dialogue.name;

        // Clear the queue
        _sentences.Clear();

        foreach (string sentence in SetDialogue(dialogue, state))
        {
            // Queue sentences to write
            _sentences.Enqueue(sentence);
        }

        // Write Next Sentence
        DisplayNextSentence();
    }

    public string[] SetDialogue(Dialogue dialogue, NPCState state)
    {
        string[] sentencesToType = new string[_maxNumberOfSentences];

        // Set NPC dialogue according to his state
        switch (state)
        {
            case NPCState.Neutral:
                sentencesToType = dialogue.neutralsentences;
                break;
            case NPCState.InQuestNoItems:
                sentencesToType = dialogue.neutralsentences2;
                break;
            case NPCState.InQuestWithItems:
                sentencesToType = dialogue.neutralsentences3;
                break;
            case NPCState.AfterQuest:
                sentencesToType = dialogue.neutralsentences4;
                break;
        }
        // Return dialogue
        return sentencesToType;
    }

    public void DisplayNextSentence()
    {
        // If there no more sentences to write
        if (_sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Dequeue sentence that's going to be written
        string sentence = _sentences.Dequeue();

        // Stop typing if player presses continue while sentence is not complete
        StopAllCoroutines();

        // Type sentence
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        // Initialise UI text with no letters
        _dialogueText.text = "";

        // Go through the string and write each letter with delay
        foreach (char letter in sentence.ToCharArray())
        {
            _dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        // Deactivate the Dialogue Box
        _animator.SetBool("isActive", false);

		OnDialogueEnded();
    }

	protected virtual void OnDialogueEnded()
	{
		DialogueEnded?.Invoke();
	}

	protected virtual void OnDialogueBegun()
	{
		DialogueBegun?.Invoke();
	}
}
