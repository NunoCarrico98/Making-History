using UnityEngine;

/// <summary>
/// Class that handles the Main Menu from the game.
/// </summary>
public class MainMenu : MonoBehaviour
{
	/// <summary>
	/// Reference to the Level Changer.
	/// </summary>
	private LevelChanger _levelChanger;
	/// <summary>
	/// Reference to the camera animator.
	/// </summary>
	private Animator _camAnim;
	/// <summary>
	/// Reference to the canvas animator.
	/// </summary>
	private Animator _canvasAnim;
	/// <summary>
	/// Reference to the script that saves the data to then send to the database.
	/// </summary>
	private DatabaseData _dbData;

	private bool countingTime;

	/// <summary>
	/// Unity Awake Method.
	/// </summary>
	private void Awake()
	{
		// Find all references for gameobjects
		_levelChanger = FindObjectOfType<LevelChanger>();
		_camAnim = FindObjectOfType<Camera>().GetComponent<Animator>();
		_canvasAnim = GetComponent<Animator>();
		_dbData = FindObjectOfType<DatabaseData>();
	}

	/// <summary>
	/// Method that is called when user presses Play on Main Menu.
	/// </summary>
	public void Play()
	{
		// Start couting total time played
		if (!countingTime)
		{
			countingTime = true;
			_dbData.IsCountingTotalTime(true);
		}
		// Fade to game.
		_levelChanger.FadeOut();
	}

	/// <summary>
	/// Method called when player wants to load a saved game. NOT CURRENTLY IMPLEMENTED.
	/// </summary>
	public void LoadGame()
	{
		/* NOT CURRENTLY IMPLEMENTED */
	}

	/// <summary>
	/// Method that plays the animation to go to the credits from the main menu.
	/// </summary>
	public void PlayCreditsAnimation()
	{
		// Rotate camera to credits rotation
		_camAnim.SetTrigger("ToCredits");
		// Move the buttons left(Credits UI) or right(Main Menu UI)
		// if on "MainMenu" Scene
		_canvasAnim.SetBool("MainMenu", true);
		// Move the buttons left(Credits UI) or right(Main Menu UI)
		// if on "Credits" Scene
		_canvasAnim.SetBool("Credits", false);
	}

	/// <summary>
	/// Method that is called when user presses quit from the Main Menu.
	/// </summary>
	public void Quit()
	{
		// Quit game
		Application.Quit();
	}

	/// <summary>
	/// Method that plays the animation to go the main menu from the credits menu.
	/// </summary>
	public void PlayBackToMainMenuAnimation()
	{
		// Rotate camera to main menu rotation
		_camAnim.SetTrigger("ToMainMenu");
		// Move the buttons left or right if on "MainMenu" Scene
		_canvasAnim.SetBool("MainMenu", false);
		// Move the buttons left or right if on "Credits" Scene
		_canvasAnim.SetBool("Credits", true);
	}
}
