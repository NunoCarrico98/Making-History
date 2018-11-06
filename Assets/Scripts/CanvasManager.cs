using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    //private static CanvasManager instance;

    [SerializeField]
    private GameObject interactionPanel;
    [SerializeField]
    private Text interactionText;

    public static CanvasManager Instance { get; private set; }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Use this for initialization
    void Start()
    {
        HideInteractionPanel();
    }

    public void HideInteractionPanel()
    {
        interactionPanel.SetActive(false);
        interactionText.enabled = false;
    }

    public void ShowInteractionPanel(string text)
    {
        interactionText.text = text;
        interactionPanel.SetActive(true);
        interactionText.enabled = true;
    }
}
