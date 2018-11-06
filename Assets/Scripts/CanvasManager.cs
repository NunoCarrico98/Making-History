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

    private Transform inventorySlots;
    private bool[] slots;

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
        slots = new bool[5];
        HideInteractionPanel();

        inventorySlots = transform.GetChild(0);
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

    public void ManageInventoryItemsImages(Inventory inv)
    {
        int index = 0;
        foreach (GameObject go in inv.Slots)
        {
            if (go != null && slots[index] == false)
            {
                Sprite img = inv.Slots[index].GetComponent<Pickable>().image;
                slots[index] = true;
                inventorySlots.GetChild(index).GetChild(0).GetComponent<Image>().sprite = img;
                Debug.Log(inventorySlots.GetChild(index).name);
                break;
            }
            else
            {
                // Default Image
            }
            index++;
        }
    }
}
