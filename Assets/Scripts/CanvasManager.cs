using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private GameObject interactionPanel;
    [SerializeField]
    private Text interactionText;
    [SerializeField]
    private Sprite defaultInventorySlotImage;
    [SerializeField]
    private Image[] inventorySlotsUI;

    //private static CanvasManager instance;
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

    public void ManageInventoryItemIcons(List<Interactable> inventory)
    {
        for (int i = 0; i < inventorySlotsUI.Length; i++)
        {
            if (i < inventory.Count)
            {
                inventorySlotsUI[i].sprite = inventory[i].inventoryIcon;
            }
            else
            {
                inventorySlotsUI[i].sprite = defaultInventorySlotImage;
            }
        }
    }
}
