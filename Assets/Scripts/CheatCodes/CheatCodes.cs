using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

/// <summary>
/// Class that deals with all cheat codes for the game.
/// </summary>
public class CheatCodes : MonoBehaviour
{
	/// <summary>
	/// Multiplier for the movement speed cheat code
	/// </summary>
	[Header("Go Lightning Fast")]
	[SerializeField] private float _movementSpeedMultiplier;

	/// <summary>
	/// First Person Controller asset to change player speed and head bob.
	/// </summary>
	private FirstPersonController _playerController;

	/// <summary>
	/// Start Method.
	/// </summary>
	private void Start()
	{
		_playerController = FindObjectOfType<FirstPersonController>();
	}

	/// <summary>
	/// Update Method. Is called once per frame.
	/// </summary>
	void Update()
	{
		// Method to run really fast
		RunFast();
		// Method to change scene
		ChangeScene();
	}

	/// <summary>
	/// Method with the cheat code to instantly change scene.
	/// </summary>
	private void ChangeScene()
	{
		// If Player presses key P
		if (Input.GetKeyDown(KeyCode.P))
			// Go One Scene Forwards
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		// If Player presses key O
		if (Input.GetKeyDown(KeyCode.O))
			// Go One Scene Backwards
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

	/// <summary>
	/// Method with the cheat code to run really fast.
	/// </summary>
	private void RunFast()
	{
		// When player presses key left shift, start running fast
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			// Increase player speed according to the given multiplier
			_playerController.MoveSpeed *= _movementSpeedMultiplier;
			// Disable Head Bob
			_playerController.UseHeadBob = false;
		}

		// When player lets go of key left shift, start running fast
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			// Decrease player speed according to the given multiplier
			_playerController.MoveSpeed /= _movementSpeedMultiplier;
			// Enable Head Bob
			_playerController.UseHeadBob = true;
		}
	}
}
