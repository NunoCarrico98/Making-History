using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelChanger : MonoBehaviour 
{
	[SerializeField] private Animator _animator;

	public void FadeOut()
	{
		_animator.SetTrigger("FadeOut");
	}

	public void OnFadeComplete()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
