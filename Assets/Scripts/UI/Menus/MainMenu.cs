using UnityEngine;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private Animator _credits;
	[SerializeField] private Animator _mainMenu;

	private LevelChanger _levelChanger;
	private Animator _camAnim;

	private void Awake()
	{
		_levelChanger = FindObjectOfType<LevelChanger>();
		_camAnim = FindObjectOfType<Camera>().GetComponent<Animator>();
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
		_credits.SetBool("ButtonsDisappear", false);
		_mainMenu.SetBool("ButtonsDisappear", true);
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void PlayBackToMainMenuAnimation()
	{
		_camAnim.SetTrigger("ToMainMenu");
		_credits.SetBool("ButtonsDisappear", true);
		_mainMenu.SetBool("ButtonsDisappear", false);
	}
}
