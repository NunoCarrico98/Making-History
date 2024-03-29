using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CheatCodes : MonoBehaviour 
{
	[Header("Go Lightning Fast")]
	[SerializeField] Transform _player;
	[SerializeField] private float _movementSpeedMultiplier;

	private bool _activeCheatCodes = false;
	private FirstPersonController _playerController;

	private void Start()
	{
		_playerController = _player.GetComponent<FirstPersonController>();
	}

	// Update is called once per frame
	void Update ()
	{
		CheckPlayerInput();
	}

	private void CheckPlayerInput()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			AreCheatsActive();
		}

		ChangeScene();
	}

	private void ChangeScene()
	{
		if (Input.GetKeyDown(KeyCode.P))
			LevelChanger.Instance.FadeOut();
	}

	private void AreCheatsActive()
	{
		if (!_activeCheatCodes)
		{
			_playerController.MoveSpeed *= _movementSpeedMultiplier;
			_playerController.UseHeadBob = false;
			_activeCheatCodes = true;
		}
		else
		{
			_playerController.MoveSpeed /= _movementSpeedMultiplier;
			_playerController.UseHeadBob = true;
			_activeCheatCodes = false;
		}
	}
}
