using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeBoard : MonoBehaviour
{
    private static TradeBoard _instance;
    public static TradeBoard Instance { get { return _instance; } }
    public List<List<BoardPosition>> board = new List<List<BoardPosition>>();
    public GameObject uiCanvas;
    public List<CardInfo> startingCards;
    public List<CardInfo> tradeCards;
    public GameObject greyBg;

    public CardView selectedBoardCard;
    public CardView selectedHandCard;
    public GameObject highlight;


    public void SetHighlight(bool isActive)
    {
        if (highlight != null)
        {
            highlight.SetActive(isActive);
        }
    }

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        // Start a coroutine to wait for trade cards to be ready
        StartCoroutine(InitializeBoardWhenCardsReady());
    }

    private IEnumerator InitializeBoardWhenCardsReady()
    {
        // Wait until TradeSceneManager has the trade cards ready
        while (TradeSceneManager.Instance == null ||
               TradeSceneManager.Instance.GetTradeCards() == null ||
               TradeSceneManager.Instance.GetTradeCards().Count == 0)
        {
            yield return null; // Wait for the next frame
        }

        // Get the trade cards from TradeSceneManager
        tradeCards = TradeSceneManager.Instance.GetTradeCards();

        // Proceed with the board initialization
        for (int i = 0; i < 1; i++) // Only one row (middle row) in this case
        {
            board.Add(new List<BoardPosition>());

            for (int j = 1; j < 4; j++) // Loop through 3 columns
            {
                BoardPosition pos = uiCanvas.transform.GetChild(i * 3 + j).gameObject.GetComponent<BoardPosition>();
                pos.setBoardPosition(i, j - 1);
                board[i].Add(pos);

                // Assign trade cards to each position and animate them
                assignTradeCard(i, j, pos.transform);
            }
        }
    }

    public void removeCard(int row, int col)
    {
        board[row][col].clearCard();
    }

    public void assignTradeCard(int row, int col, Transform parentTransform)
    {
        if (tradeCards != null && tradeCards.Count >= col) // Ensure enough cards are available
        {
            GameObject card = TradeSceneManager.Instance.generateCard(tradeCards[col - 1], parentTransform, "BoardCard");
            card.GetComponent<CardMover>().locked = true; // Prevent this card from being dragged

            // Start the card animation
            StartCoroutine(AnimateCardIntoPosition(card, board[row][col - 1].transform.position));

            board[row][col - 1].setCard(card.GetComponent<CardView>());
        }
        else
        {
            Debug.LogError("Not enough trade cards available in TradeSceneManager.");
        }
    }

    private IEnumerator AnimateCardIntoPosition(GameObject card, Vector3 targetPosition)
    {
        // Initial position is above the screen
        Vector3 startPosition = new Vector3(targetPosition.x, targetPosition.y + 200f, targetPosition.z); // 200 units above
        card.transform.position = startPosition;

        float duration = 2f; // Duration of the animation
        float elapsedTime = 1f;

        while (elapsedTime < duration)
        {
            card.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is set
        card.transform.position = targetPosition;
    }



    public int tradeIndex = 0;

    // Tiny finite state machine for combat
    public enum tradePhase
    {
        Setup,
        WaitingForPlayerSelection,
        PlayerSelected,
        ConfirmingTrade,
        TradeCompleted
    }

    public tradePhase currentPhase;
    public tradePhase previousPhase;

    // Update is called once per frame
    void Update()
    {
        switch (currentPhase)
        {
            case tradePhase.Setup:
                // Currently: Nothing we need to do.
                break;

            case tradePhase.WaitingForPlayerSelection:
                break;

            case tradePhase.ConfirmingTrade:
                break;

            case tradePhase.TradeCompleted:
                break;
        }
    }

    public void TradeButton()
    {
        if (currentPhase != tradePhase.Setup)
        {
            return;
        }

        unlockRowAndHand(true, true);
        

        currentPhase = tradePhase.WaitingForPlayerSelection;
        previousPhase = tradePhase.Setup;
        greyBg.SetActive(true);

    }

    public void unlockRowAndHand(bool lockVal, bool canSelect)
    {
        for (int i = 0; i < 3; i++)
        {
            board[0][i].card.gameObject.GetComponent<CardMover>().locked = lockVal;
            board[0][i].card.gameObject.GetComponent<CardMover>().canSelect = canSelect;
        }

        foreach (GameObject handcards in TradeSceneManager.Instance.hand)
        {
            handcards.GetComponent<CardMover>().canSelect = canSelect;
        }
        print("you have it to be selected");
    }

    public void SelectBoardCard(CardView card)
    {
        if (currentPhase != tradePhase.WaitingForPlayerSelection) return;

        // Handle board card selection
        if (selectedBoardCard != null)
        {
            // Disable highlight on the previously selected board card
            selectedBoardCard.SetHighlight(false);
        }

        // Update to the new selection
        selectedBoardCard = card;
        selectedBoardCard.SetHighlight(true);  // Enable highlight on the new selection
        print("Board card selected");
    }

    public void SelectHandCard(CardView card)
    {
        if (currentPhase != tradePhase.WaitingForPlayerSelection) return;

        // Handle hand card selection
        if (selectedHandCard != null)
        {
            // Disable highlight on the previously selected hand card
            selectedHandCard.SetHighlight(false);
        }

        // Update to the new selection
        selectedHandCard = card;
        selectedHandCard.SetHighlight(true);  // Enable highlight on the new selection
        print("Hand card selected");
    }


    public void ConfirmTrade()
    {
        greyBg.SetActive(false);
        if (selectedHandCard != null && selectedBoardCard != null)
        {
            // Swap the cards here
            CardInfo tempHandCardInfo = selectedHandCard.cardInfo;
            CardInfo tempBoardCardInfo = selectedBoardCard.cardInfo;

            TradeSceneManager.Instance.swapCards(tempHandCardInfo, tempBoardCardInfo);
            print("Swapped successfully");

            // Reset the selections after the trade
            selectedHandCard = null;
            selectedBoardCard = null;

        }
        else
        {
            Debug.LogWarning("Both cards need to be selected before confirming trade.");
        }
    }




}