using UnityEngine;

public class MainMenu : MonoBehaviour
{
	private LevelChanger _levelChanger;

	private void Awake()
	{
		_levelChanger = FindObjectOfType<LevelChanger>();
	}

	public void Play()
	{
		_levelChanger.FadeOut();
	}

	public void LoadGame()
	{

	}

	public void Options()
	{

	}

	public void Quit()
	{
		Application.Quit();
	}
}
