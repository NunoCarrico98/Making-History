using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] private GameObject _pauseMenu;
	[SerializeField] private LevelChanger _levelChanger;

	private Player _player;
	private bool _paused;

	private void Awake()
	{
		_player = FindObjectOfType<Player>();
	}

	private void Update()
	{
		PauseUnpauseGame();
	}

	private void PauseUnpauseGame()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			if (!_paused)
				PauseGame();
			else
				ResumeGame();
	}

	private void PauseGame()
	{
		_paused = true;
		_player.DisablePlayerControls();
		_player.EnableMouseLock();
		_pauseMenu.SetActive(true);
	}

	public void ResumeGame()
	{
		_paused = false;
		_player.EnablePlayerControls();
		_player.DisableMouseLock();
		_pauseMenu.SetActive(false);
	}

	public void SaveGame()
	{

	}

	public void BackToMainMenu()
	{
		_pauseMenu.SetActive(false);
		_levelChanger.ToMainMenu = true;
		_levelChanger.FadeOut();
	}
}
