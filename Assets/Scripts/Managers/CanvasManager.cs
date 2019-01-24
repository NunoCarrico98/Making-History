using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Class that manages all canvas operations.
/// </summary>
public class CanvasManager : MonoBehaviour
{
	/// <summary>
	/// Reference to the interaction panel.
	/// </summary>
	[Header("Interaction UI")]
	[SerializeField] private GameObject _interactionPanel;
	/// <summary>
	/// Reference to the interation text.
	/// </summary>
	[SerializeField] private TextMeshProUGUI _interactionText;

	/// <summary>
	/// Reference to the animator in the inventory panel.
	/// </summary>
	[Header("Inventory UI")]
	[SerializeField] private Animator _inventoryPanel;
	/// <summary>
	/// Default image on inventory slot when inventory is empty.
	/// </summary>
	[SerializeField] private Sprite _defaultInventorySlotImage;
	/// <summary>
	/// Reference to the inventory slots images.
	/// </summary>
	[SerializeField] private Image[] _inventorySlotsUI;
	/// <summary>
	/// Wait time before hiding inventory.
	/// </summary>
	[SerializeField] private float _waitTimeBeforeHide;

	/// <summary>
	/// Reference to the default button selected.
	/// </summary>
	[Header("Multiple Choice UI")]
	[SerializeField] private Button _defaultOptionSelected;
	/// <summary>
	/// Reference to the default button chosen after quest.
	/// </summary>
	[SerializeField] private Button _defaultOptionAfterQuest;
	/// <summary>
	/// Reference to the dialogue options entire UI.
	/// </summary>
	[SerializeField] private GameObject _optionsUI;
	/// <summary>
	/// Reference to the dialogue options texts.
	/// </summary>
	[SerializeField] private TextMeshProUGUI[] _optionsText;

	/// <summary>
	/// Reference to the dialogue box.
	/// </summary>
	[Header("DialogueBox")]
	[SerializeField] private GameObject _dialogueBoxPanel;

	/// <summary>
	/// Last button selected before pressing the mouse.
	/// </summary>
	private GameObject _lastSelected;

	/// <summary>
	/// Unity Start Method.
	/// </summary>
	private void Start()
	{
		HideInteractionPanel();
		HideMultipleDialogueChoiceUI();
		HideDialogueBox();
	}

	/// <summary>
	/// Unity Update Method.
	/// </summary>
	private void Update() => RefreshButtonFocus();

	/// <summary>
	/// Method that hides the interaction panel.
	/// </summary>
	public void HideInteractionPanel()
	{
		_interactionPanel.SetActive(false);
		_interactionText.gameObject.SetActive(false);
	}

	/// <summary>
	/// Method that shows the interaction panel.
	/// </summary>
	/// <param name="text">Text to write.</param>
	public void ShowInteractionPanel(string text)
	{
		_interactionText.text = text;
		_interactionPanel.SetActive(true);
		_interactionText.gameObject.SetActive(true);
	}

	/// <summary>
	/// Method that sets the interaction text for all interactables.
	/// </summary>
	/// <param name="interactable">Interactacble detected.</param>
	/// <param name="inventory">Player inventory.</param>
	public void SetInteractionText(IInteractable interactable, Inventory inventory)
	{
		SetNPCInteractionText(interactable);
		SetStaticInteractableInteractionText(interactable);
		SetItemInteractionText(interactable, inventory);
	}

	/// <summary>
	/// Method that sets the interaction text for NPCs.
	/// </summary>
	/// <param name="npc">NPC detected.</param>
	private void SetNPCInteractionText(IInteractable npc)
	{
		if (npc is NPC && npc.IsActive)
			ShowInteractionPanel(npc.InteractionText);
		else if (npc is NPC && !npc.IsActive)
			HideInteractionPanel();
	}

	/// <summary>
	/// Method that sets the interaction text for static interactable when in 
	/// after quest state.
	/// </summary>
	/// <param name="interactable">Interactacble detected.</param>
	private void SetStaticInteractableInteractionText(IInteractable interactable)
	{
		if (interactable is StaticInteractable)
			if ((interactable as StaticInteractable).AfterQuest)
				ShowInteractionPanel((interactable as StaticInteractable).TextAfterQuest);
	}

	/// <summary>
	/// Method that sets the interaction text for items and static interactables.
	/// </summary>
	/// <param name="interactable">Interactacble detected.</param>
	/// <param name="inventory">Player inventory</param>
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

	/// <summary>
	/// 
	/// </summary>
	/// <returns>Return coroutine value.</returns>
	public IEnumerator HideInventory()
	{
		yield return new WaitForSeconds(_waitTimeBeforeHide);
		_inventoryPanel.SetBool("isActive", false);
	}

	/// <summary>
	/// Method that show snventory UI.
	/// </summary>
	public void ShowInventory() => _inventoryPanel.SetBool("isActive", true);

	/// <summary>
	/// Method that clears all inventory slots UI.
	/// </summary>
	private void ClearAllInventorySlotIcons()
	{
		for (int i = 0; i < _inventorySlotsUI.Length; ++i)
			ClearInventorySlotIcon(i);
	}

	/// <summary>
	/// Method that updates the inventory with the default image.
	/// </summary>
	/// <param name="slotIndex">Inventory UI slot.</param>
	public void ClearInventorySlotIcon(int slotIndex)
		=> _inventorySlotsUI[slotIndex].sprite = _defaultInventorySlotImage;

	/// <summary>
	/// Method that updates the inventory with item icon.
	/// </summary>
	/// <param name="slotIndex">Inventory UI slot.</param>
	/// <param name="icon">Item icon.</param>
	public void SetInventorySlotIcon(int slotIndex, Sprite icon)
		=> _inventorySlotsUI[slotIndex].sprite = icon;

	/// <summary>
	/// Method that manages buttons in multiple dialogue choices.
	/// </summary>
	/// <param name="npc">Current detected NPC.</param>
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

		// Manage buttons
		ManageButton1(npc);
		ManageButton2(npc);
		ManageButton3(npc);
	}

	/// <summary>
	/// Method that manages buttons when there is 1 dialogue options.
	/// </summary>
	/// <param name="npc">Current detected NPC.</param>
	private void ManageButton1(NPC npc)
	{
		if (npc is QuestGiver)
		{
			if (!(npc as QuestGiver).CompletedQuest && (npc as QuestGiver).NPCQuest.IsActive)
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
			// Activate the first button
			_optionsUI.transform.GetChild(0).gameObject.SetActive(true);

		// Button 1 always exists and receives text from the array of lists
		_optionsText[0].text = npc.GetButtonText(0);
	}

	/// <summary>
	/// Method that manages buttons when there is 2 dialogue options.
	/// </summary>
	/// <param name="npc">Current detected NPC.</param>
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

	/// <summary>
	/// Method that manages buttons when there is 3 dialogue options.
	/// </summary>
	/// <param name="npc">Current detected NPC.</param>
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

	/// <summary>
	/// Method that disables the multiple dialogue UI.
	/// </summary>
	public void HideMultipleDialogueChoiceUI() => _optionsUI.SetActive(false);

	/// <summary>
	/// Method that refreshes focus to the buttons.
	/// </summary>
	private void RefreshButtonFocus()
	{
		// If no button is selected
		if (EventSystem.current.currentSelectedGameObject == null)
			// Reselect the last button selected
			EventSystem.current.SetSelectedGameObject(_lastSelected);
		else
			// Get the current button selected
			_lastSelected = EventSystem.current.currentSelectedGameObject;
	}

	/// <summary>
	/// Method that hides the dialogue box.
	/// </summary>
	public void HideDialogueBox() => _dialogueBoxPanel.SetActive(false);

	/// <summary>
	/// Method that shows the dialogue box.
	/// </summary>
	public void ShowDialogueBox() => _dialogueBoxPanel.SetActive(true);
}
