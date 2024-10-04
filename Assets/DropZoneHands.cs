using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZoneHands : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public float cardWidthInHand = 50f;  // Set to your desired card width in hand
    public float cardHeightInHand = 75f; // Set to your desired card height in hand

    public float cardWidthInGrid = 120f;  // Set the card size when placed in the grid
    public float cardHeightInGrid = 180f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnEnterDropZoneHand");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnExitDropZoneHand");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " dropped onto " + gameObject.name);

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            Transform parentTransform = d.GetOriginalParent(); // Get the original parent of the card

            // Check if the card's original parent is a PlayerHand or EnemyHand
            bool isPlayerCard = parentTransform.CompareTag("PlayerHand");
            bool isEnemyCard = parentTransform.CompareTag("EnemyHand");

            if (this.CompareTag("PlayerHand") || this.CompareTag("EnemyHand"))
            {
                if ((this.CompareTag("PlayerHand") && isEnemyCard) || (this.CompareTag("EnemyHand") && isPlayerCard))
                {
                    Debug.Log("Invalid drop! Card cannot be added to this hand.");
                    d.parentToReturnTo = parentTransform;
                    return;
                }

                Debug.Log("Card dropped in PlayerHand or EnemyHand.");
                d.parentToReturnTo = this.transform;

                RectTransform cardRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
                cardRectTransform.SetParent(this.transform);
                cardRectTransform.localPosition = Vector3.zero;
                cardRectTransform.localRotation = Quaternion.identity;
                cardRectTransform.localScale = Vector3.one;

                cardRectTransform.sizeDelta = new Vector2(cardWidthInHand, cardHeightInHand);

                Debug.Log("Card resized and dropped into the hand.");
            }
            else if (this.CompareTag("GridZone"))
            {
                Debug.Log("Card dropped in GridZone.");

                d.parentToReturnTo = this.transform;

                // Set the card's parent to the grid and adjust its position and size
                RectTransform cardRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
                cardRectTransform.SetParent(this.transform);
                cardRectTransform.localPosition = Vector3.zero;
                cardRectTransform.localRotation = Quaternion.identity;
                cardRectTransform.localScale = Vector3.one;
                cardRectTransform.sizeDelta = new Vector2(cardWidthInGrid, cardHeightInGrid);

                Debug.Log("Card resized and placed in GridZone. Disabling dragging...");

                d.DisableDragging();  // Disable dragging after placing it in the grid

                Debug.Log("Dragging disabled for the card placed in the grid.");
            }


        }
    }
}
