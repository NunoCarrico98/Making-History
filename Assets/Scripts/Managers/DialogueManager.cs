using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
	[Header("Button UI")]
	[SerializeField] private Button _continueButton;

	[Header("Dialogue UI")]
	[SerializeField] private TextMeshProUGUI _nameText;
	[SerializeField] private TextMeshProUGUI _dialogueText;
	[SerializeField] private Animator _animator;

	private Queue<string> _sentences;
	private CanvasManager _canvasManager;
	private NPC _tempNPC;

	public Button ContinueButton => _continueButton;

	public event Action DialogueEnded;
	public event Action DialogueBegin;

	public int DialogueChosen { get; private set; }

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

	public void ActivateDialogue(NPC npc)
	{
		_tempNPC = npc;
		OnDialogueBegin();
		_canvasManager.ShowMultipleDialogueChoiceUI(npc);
		_continueButton.enabled = false;
	}

	public void SetDialogue()
	{
		List<string> sentencesToType = new List<string>();

		_nameText.text = _tempNPC.Name;

		switch (DialogueChosen)
		{
			case 1:
				sentencesToType = _tempNPC.GetDialogue(DialogueChosen);
				StartDialogue(sentencesToType);
				break;
			case 2:
				sentencesToType = _tempNPC.GetDialogue(DialogueChosen);
				StartDialogue(sentencesToType);
				break;
			case 3:
				sentencesToType = _tempNPC.GetDialogue(DialogueChosen);
				StartDialogue(sentencesToType);
				break;
			case 4:
				EndDialogue();
				break;
		}
	}

	public void StartDialogue(IEnumerable<string> sentencesToType)
	{
		// Space Button is now selected in dialogues
		_continueButton.Select();
		// Activate Dialogue Continue Button
		_continueButton.enabled = true;
		_canvasManager.HideMultipleDialogueChoiceUI();

		// Activate the Dialogue Box
		_animator.SetBool("isActive", true);

		// Clear the queue
		_sentences.Clear();

		foreach (string sentence in sentencesToType)
		{
			// Queue sentences to write
			_sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
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
		DialogueChosen = 0;
		_tempNPC = null;
		// Deactivate the Dialogue Box
		_animator.SetBool("isActive", false);
		OnDialogueEnded();
		_canvasManager.HideMultipleDialogueChoiceUI();
	}

	public void ChooseOption1()
	{
		DialogueChosen = 1;
		SetDialogue();
	}

	public void ChooseOption2()
	{
		DialogueChosen = 2;
		SetDialogue();
	}

	public void ChooseOption3()
	{
		DialogueChosen = 3;
		SetDialogue();
	}

	public void ChooseOption4()
	{
		DialogueChosen = 4;
		SetDialogue();
	}

	protected virtual void OnDialogueEnded()
	{
		DialogueEnded?.Invoke();
	}

	protected virtual void OnDialogueBegin()
	{
		DialogueBegin?.Invoke();
	}
}
