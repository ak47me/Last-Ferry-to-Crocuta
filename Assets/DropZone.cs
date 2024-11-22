using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardData currentCard;  // Keep track of the card currently in this DropZone
    private Image dropZoneImage;      // Reference to the Image component of the DropZone
    public int row;                   // Row position in the grid
    public int column;                // Column position in the grid

    public Color normalColor = Color.gray;
    public Color highlightColor = Color.green;

    private void Start()
    {
        dropZoneImage = GetComponent<Image>();
        if (dropZoneImage != null)
        {
            dropZoneImage.color = normalColor;
        }
    }
    public bool HasCard()
    {
        return currentCard != null;
    }
    public CardData GetCard()
    {
        return currentCard;
    }

    public void SetCard(CardData card)
    {
        currentCard = card;
        card.posX = this.row;
        card.posY = this.column;

        // Assign the card's sprite to the DropZone's Image component
        if (dropZoneImage != null)
        {
            dropZoneImage.sprite = card.cardImage;  // Ensure card.cardImage is a Sprite
            dropZoneImage.color = Color.white;      // Make sure the Image is not transparent
        }

    }


    public void SetGridPosition(int row, int column)
    {
        this.row = row;
        this.column = column;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (dropZoneImage != null)
        {
            dropZoneImage.color = highlightColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (dropZoneImage != null)
        {
            dropZoneImage.color = normalColor;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Get the draggable object being dropped
        Draggable draggable = eventData.pointerDrag.GetComponent<Draggable>();

        if (draggable != null)
        {
            // Validate the card type based on row
            if (CanDropCard(draggable))
            {
                currentCard = draggable.cardData;
                Debug.Log(currentCard);
                currentCard.posX = this.row;
                currentCard.posY = this.column;


                Debug.Log($"Card of type {draggable.cardData.cardType} dropped in row {row}, column {column}.");

                // Set this DropZone as the new parent of the dropped card
                draggable.parentToReturnTo = this.transform;

                // Optionally, you can snap the card to the center of the DropZone
                draggable.transform.position = this.transform.position;
                // Resize the draggable object to match the size of the DropZone
                RectTransform dropZoneRect = this.GetComponent<RectTransform>();
                RectTransform draggableRect = draggable.GetComponent<RectTransform>();

                if (dropZoneRect != null && draggableRect != null)
                {
                    draggableRect.sizeDelta = dropZoneRect.sizeDelta; // Match size
                    draggableRect.localScale = Vector3.one; // Reset scale to default (1,1,1)
                }
            }
            else
            {
                Debug.Log("Card type is not allowed in this row.");
            }
        }
    }

    private bool CanDropCard(Draggable card)
    {
        if (card == null || card.cardData == null)
        {
            Debug.LogError("Card is null or cardData is null.");
            return false;
        }

        switch (row)
        {
            case 0:
                return card.cardData.cardType == CardData.CardType.Enemy;
            case 1:
                return card.cardData.cardType == CardData.CardType.Resource;
            case 2:
                return card.cardData.cardType == CardData.CardType.Character;
            default:
                Debug.LogWarning("Invalid row");
                return false;
        }
    }

    public void ClearCurrentCard()
    {
        currentCard = null;
    }
}
