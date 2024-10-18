using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardPosition : MonoBehaviour, IDropHandler
{
    public CardView card;   // Reference to the card in this position
    public Image image;
    public int boardRow;
    public int boardCol;

    public Color baseColor = Color.white;
    public Color modColor = Color.black;

    void Start()
    {
        image = GetComponent<Image>();

        if (image != null)
        {
            image.color = baseColor;
        }
    }

    public bool hasCard()
    {
        return card != null;
    }
    public bool getCard()
    {
        return card;
    }

    public void setCard(CardView card)
    {
        this.card = card;
        card.transform.position = transform.position;
        card.transform.localScale = card.cardInfo.boardScale;
        image.color = baseColor;
    }

    public void setBoardPosition(int y, int x)
    {
        this.boardRow = y;
        this.boardCol = x;
    }

    // Handles a card being dropped into this board position
    public void OnDrop(PointerEventData eventData)
    {
        // Get the draggable object being dropped
        CardMover mover = eventData.pointerDrag.GetComponent<CardMover>();

        if (mover.locked)
        {
            return;
        }

        if (mover != null)
        {
            // Validate the card type based on row
            if (canDropCard(mover.card))
            {
                setCard(mover.card);

                // Set this DropZone as the new parent of the dropped card
                mover.parentToReturnTo = this.transform;

                // Optionally, you can snap the card to the center of the DropZone
                mover.transform.position = this.transform.position;
                mover.startPos = this.transform.position;
                mover.startScale = mover.transform.localScale;
            }
            else
            {
                Debug.Log("You can't place that there.");
            }
        }
    }

    private bool canDropCard(CardView card)
    {
        if (card == null || card.cardInfo == null)
        {
            Debug.LogError("Attempting to drop null card.");
            return false;
        }
        else if (hasCard())
        {
            return false;
        }

        switch (boardRow)
        {
            case 0:
                return card.cardInfo.type == CardInfo.cardType.Enemy;
            case 1:
                return card.cardInfo.type == CardInfo.cardType.HandCard;
            case 2:
                return card.cardInfo.type == CardInfo.cardType.KeyCard;
            default:
                Debug.LogWarning("Invalid row");
                return false;
        }
    }

    public void clearCard()
    {
        card = null;
    }
}
