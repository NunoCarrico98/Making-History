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
        _defaultOptionSelected.Select();
        _defaultOptionSelected.OnSelect(null);

        _optionsUI.SetActive(true);
		_optionsText[0].text = npc.OptionsText[0];
		_optionsText[1].text = npc.OptionsText[1];
		_optionsText[2].text = npc.OptionsText[2];
	}

	public void HideMultipleDialogueChoiceUI()
	{
		_optionsUI.SetActive(false);
	}
}
