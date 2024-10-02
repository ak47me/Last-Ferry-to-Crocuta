using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;  // For using UI components like Image

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject currentCard;  // Keep track of the card currently in this DropZone
    private Image dropZoneImage;     // Reference to the Image component of the DropZone

    public Color normalColor = Color.gray;        // Default color when not highlighted
    public Color highlightColor = Color.green;     // Color when highlighted

    private void Start()
    {
        // Get the Image component (assuming there's an Image component on the DropZone)
        dropZoneImage = GetComponent<Image>();

        // Set initial color (default)
        if (dropZoneImage != null)
        {
            dropZoneImage.color = normalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change color when a card is dragged over the DropZone
        if (dropZoneImage != null)
        {
            dropZoneImage.color = highlightColor;
        }
        Debug.Log("OnEnterDropZone");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Revert color when the card leaves the DropZone
        if (dropZoneImage != null)
        {
            dropZoneImage.color = normalColor;
        }
        Debug.Log("OnExitDropZone");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " dropped onto " + gameObject.name);

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            // Check if there's already a card in the drop zone
            if (currentCard == null)
            {
                // Set the dropped card's parent to this drop zone
                d.parentToReturnTo = this.transform;
                currentCard = eventData.pointerDrag;  // Store reference to this card
                RectTransform cardRectTransform = currentCard.GetComponent<RectTransform>();
                RectTransform dropZoneRectTransform = GetComponent<RectTransform>();
                //cardRectTransform.sizeDelta = dropZoneRectTransform.sizeDelta;
                cardRectTransform.SetParent(dropZoneRectTransform, false);  
                cardRectTransform.anchorMin = new Vector2(0, 0);  // Set anchor to bottom-left
                cardRectTransform.anchorMax = new Vector2(1, 1);  // Set anchor to top-right
                cardRectTransform.offsetMin = Vector2.zero;  // Reset the minimum offset
                cardRectTransform.offsetMax = Vector2.zero;  // Reset the maximum offset
                cardRectTransform.localPosition = Vector3.zero;  
                Debug.Log("Card successfully dropped in the DropZone.");
            }
            else
            {
                // Slot is full, reject the new card
                dropZoneImage.color = Color.red;
                Debug.Log("DropZone already has a card!");
            }

            // Reset the highlight after the card is dropped
            if (dropZoneImage != null)
            {
                dropZoneImage.color = normalColor;
            }
        }
    }

    // Method to clear the current card when it is removed (dragged out)
    public void ClearCurrentCard()
    {
        Debug.Log("Card removed from the DropZone.");
        currentCard = null;
    }
}
