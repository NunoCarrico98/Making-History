using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CanvasManager : MonoBehaviour
{
	[Header("Interaction UI")]
	[SerializeField] private GameObject _interactionPanel;
	[SerializeField] private TextMeshProUGUI _interactionText;

	[Header("Inventory UI")]
	[SerializeField] private Animator _inventoryPanel;
	[SerializeField] private Sprite _defaultInventorySlotImage;
	[SerializeField] private Image[] _inventorySlotsUI;
	[SerializeField] private float _waitTimeBeforeHide;

	[Header("Multiple Choice UI")]
	[SerializeField] private Button _defaultOptionSelected;
	[SerializeField] private Button _defaultOptionAfterQuest;
	[SerializeField] private GameObject _optionsUI;
	[SerializeField] private TextMeshProUGUI[] _optionsText;

	[Header("DialogueBox")]
	[SerializeField] private GameObject _dialogueBoxPanel;

	private GameObject _lastSelected;

	// Use this for initialization
	private void Start()
	{
		HideInteractionPanel();
		HideMultipleDialogueChoiceUI();
		HideDialogueBox();
	}

	private void Update()
	{
		RefreshButtonFocus();
	}

	#region Interaction UI
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

	public void SetInteractionText(IInteractable interactable, Inventory inventory)
	{
		SetNPCInteractionText(interactable);
		SetStaticInteractableInteractionText(interactable);
		SetItemInteractionText(interactable, inventory);
	}

	private void SetNPCInteractionText(IInteractable npc)
	{
		if (npc is NPC && npc.IsActive)
			ShowInteractionPanel(npc.InteractionText);
		else if (npc is NPC && !npc.IsActive)
			HideInteractionPanel();
	}

	private void SetStaticInteractableInteractionText(IInteractable interactable)
	{
		if (interactable is StaticInteractable)
			if ((interactable as StaticInteractable).AfterQuest)
				ShowInteractionPanel((interactable as StaticInteractable).TextAfterQuest);
	}

	private void SetItemInteractionText(IInteractable interactable, Inventory inventory)
	{
		if (interactable is InventoryItem || interactable is StaticInteractable)
		{
			if (inventory.HasRequirements(interactable) && interactable.IsActive)
				ShowInteractionPanel(interactable.InteractionText);
			else
				ShowInteractionPanel(interactable.RequirementText);
		}
	}
	#endregion

	#region Inventory UI
	public IEnumerator HideInventory()
	{
		yield return new WaitForSeconds(_waitTimeBeforeHide);
		_inventoryPanel.SetBool("isActive", false);
	}

	public void ShowInventory()
	{
		_inventoryPanel.SetBool("isActive", true);
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
	#endregion

	#region Dialogue Choice Buttons
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

		ManageButton1(npc);
		ManageButton2(npc);
		ManageButton3(npc);
	}

	private void ManageButton1(NPC npc)
	{
		if (npc is QuestGiver)
		{
			if (!(npc as QuestGiver).AfterQuest && (npc as QuestGiver).NPCQuest.IsActive)
				// Activate the first button
				_optionsUI.transform.GetChild(0).gameObject.SetActive(true);
			else
			{
				// Activate the second button
				_optionsUI.transform.GetChild(0).gameObject.SetActive(false);
				_defaultOptionAfterQuest.Select();
				_defaultOptionAfterQuest.OnSelect(null);
			}
		}
		else
		{
			// Activate the first button
			_optionsUI.transform.GetChild(0).gameObject.SetActive(true);
		}

		// Button 1 always exists and receives text from the array of lists
		_optionsText[0].text = npc.GetButtonText(0);
	}

	private void ManageButton2(NPC npc)
	{
		// If there is a second dialogue option
		if (npc.Dialogue.Options.Length == 2)
		{
			// Activate the second button
			_optionsUI.transform.GetChild(1).gameObject.SetActive(true);
			// Button 2 receives text from the array of lists
			_optionsText[1].text = npc.GetButtonText(1);
		}
	}

	private void ManageButton3(NPC npc)
	{
		if (npc.Dialogue.Options.Length == 3)
		{
			// Activate the second button
			_optionsUI.transform.GetChild(1).gameObject.SetActive(true);
			// Activate the third button
			_optionsUI.transform.GetChild(2).gameObject.SetActive(true);

			// Button 2 receives tex from the array of lists
			_optionsText[1].text = npc.GetButtonText(1);
			// Button 3 receives text from the array of lists
			_optionsText[2].text = npc.GetButtonText(2);
		}
	}

	public void HideMultipleDialogueChoiceUI()
	{
		_optionsUI.SetActive(false);
	}

	private void RefreshButtonFocus()
	{
		if (EventSystem.current.currentSelectedGameObject == null)
		{
			EventSystem.current.SetSelectedGameObject(_lastSelected);
		}
		else
		{
			_lastSelected = EventSystem.current.currentSelectedGameObject;
		}
	}
	#endregion

	#region
	public void HideDialogueBox()
	{
		_dialogueBoxPanel.SetActive(false);
	}

	public void ShowDialogueBox()
	{
		_dialogueBoxPanel.SetActive(true);
	}
	#endregion
}
