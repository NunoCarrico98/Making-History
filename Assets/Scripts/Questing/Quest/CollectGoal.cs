using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGoal : IQuestGoal
{
    public bool Completed { get; set; }
    public int ItemID { get; }

    public CollectGoal(int itemID)
    {
        ItemID = itemID;
    }

    public void CheckForCompletion(IEnumerable<InventoryItem> inventory)
    {
        foreach (InventoryItem item in inventory)
        {
            if (item.ID == ItemID) Completed = true;
        }
    }
}
