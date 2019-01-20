using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryRenderer : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _storyTextUI;
	[SerializeField] private List<string> _storyText;
	[SerializeField] private float duration;

	public void Start()
	{
		_storyTextUI.text = _storyText[0];
	}

	public void ContinueStory()
	{
		_storyTextUI.CrossFadeAlpha(0, );
	}
}
