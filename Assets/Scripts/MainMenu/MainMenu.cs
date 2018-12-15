using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void Play()
    {
		GameManager.Instance.ChangeScene();
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
