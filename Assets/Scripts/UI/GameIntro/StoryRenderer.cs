using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class that handles the Rendering of the game intro.
/// </summary>
public class StoryRenderer : MonoBehaviour
{
	/// <summary>
	/// Reference to the UI element containg the story text.
	/// </summary>
	[SerializeField] private TextMeshProUGUI _storyTextUI;
	/// <summary>
	/// Reference to the UI element containg the button text.
	/// </summary>
	[SerializeField] private TextMeshProUGUI _buttonText;
	/// <summary>
	/// Reference to the UI element containg the continue button.
	/// </summary>
	[SerializeField] private Button _nextTextButton;
	/// <summary>
	/// Duration of the fade out or in.
	/// </summary>
	[SerializeField] private float _fadeDuration;
	/// <summary>
	/// 
	/// </summary>
	[TextArea(3,20)]
	[SerializeField] private List<string> _storyText;

	/// <summary>
	/// Reference to the Level Changer.
	/// </summary>
	private LevelChanger _levelChanger;
	/// <summary>
	/// Next story text to be shown on screen.
	/// </summary>
	private int _nextIndex = 0;

	/// <summary>
	/// Unity Awake Method.
	/// </summary>
	private void Awake()
	{
		// Find references
		_levelChanger = FindObjectOfType<LevelChanger>();
	}

	/// <summary>
	/// Unity Start Method.
	/// </summary>
	public void Start()
	{
		// Set Initial Text
		SetNextText(_nextIndex);
	}

	/// <summary>
	/// Method called when user presses to continue reading the story.
	/// </summary>
	public void ContinueStoryButton()
	{
		StopAllCoroutines();
		StartCoroutine(ContinueStory());
	}

	/// <summary>
	/// Coroutine to be started when user presses continue (to read more story).
	/// </summary>
	/// <returns>Returns value from coroutine.</returns>
	private IEnumerator ContinueStory()
	{
		// Fade out UI
		FadeOut();
		// Wait 3 Seconds
		yield return new WaitForSeconds(3);
		// If there is still more story to tell
		if (_nextIndex < _storyText.Count)
			// Fade in UI
			FadeIn();
		// If story is over
		else
			// Go to the next scene
			_levelChanger.FadeOut();
	}

	/// <summary>
	/// Method that fades out all UI elements for the story.
	/// </summary>
	private void FadeOut()
	{
		// Fade out all UI elements
		FadeText(_storyTextUI, false);
		FadeButton(_nextTextButton, false);
		FadeText(_buttonText, false);
	}

	/// <summary>
	/// Method that fades in all UI elements for the story.
	/// </summary>
	private void FadeIn()
	{
		// Choose the next text to be shown
		SetNextText(_nextIndex);
		// Fade in all UI elements
		FadeText(_storyTextUI, true);
		FadeButton(_nextTextButton, true);
		FadeText(_buttonText, true);
	}

	/// <summary>
	/// Method that decides the next text to be shown in the story.
	/// </summary>
	public void SetNextText(int storyIndex)
	{
		// Set next text t be shown
		_storyTextUI.text = _storyText[_nextIndex];
		// Increase the index to be shown next
		_nextIndex++;
	}

	/// <summary>
	/// Method to handle text fading in or out.
	/// </summary>
	/// <param name="text">Text to fade.</param>
	/// <param name="fadeIn">Bool to decide between fade in ou out.</param>
	public void FadeText(TextMeshProUGUI text, bool fadeIn)
	{
		// If not to fade in
		if (!fadeIn)
			// Fade text out
			text.CrossFadeAlpha(0, _fadeDuration, true);
		// If to fade in
		else
		{
			// Fade text in
			text.canvasRenderer.SetAlpha(0);
			text.CrossFadeAlpha(1, _fadeDuration, true);
		}
	}

	/// <summary>
	/// Method to handle buttons fading in or out.
	/// </summary>
	/// <param name="button">Button to fade.</param>
	/// <param name="fadeIn">Bool to decide between fade in ou out.</param>
	public void FadeButton(Button button, bool fadeIn)
	{
		// If not to fade in
		if (!fadeIn)
		{
			// Disable the button interaction
			button.enabled = false;
			// Fade out the button image
			button.image.CrossFadeAlpha(0, _fadeDuration, true);
		}
		// If to fade in
		else
		{
			// Enable the button interaction
			button.enabled = true;
			// Fade in the button image
			button.image.canvasRenderer.SetAlpha(0);
			button.image.CrossFadeAlpha(1, _fadeDuration, true);
		}
	}
}
