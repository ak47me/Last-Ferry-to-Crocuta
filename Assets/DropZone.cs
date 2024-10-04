using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject currentCard;  // Keep track of the card currently in this DropZone
    private Image dropZoneImage;     // Reference to the Image component of the DropZone

    public Color normalColor = Color.gray;        // Default color when not highlighted
    public Color highlightColor = Color.green;    // Color when highlighted

    private void Start()
    {
        dropZoneImage = GetComponent<Image>();
        if (dropZoneImage != null)
        {
            dropZoneImage.color = normalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (dropZoneImage != null)
        {
            dropZoneImage.color = highlightColor;
        }
        Debug.Log("OnEnterDropZone");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (dropZoneImage != null)
        {
            dropZoneImage.color = normalColor;
        }
        Debug.Log("OnExitDropZone");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();
        if (draggable != null)
        {
            // Handle the card moving out of the previous slot (if any)
            if (draggable.GetOriginalParent().GetComponent<DropZone>() != null)
            {
                // If the original parent was a DropZone, clear its current card
                draggable.GetOriginalParent().GetComponent<DropZone>().ClearCurrentCard();
            }

            // Ensure the slot is empty before placing the card
            if (currentCard == null)
            {
                draggable.parentToReturnTo = this.transform;
                currentCard = eventData.pointerDrag;  // Track the current card in this slot

                // Update the card's transform to fit in the new slot
                RectTransform cardRectTransform = currentCard.GetComponent<RectTransform>();
                cardRectTransform.SetParent(this.transform, false);
                cardRectTransform.anchorMin = new Vector2(0, 0);  // Set anchor to bottom-left
                cardRectTransform.anchorMax = new Vector2(1, 1);  // Set anchor to top-right
                cardRectTransform.offsetMin = Vector2.zero;  // Reset the minimum offset
                cardRectTransform.offsetMax = Vector2.zero;  // Reset the maximum offset
                cardRectTransform.localPosition = Vector3.zero;

                Debug.Log("Card successfully dropped in the grid slot.");

                // Disable dragging once placed in the slot
                draggable.DisableDragging();  // Disable dragging after placing the card
                Debug.Log("Dragging disabled for the placed card.");
            }
            else
            {
                Debug.Log("DropZone already has a card! Cannot add another card.");
                dropZoneImage.color = Color.red;
            }
        }
    }


    // Clear the reference to the card when it is moved out
    public void ClearCurrentCard()
    {
        currentCard = null;  // Clear the reference when the card is moved to a different slot
        Debug.Log("Card removed from DropZone.");
    }
}
