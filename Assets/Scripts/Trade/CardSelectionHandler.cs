using System.Collections;
using UnityEngine;

public class CardSelectionHandler : MonoBehaviour
{
    //private GameObject selectedBoardCard;
    //private GameObject selectedHandCard;
    //private TradeBoard tradeBoard;

    //void Start()
    //{
    //    tradeBoard = TradeBoard.Instance; // Get the reference to TradeBoard instance
    //}

    //// Call this to start selecting a card from the board
    //public void SelectCardFromBoard(GameObject card)
    //{
    //    if (tradeBoard.currentTradePhase == TradeBoard.TradePhase.PlayerSelected)
    //    {
    //        selectedBoardCard = card;
    //        Debug.Log("Board card selected.");

    //        // Highlight or mark the selected card on the board
    //        HighlightCard(card);
    //    }
    //}

    //// Call this to start selecting a card from the hand
    //public void SelectCardFromHand(GameObject card)
    //{
    //    if (tradeBoard.currentTradePhase == TradeBoard.TradePhase.PlayerSelected)
    //    {
    //        selectedHandCard = card;
    //        Debug.Log("Hand card selected.");

    //        // Highlight or mark the selected card in the hand
    //        HighlightCard(card);
    //    }
    //}

    //private void HighlightCard(GameObject card)
    //{
    //    // Example: Change the color or size to highlight the selected card
    //    card.GetComponent<Renderer>().material.color = Color.yellow;  // Highlight the card by changing color
    //}

    //public void ConfirmTrade()
    //{
    //    if (tradeBoard.currentTradePhase == TradeBoard.TradePhase.PlayerSelected && selectedBoardCard != null && selectedHandCard != null)
    //    {
    //        // Change the phase to ConfirmingTrade
    //        tradeBoard.currentTradePhase = TradeBoard.TradePhase.ConfirmingTrade;

    //        // Animate the trade (swap the cards)
    //        StartCoroutine(AnimateTrade(selectedBoardCard, selectedHandCard));

    //        // After animation, swap the cards in the board and hand
    //        //tradeBoard.SwapCards();

    //        // Change the phase to TradeCompleted
    //        tradeBoard.currentTradePhase = TradeBoard.TradePhase.TradeCompleted;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Both cards need to be selected for trade.");
    //    }
    //}

    //private IEnumerator AnimateTrade(GameObject boardCard, GameObject handCard)
    //{
    //    // Animate the board card and hand card to their new positions
    //    Vector3 boardCardStartPos = boardCard.transform.position;
    //    Vector3 handCardStartPos = handCard.transform.position;

    //    Vector3 boardCardEndPos = handCardStartPos;
    //    Vector3 handCardEndPos = boardCardStartPos;

    //    float duration = 1f;
    //    float elapsedTime = 0f;

    //    while (elapsedTime < duration)
    //    {
    //        boardCard.transform.position = Vector3.Lerp(boardCardStartPos, boardCardEndPos, elapsedTime / duration);
    //        handCard.transform.position = Vector3.Lerp(handCardStartPos, handCardEndPos, elapsedTime / duration);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    // Set the final positions after animation
    //    boardCard.transform.position = boardCardEndPos;
    //    handCard.transform.position = handCardEndPos;
    //}
}
