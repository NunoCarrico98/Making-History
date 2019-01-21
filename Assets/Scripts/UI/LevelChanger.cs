using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private string _nextScene;

	public bool ToMainMenu { get; set; }

	public void FadeOut()
	{
		_animator.SetTrigger("FadeOut");
	}

	public void OnFadeComplete()
	{
		if (!ToMainMenu)
			SceneManager.LoadScene(_nextScene);
		else
			SceneManager.LoadScene("MainMenu");
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
