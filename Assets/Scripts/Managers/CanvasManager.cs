using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject _interactionPanel;
    [SerializeField] private TextMeshProUGUI _interactionText;
    [SerializeField] private Sprite _defaultInventorySlotImage;
    [SerializeField] private Image[] _inventorySlotsUI;

    [Header("Multiple Choice UI")]
    [SerializeField] private Button _defaultOptionSelected;
    [SerializeField] private GameObject _optionsUI;
    [SerializeField] private TextMeshProUGUI[] _optionsText;

    public static CanvasManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    private void Start()
    {
        HideInteractionPanel();
        HideMultipleDialogueChoiceUI();
    }

    public void HideInteractionPanel()
    {
        _interactionPanel.SetActive(false);
        _interactionText.gameObject.SetActive(false);
    }

    public void ShowInteractionPanel(string text)
    {
        _interactionText.text = text;
        _interactionPanel.SetActive(true);
        _interactionText.gameObject.SetActive(true);
    }

    private void ClearAllInventorySlotIcons()
    {
        for (int i = 0; i < _inventorySlotsUI.Length; ++i)
            ClearInventorySlotIcon(i);
    }

    public void ClearInventorySlotIcon(int slotIndex)
    {
        _inventorySlotsUI[slotIndex].sprite = _defaultInventorySlotImage;
    }

    public void SetInventorySlotIcon(int slotIndex, Sprite icon)
    {
        _inventorySlotsUI[slotIndex].sprite = icon;
    }

    public void ShowMultipleDialogueChoiceUI(NPC npc)
    {
        // Default selected button is the first
        _defaultOptionSelected.Select();
        _defaultOptionSelected.OnSelect(null);

        // Set buttons active
        _optionsUI.SetActive(true);

        // Deactivate buttons 2 and 3 (dialogue options)
        _optionsUI.transform.GetChild(1).gameObject.SetActive(false);
        _optionsUI.transform.GetChild(2).gameObject.SetActive(false);

        // Button 1 always exists and receives text from the array of lists
        _optionsText[0].text = npc.Dialogue.Options[0].ButtonText;
        // If there is a second dialogue option
        if (npc.Dialogue.Options.Length == 2)
        {
            // Activate the second button
            _optionsUI.transform.GetChild(1).gameObject.SetActive(true);
            // Button 2 receives text from the array of lists
            _optionsText[1].text = npc.Dialogue.Options[1].ButtonText;
        }
        if (npc.Dialogue.Options.Length == 3)
        {
            // Activate the second button
            _optionsUI.transform.GetChild(1).gameObject.SetActive(true);
            // Activate the third button
            _optionsUI.transform.GetChild(2).gameObject.SetActive(true);

            // Button 2 receives tex from the array of lists
            _optionsText[1].text = npc.Dialogue.Options[1].ButtonText;
            // Button 3 receives text from the array of lists
            _optionsText[2].text = npc.Dialogue.Options[2].ButtonText;
        }
    }

    public void HideMultipleDialogueChoiceUI()
    {
        _optionsUI.SetActive(false);
    }
}
