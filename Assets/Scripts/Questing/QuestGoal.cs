using UnityEngine;
using System;

[System.Serializable]
public class QuestGoal
{
    [SerializeField] private GoalType _goalType;
    [SerializeField] private int _itemID;
    [SerializeField] private int _requiredAmmount;

    private int _currentAmmount = 0;
    public bool completed;
    public bool Completed { get; set; }
    public int ItemID => _itemID;
    public int RequiredAmmount => _requiredAmmount;

    public void CheckForCompletion(IInteractable interactable)
    {
        if (!Completed)
        {
            CheckForType(interactable);
        }
    }

    public void CheckForType(IInteractable interactable)
    {
        switch (_goalType)
        {
            case GoalType.Collect:
                Debug.Log("isto vais er chamado 2 xx");
                CollectComplete(interactable);
                break;
            case GoalType.Speak:
                break;
        }
    }

    private void CollectComplete(IInteractable interactable)
    {
        InventoryItem item = interactable as InventoryItem;
        Debug.Log(item.ID);
        Debug.Log(ItemID);
        if (item.ID == ItemID)
        {
            Debug.Log("isto vais er chamado 2 x");
            _currentAmmount++;
            if (_currentAmmount == _requiredAmmount)
            {
                Debug.Log("isto vais er chamado 1 x");
                completed = true;
                Completed = true;
            }
        }
    }

    public enum GoalType
    {
        Collect,
        Speak
    }

    public static string[] GetEnumValuesAsStrings()
    {
        return Enum.GetNames(typeof(GoalType));
    }
}
