using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private string _nextScene;

	public void FadeOut()
	{
		_animator.SetTrigger("FadeOut");
	}

	public void OnFadeComplete()
	{
		SceneManager.LoadScene(_nextScene);
	}

	public void SetCursorLock()
	{
		if (SceneManager.GetActiveScene().name == "Credits")
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}
