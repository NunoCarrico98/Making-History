using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour 
{
	[SerializeField] private Animator _animator;

	public static LevelChanger Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != null)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);
	}

	public void FadeOut()
	{
		_animator.SetTrigger("FadeOut");
	}

	public void OnFadeComplete()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
