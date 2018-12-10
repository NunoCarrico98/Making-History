using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGoal : QuestGoal
{
    [SerializeField] private int _itemID;
    [SerializeField] private int _requiredAmmount;

    private int _currentAmmount = 0;

    public int ItemID => _itemID;

    public override void CheckForCompletion(IInteractable interactable)
    {
        InventoryItem item = interactable as InventoryItem;
        if (item.ID == ItemID)
        {
            _currentAmmount++;

            if (_currentAmmount == _requiredAmmount)
                Completed = true;
        }
    }
}
