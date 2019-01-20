using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoryRenderer : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _storyTextUI;
	[SerializeField] private Button _nextTextButton;
	[SerializeField] private float _fadeDuration;
	[TextArea(3,20)]
	[SerializeField] private List<string> _storyText;

	private LevelChanger _levelChanger;
	private int _nextIndex = 1;

	private void Awake()
	{
		_levelChanger = FindObjectOfType<LevelChanger>();
	}

	public void Start()
	{
		_storyTextUI.text = _storyText[0];
	}

	public void ContinueStoryButton()
	{
		StartCoroutine(ContinueStory());
	}

	private IEnumerator ContinueStory()
	{
		FadeText(_storyTextUI, false);
		FadeButton(_nextTextButton, false);
		yield return new WaitForSeconds(3);
		if (_nextIndex < _storyText.Count)
		{
			SetNextText();
			FadeText(_storyTextUI, true);
			FadeButton(_nextTextButton, true);
		}
		else
		{
			_levelChanger.FadeOut();
		}
	}

	public void SetNextText()
	{
		_storyTextUI.text = _storyText[_nextIndex];
		_nextIndex++;
	}

	public void FadeText(TextMeshProUGUI text, bool fadeIn)
	{
		if (!fadeIn)
			text.CrossFadeAlpha(0, _fadeDuration, true);
		else
		{
			text.canvasRenderer.SetAlpha(0);
			text.CrossFadeAlpha(1, _fadeDuration, true);
		}
	}

	public void FadeButton(Button button, bool fadeIn)
	{
		if (!fadeIn)
		{
			button.enabled = false;
			button.image.CrossFadeAlpha(0, _fadeDuration, true);
		}
		else
		{
			button.enabled = true;
			button.image.canvasRenderer.SetAlpha(0);
			button.image.CrossFadeAlpha(1, _fadeDuration, true);
		}
	}
}
