using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float maxInteractionDistance;

    private CanvasManager canvasManager;
    private Camera cam;
    private RaycastHit raycastHit;
    private IInteractable currentInteractable;

    private void Start()
    {
        canvasManager = CanvasManager.Instance;
        cam = GetComponentInChildren<Camera>();
        currentInteractable = null;
    }

    // Update is called once per frame
    private void Update()
    {
        CheckForInteractable();
        CheckForInteractionClick();
    }

    private void CheckForInteractionClick()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void CheckForInteractable()
    {
        if (Physics.Raycast(cam.transform.position,
            cam.transform.forward, out raycastHit, maxInteractionDistance))
        {
            IInteractable newInteractable =
                raycastHit.collider.GetComponent<IInteractable>();

            if (newInteractable != null && newInteractable.IsInteractable())
            {
                SetInteractable(newInteractable);
            }
            else
            {
                ClearInteractable();
            }

        }
        else
        {
            ClearInteractable();
        }
    }

    private void SetInteractable(IInteractable newInteractable)
    {
        if (newInteractable != currentInteractable)
        {
            currentInteractable = newInteractable;

            canvasManager.ShowInteractionPanel(currentInteractable.GetInteractionText());
        }
    }

    private void ClearInteractable()
    {
        currentInteractable = null;

        canvasManager.HideInteractionPanel();
    }
}
