using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionOBjective : QuestObjective 
{
	public int ItemID { get; private set; }

	public CollectionOBjective(int itemID, bool completed, int currentAmmount, 
							   int requiredAmmount)
	{
		ItemID			= itemID;
		Completed		= completed;
		CurrentAmmount	= currentAmmount;
		RequiredAmmount = requiredAmmount;
	}

	private void ItemPickeUp(InventoryItem item)
	{
		if(item.ID == ItemID)
		{
			CurrentAmmount++;
			CheckProgress();
		}
	}
}
