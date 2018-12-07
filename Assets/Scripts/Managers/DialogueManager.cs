using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
	[Header("Button UI")]
    [SerializeField] private Button				_continueButton;
    [SerializeField] private KeyCode			_continueKey;

	[Header("Dialogue UI")]
    [SerializeField] private TextMeshProUGUI	_nameText;
    [SerializeField] private TextMeshProUGUI	_dialogueText;
    [SerializeField] private Animator			_animator;

	private Queue<string>	_sentences;
	private Dialogue		_tempDialogue;
	private CanvasManager	_canvasManager;
	private int				_dialogueChosen;
	private bool			_choosing;
	private bool			_speaking;

	public event Action DialogueEnded;
    public event Action DialogueBegin;
	public event Action<NPC> OptionChosen;

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
		_canvasManager = CanvasManager.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(_continueKey) && _speaking)
        {
            _continueButton.onClick.Invoke();
        }
    }

	private void OnEnable()
	{
		OptionChosen += StartDialogue;
	}

	public void ActivateDialogue(NPC npc)
    {
		_choosing = true;
		_speaking = false;
		OnDialogueBegin();
		_canvasManager.ShowMultipleDialogueChoiceUI(npc);
    }

	public void StartDialogue(NPC npc)
	{
		// Activate the Dialogue Box
		_animator.SetBool("isActive", true);

		// Set NPC name on UI
		_nameText.text = npc.NPCName;

		// Clear the queue
		_sentences.Clear();

		foreach (string sentence in SetDialogue(npc))
		{
			// Queue sentences to write
			_sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

    public List<string> SetDialogue(NPC npc)
    {
        List<string> sentencesToType = new List<string>();

		switch (_dialogueChosen)
		{
			case 1:
				sentencesToType = npc.Dialogue.option1;
				_speaking = false;
				_choosing = true;
				OnOptionChosen(npc);
				break;
			case 2:
				sentencesToType = npc.Dialogue.option2;
				_speaking = false;
				_choosing = true;
				OnOptionChosen(npc);
				break;
			case 3:
				sentencesToType = npc.Dialogue.option3;
				_speaking = false;
				_choosing = true;
				OnOptionChosen(npc);
				break;
			case 4:
				OnOptionChosen(npc);
				EndDialogue();
				break;
		}

		_speaking = true;
		_choosing = false;

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
		foreach (char letter in sentence)
		{
			_dialogueText.text += letter;
			yield return null;
		}
	}

	public void EndDialogue()
    {
        // Deactivate the Dialogue Box
        _animator.SetBool("isActive", false);
		_speaking = false;
		_choosing = false;
		OnDialogueEnded();
    }

	public void ChooseOption1()
	{
		_dialogueChosen = 1;
	}

	public void ChooseOption2()
	{
		_dialogueChosen = 2;
	}

	public void ChooseOption3()
	{
		_dialogueChosen = 3;
	}

	public void ChooseOption4()
	{
		_dialogueChosen = 4;
	}

	protected virtual void OnDialogueEnded()
	{
		DialogueEnded?.Invoke();
	}

	protected virtual void OnDialogueBegin()
	{
		DialogueBegin?.Invoke();
	}

	protected virtual void OnOptionChosen(NPC npc)
	{
		OptionChosen?.Invoke(npc);
	}
}
