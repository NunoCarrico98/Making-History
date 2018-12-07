using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject _interactionPanel;
    [SerializeField] private Text		_interactionText;
    [SerializeField] private Sprite		_defaultInventorySlotImage;
    [SerializeField] private Image[]	_inventorySlotsUI;

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
    }

    public void HideInteractionPanel()
    {
        _interactionPanel.SetActive(false);
        _interactionText.enabled = false;
    }

    public void ShowInteractionPanel(string text)
    {
        _interactionText.text = text;
        _interactionPanel.SetActive(true);
        _interactionText.enabled = true;
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
}
