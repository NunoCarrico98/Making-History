using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    //private static CanvasManager instance;
    public static CanvasManager Instance { get; private set; }

    [SerializeField]
    private GameObject interactionPanel;
    [SerializeField]
    private Text interactionText;
    [SerializeField]
    private Sprite defaultInventorySlotImage;

    private Transform inventorySlots;

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

    public void ManageInventoryItemsImages(IEnumerable<Interactable> inventory)
    {
        int i = 0;
        foreach (Interactable interactable in inventory)
        {
            inventorySlots = transform.GetChild(0).GetChild(i).GetChild(0);

            if (interactable != null && CheckInventorySlotImage())
            {
                // Set inventory slot image as the item picked
                inventorySlots.GetComponent<Image>().sprite = interactable.image;
            }
            else
            {
                // Set inventory slot image as default
                inventorySlots.GetComponent<Image>().sprite = defaultInventorySlotImage;
            }
            i++;
        }
    }

    private bool CheckInventorySlotImage()
    {
        return inventorySlots.GetComponent<Image>().sprite == defaultInventorySlotImage;
    }
}
