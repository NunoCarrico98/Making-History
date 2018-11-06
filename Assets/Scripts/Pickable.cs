using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private string interactionText;
    [SerializeField]
    private bool isInteractable;

    private Inventory inventory;

    void Start()
    {
        if (player != null)
        {
            inventory = player.GetComponent<Inventory>();
        }
    }

    public string GetInteractionText()
    {
        return interactionText;
    }

    public void Interact()
    {
        if (isInteractable)
        {
            int index = 0;
            foreach (GameObject p in inventory.Inv)
            {
                if (p == null && gameObject.GetComponent<Pickable>() != null)
                {
                    inventory.Inv[index] = gameObject;
                    if (inventory.Inv[index] != null)
                        Debug.Log(inventory.Inv[index].name);
                    Destroy(gameObject);
                }
                index++;
            }
        }
    }

    public bool IsInteractable()
    {
        return isInteractable;
    }
}
