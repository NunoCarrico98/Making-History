using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour, IInteractable
{
    public Sprite image;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private string interactionText;
    [SerializeField]
    private bool isInteractable;

    private Inventory inventory;
    private CanvasManager canvasManager;

    void Start()
    {
        canvasManager = CanvasManager.Instance;
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
            foreach (GameObject go in inventory.Slots)
            {
                if (go == null && GetComponent<Pickable>() != null)
                {
                    inventory.Slots[index] = gameObject;
                    Debug.Log("Index: " + index + " " + inventory.Slots[index].name);
                    transform.position = player.transform.position;
                    transform.SetParent(player.transform);
                    gameObject.SetActive(false);
                    canvasManager.ManageInventoryItemsImages(inventory);
                    break;
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
