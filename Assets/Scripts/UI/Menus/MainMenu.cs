using UnityEngine;

public class MainMenu : MonoBehaviour
{
	private LevelChanger _levelChanger;
	private Animator _camAnim;
	private Animator _canvasAnim;

	private void Awake()
	{
		_levelChanger = FindObjectOfType<LevelChanger>();
		_camAnim = FindObjectOfType<Camera>().GetComponent<Animator>();
		_canvasAnim = GetComponent<Animator>();
	}

	public void Play()
	{
		_levelChanger.FadeOut();
	}

	public void LoadGame()
	{

	}

	public void PlayCreditsAnimation()
	{
		_camAnim.SetTrigger("ToCredits");
		_canvasAnim.SetBool("MoveRight", true);
		_canvasAnim.SetBool("MoveLeft", false);
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void PlayBackToMainMenuAnimation()
	{
		_camAnim.SetTrigger("ToMainMenu");
		_canvasAnim.SetBool("MoveRight", false);
		_canvasAnim.SetBool("MoveLeft", true);
	}
}
