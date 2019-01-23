using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class that manages the Pause Menu.
/// </summary>
public class PauseMenu : MonoBehaviour
{
	/// <summary>
	/// Reference to the gameobject containing all the UI for the pause menu.
	/// </summary>
	[SerializeField] private GameObject _pauseMenu;
	/// <summary>
	/// Reference to the Level Changer Oject.
	/// </summary>
	[SerializeField] private LevelChanger _levelChanger;

	/// <summary>
	/// Reference to the player.
	/// </summary>
	private Player _player;
	/// <summary>
	/// Indicates if game is paused or not.
	/// </summary>
	private bool _paused;

	/// <summary>
	/// Unity Awake Method.
	/// </summary>
	private void Awake()
	{
		_player = FindObjectOfType<Player>();
	}

	/// <summary>
	/// Unity Update Method.
	/// </summary>
	private void Update()
	{
		// Check if player wants to pause the game
		PauseUnpauseGame();
	}

	/// <summary>
	/// Method that checks if user pressed Escape to pause the game.
	/// </summary>
	private void PauseUnpauseGame()
	{
		// If player presses Escape
		if (Input.GetKeyDown(KeyCode.Escape))
			// If game is not paused
			if (!_paused)
				// Pause the game
				PauseGame();
			// If game is paused
			else
				// Unpause the game
				ResumeGame();
	}

	/// <summary>
	/// Method called when players press Escape to pause the game.
	/// </summary>
	private void PauseGame()
	{
		// Pause Game
		_paused = true;
		// Disable player movement
		_player.DisablePlayerControls();
		// Disable camera controls with mouse
		_player.EnableMouseLock();
		// Enable pause menu UI
		_pauseMenu.SetActive(true);
	}

	/// <summary>
	/// Method called when players want to resume the game.
	/// </summary>
	public void ResumeGame()
	{
		// Unpause game
		_paused = false;
		// Enable player controls
		_player.EnablePlayerControls();
		// Enable camera controls with mouse
		_player.DisableMouseLock();
		// Disabe pause menu UI
		_pauseMenu.SetActive(false);
	}

	/// <summary>
	/// Method called when players want to save the game. NOT CURRENTLY IMPLEMENTED.
	/// </summary>
	public void SaveGame()
	{
		/* NOT CURRENTLY IMPLEMENTED */
	}

	/// <summary>
	/// Method called when user chooses to go to Main Menu.
	/// </summary>
	public void BackToMainMenu()
	{
		// Disable the pause menu UI
		_pauseMenu.SetActive(false);
		// Activate variable to go Main Menu from the Level Changer
		_levelChanger.ActivateGoToMainMenu();
		// Fade Out to Main Menu
		_levelChanger.FadeOut();
	}
}
