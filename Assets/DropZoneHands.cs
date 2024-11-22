using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZoneHands : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Define the standard width and height for all cards in the hand
    public float cardWidth = 35f;  // Desired card width
    public float cardHeight = 60f; // Desired card height

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Optional: You can add hover effects here for when the card enters the drop zone
        Debug.Log("OnEnterDropZone");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Optional: Handle when pointer exits the drop zone
        Debug.Log("OnExitDropZone");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " dropped onto " + gameObject.name);

        // Get the Draggable component of the dragged card
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            // Set the dragged card's parent to this DropZone (Hand)
            d.parentToReturnTo = this.transform;

            // Get the RectTransform of the dragged card to modify its size and position
            RectTransform cardRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();

            // Set this DropZone (Hand) as the card's parent
            cardRectTransform.SetParent(this.transform);

            // Reset the local position, rotation, and scale to ensure proper placement in the hand
            cardRectTransform.localPosition = Vector3.zero; // Center the card in the slot
            cardRectTransform.localRotation = Quaternion.identity; // Reset rotation to default
            cardRectTransform.localScale = Vector3.one; // Ensure the scale is normal (1, 1, 1)

            // Set the size of the card to match the standard size for cards in the hand
            cardRectTransform.sizeDelta = new Vector2(cardWidth, cardHeight); // Reset size to predefined dimensions

            Debug.Log("Card successfully dropped and resized in the DropZone (Hand).");
        }
    }
}

