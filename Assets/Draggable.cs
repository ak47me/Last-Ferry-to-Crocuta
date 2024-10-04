using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    private DropZone previousDropZone = null;  // Reference to the previous DropZone
    public CardData cardData;  // Reference to the card data
    private CanvasGroup canvasGroup;  // Reference to CanvasGroup component

    private void Awake()
    {
        // Get the CanvasGroup component for managing raycasts during drag
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup component is missing from the card.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");

        parentToReturnTo = this.transform.parent;

        // If the card was in a drop zone, inform that drop zone that the card is being removed
        previousDropZone = parentToReturnTo.GetComponent<DropZone>();
        if (previousDropZone != null)
        {
            previousDropZone.ClearCurrentCard();
        }

        // Set parent to a higher level to avoid being clipped by other UI elements
        this.transform.SetParent(this.transform.parent.parent);

        // Disable raycasts so the card can be dropped onto DropZones

        canvasGroup.blocksRaycasts = false;


        // Get the card data from the card being dragged
        CardDisplay cardDisplay = GetComponent<CardDisplay>(); // Ensure it's on the same GameObject
        if (cardDisplay != null)
        {
            cardData = cardDisplay.cardData; // Assign card data
            Debug.Log("Card Data Retrieved: " + cardData.cardName);
        }
        else
        {
            Debug.LogError("CardDisplay component not found on this GameObject.");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        this.transform.SetParent(parentToReturnTo);

        // Re-enable raycasts so the card can be interacted with again
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }
    }
}