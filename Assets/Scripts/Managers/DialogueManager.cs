using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class that manages all dialogue written on screen.
/// </summary>
public class DialogueManager : MonoBehaviour
{
	/// <summary>
	/// Reference to the continue Button.
	/// </summary>
	[Header("Button UI")]
	[SerializeField] private Button _continueButton;

	/// <summary>
	/// Reference to the name TMP.
	/// </summary>
	[Header("Dialogue UI")]
	[SerializeField] private TextMeshProUGUI _nameText;
	/// <summary>
	/// Reference to the dialogue text TMP.
	/// </summary>
	[SerializeField] private TextMeshProUGUI _dialogueText;
	/// <summary>
	/// Reference to the dialogue box animator.
	/// </summary>
	[SerializeField] private Animator _animator;

	/// <summary>
	/// Sentences to write on screen.
	/// </summary>
	private Queue<string> _sentences;
	/// <summary>
	/// Reference to the canvas manager.
	/// </summary>
	private CanvasManager _canvasManager;
	/// <summary>
	/// Reference to the NPC detected.
	/// </summary>
	private NPC _tempNPC;
	/// <summary>
	/// Reference to the Level Changer.
	/// </summary>
	private LevelChanger _levelChanger;

	/// <summary>
	/// Properyt that returns the reference to the continue button.
	/// </summary>
	public Button ContinueButton => _continueButton;
	/// <summary>
	/// Property that defines the dialogue option chosen.
	/// </summary>
	public int DialogueChosen { get; private set; }

	/// <summary>
	/// Event called when dialogue begins.
	/// </summary>
	public event Action DialogueBegin;
	/// <summary>
	/// Event called when dialogue ends.
	/// </summary>
	public event Action DialogueEnded;

	/// <summary>
	/// Unity Awake Method.
	/// </summary>
	private void Awake()
	{
		// Find References
		_canvasManager = FindObjectOfType<CanvasManager>();
		_levelChanger = FindObjectOfType<LevelChanger>();
		_sentences = new Queue<string>();
	}

	/// <summary>
	/// Method that activates the dialogue UI.
	/// </summary>
	/// <param name="npc">NPC detected.</param>
	public void ActivateDialogue(NPC npc)
	{
		// Set temporary npc
		_tempNPC = npc;
		// Call event
		OnDialogueBegin();
		// Show multiple choice dialogue UI
		_canvasManager.ShowMultipleDialogueChoiceUI(npc);
		_continueButton.enabled = false;
	}

	/// <summary>
	/// Method that sets the dialogue to be written on screen.
	/// </summary>
	public void SetDialogue()
	{
		IEnumerable<string> sentencesToType = new List<string>();

		// Set NPC name
		_nameText.text = _tempNPC.Name;

		// According to the player choice
		switch (DialogueChosen)
		{
			case 1:
				// Get NPC dialogue and start dialogue
				sentencesToType = _tempNPC.GetDialogue(DialogueChosen);
				StartDialogue(sentencesToType);
				break;
			case 2:
				// Get NPC dialogue and start dialogue
				sentencesToType = _tempNPC.GetDialogue(DialogueChosen);
				StartDialogue(sentencesToType);
				break;
			case 3:
				// Get NPC dialogue and start dialogue
				sentencesToType = _tempNPC.GetDialogue(DialogueChosen);
				StartDialogue(sentencesToType);
				break;
			case 4:
				EndDialogue();
				break;
		}
	}

	/// <summary>
	/// Method called after player chooses a dialogue option.
	/// </summary>
	/// <param name="sentencesToType">All NPC dialogue/sentences.</param>
	public void StartDialogue(IEnumerable<string> sentencesToType)
	{
		// Space Button is now selected in dialogues
		_continueButton.Select();
		// Activate Dialogue Continue Button
		_continueButton.enabled = true;

		// Hide multiple choice UI
		_canvasManager.HideMultipleDialogueChoiceUI();
		// Show dialogue box
		_canvasManager.ShowDialogueBox();

		// Clear the queue
		_sentences.Clear();

		foreach (string sentence in sentencesToType)
			// Queue sentences to write
			_sentences.Enqueue(sentence);

		DisplayNextSentence();
	}

	/// <summary>
	/// Method called when player presses the continue button.
	/// </summary>
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

	/// <summary>
	/// Coroutine that writes each word on a delay.
	/// </summary>
	/// <param name="sentence">Sentence to write</param>
	/// <returns>Return coroutine value.</returns>
	IEnumerator TypeSentence(string sentence)
	{
		// Initialise UI text with no letters
		_dialogueText.text = "";

		// Go through the string and write each letter with delay
		foreach (char letter in sentence)
		{
			// Write letter
			_dialogueText.text += letter;
			yield return null;
		}
	}

	/// <summary>
	/// Method that ends the dialogue.
	/// </summary>
	public void EndDialogue()
	{
		// Call event
		OnDialogueEnded();
		// Hide dialogue box
		_canvasManager.HideDialogueBox();
		// If NPC is supposed to change scene, change scene
		if (DialogueChosen == 1 && _tempNPC.IsSceneChanger) _levelChanger.FadeOut();
		// Reset dialogue option and npc
		DialogueChosen = 0;
		_tempNPC = null;
	}

	/// <summary>
	/// Method that is called when option 1 is chosen on dialogue.
	/// </summary>
	public void ChooseOption1()
	{
		DialogueChosen = 1;
		SetDialogue();
	}

	/// <summary>
	/// Method that is called when option 2 is chosen on dialogue.
	/// </summary>
	public void ChooseOption2()
	{
		DialogueChosen = 2;
		SetDialogue();
	}

	/// <summary>
	/// Method that is called when option 3 is chosen on dialogue.
	/// </summary>
	public void ChooseOption3()
	{
		DialogueChosen = 3;
		SetDialogue();
	}

	/// <summary>
	/// Method that is called when option 4 is chosen on dialogue.
	/// </summary>
	public void ChooseOption4()
	{
		DialogueChosen = 4;
		SetDialogue();
	}

	/// <summary>
	/// Method that calls the event when dialogue ends.
	/// </summary>
	protected virtual void OnDialogueEnded() => DialogueEnded?.Invoke();

	/// <summary>
	/// Method that calls the event when dialogue begins.
	/// </summary>
	protected virtual void OnDialogueBegin() => DialogueBegin?.Invoke();
}
