using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Transform parentToReturnTo = null;
    private Transform originalParent = null;
    private CanvasGroup canvasGroup;
    public bool isDraggable = true;
    public bool isInGrid = false; // Add this to track if the card is in a grid

    private void Start()    
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDraggable || isInGrid) return;  // Block if dragging is disabled or card is in grid
        Debug.Log("OnBeginDrag");

        originalParent = this.transform.parent;
        parentToReturnTo = originalParent;

        this.transform.SetParent(this.transform.root);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggable || isInGrid) return;  // Block dragging if disabled or in grid
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggable || isInGrid) return;  // Block if dragging is disabled or in grid
        Debug.Log("OnEndDrag");

        this.transform.SetParent(parentToReturnTo);
        canvasGroup.blocksRaycasts = true;
    }

    public void DisableDragging()
    {
        // Disable dragging completely and mark the card as placed in the grid
        isDraggable = false;
        isInGrid = true;
        canvasGroup.blocksRaycasts = true;
        Debug.Log("Dragging disabled for card: " + gameObject.name);
    }

    // To prevent any clicks from trying to move the card
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isInGrid)
        {
            // If the card is in the grid, prevent any click-based movement or re-selection
            Debug.Log("Card is locked in the grid and cannot be clicked or moved.");
            return;
        }
    }




// Method to get the original parent (for tag checking in DropZone)
public Transform GetOriginalParent()
    {
        return originalParent;
    }
    // To prevent any clicks from trying to move the card

}
