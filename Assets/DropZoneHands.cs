using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZoneHands : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Define the standard width and height for all cards in the hand
    public float cardWidth = 50f;  // Set to your desired card width
    public float cardHeight = 75f; // Set to your desired card height

    public void OnPointerEnter(PointerEventData eventData)
    {
        // You can add hover effects here if you like
        Debug.Log("OnEnterDropZone");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Handle when pointer exits the drop zone
        Debug.Log("OnExitDropZone");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " dropped onto " + gameObject.name);

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            // Set the parent of the dragged card to this drop zone (the hand)
            d.parentToReturnTo = this.transform;

            // Reset the card's local position, rotation, and scale
            RectTransform cardRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
            cardRectTransform.SetParent(this.transform); // Ensure it's a child of the hand drop zone
            cardRectTransform.localPosition = Vector3.zero; // Reset position to be centered in the slot
            cardRectTransform.localRotation = Quaternion.identity; // Reset rotation

            // Set the width and height of the card to match the standard size for all cards in the hand
            cardRectTransform.sizeDelta = new Vector2(cardWidth, cardHeight); // Reset size to default
            cardRectTransform.localScale = Vector3.one; // Ensure the scale is normal

            Debug.Log("Card successfully dropped and resized in the DropZone (Hand).");
        }
    }
}
