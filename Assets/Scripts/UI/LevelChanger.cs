using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that manages the level changer animations and oads other scenes.
/// </summary>
public class LevelChanger : MonoBehaviour
{
	/// <summary>
	/// String value corresponding to the next scene to be loaded.
	/// </summary>
	[SerializeField] private string _nextScene;
	/// <summary>
	/// Reference to the animator component in the gameobject.
	/// </summary>
	private Animator _animator;

	/// <summary>
	/// Property to define if player wants to go to Main Menu from the pause menu.
	/// </summary>
	public bool ToMainMenu { get; set; }

	/// <summary>
	/// Unity Awake Method.
	/// </summary>
	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	/// <summary>
	/// Method that plays the Fade out animation for the level.
	/// </summary>
	public void FadeOut()
	{
		_animator.SetTrigger("FadeOut");
	}

	/// <summary>
	/// Method that loads the next scene or the main menu(from pause menu).
	/// </summary>
	public void OnFadeComplete()
	{
		// If player chooses to go to Main Menu from the pause menu
		if (!ToMainMenu)
			SceneManager.LoadScene(_nextScene);
		// If not load next scene when called
		else
			SceneManager.LoadScene("MainMenu");
	}

	/// <summary>
	/// Method that sets the Cursor if on the scene "Credits".
	/// </summary>
	public void SetCursorLock()
	{
		// If the name of the scene is "Credits"
		if (SceneManager.GetActiveScene().name == "Credits")
		{
			// Unlock Cursor
			Cursor.lockState = CursorLockMode.None;
			// Make cursor visible
			Cursor.visible = true;
		}
	}

	/// <summary>
	/// Method that manages the ToMainMenu property.
	/// </summary>
	public void ActivateGoToMainMenu()
	{
		// Activate property
		ToMainMenu = true;
	}
}
