using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private static Board _instance;
    public static Board Instance { get { return _instance; } }
    public List<List<BoardPosition>> board = new List<List<BoardPosition>>();
    public GameObject uiCanvas;
    private List<CardInfo> enemyCards;
    public List<CardInfo> startingCards;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        // Start a coroutine to wait for enemy cards to be ready
        StartCoroutine(InitializeBoardWhenCardsReady());
    }

    private IEnumerator InitializeBoardWhenCardsReady()
    {
        // Wait until enemy cards are available
        while (CombatSceneManager.Instance == null || CombatSceneManager.Instance.GetEnemyCards() == null || CombatSceneManager.Instance.GetEnemyCards().Count == 0)
        {
            yield return null; // Wait for the next frame
        }

        // Get the enemy cards from CombatSceneManager
        enemyCards = CombatSceneManager.Instance.GetEnemyCards();

        for (int i = 0; i < 3; i++)
        {
            board.Add(new List<BoardPosition>());

            for (int j = 1; j < 4; j++)
            {
                BoardPosition pos = uiCanvas.transform.GetChild(i * 3 + j).gameObject.GetComponent<BoardPosition>();
                pos.setBoardPosition(i, j - 1);
                board[i].Add(pos);

                if (i == 0)
                {
                    // Assign enemy cards using the new method with CombatSceneManager's cards
                    assignEnemy(i, j, pos.transform);
                }
                else if (i == 2)
                {
                    assignHero(i, j, pos.transform);
                }
            }
        }
    }

    public void removeCard(int row, int col)
    {
        board[row][col].clearCard();
    }

    // Modified to use the enemy cards from CombatSceneManager instead of startingCards
    public void assignEnemy(int row, int col, Transform parentTransform)
    {
        if (enemyCards != null && enemyCards.Count >= col) // Ensure enough cards are available
        {
            GameObject card = HandHandler.Instance.generateCard(enemyCards[col - 1], parentTransform);
            card.GetComponent<CardMover>().locked = true; // prevent this card from being dragged
            board[row][col - 1].setCard(card.GetComponent<CardView>());
        }
        else
        {
            Debug.LogError("Not enough enemy cards available in CombatSceneManager.");
        }
    }

    public void assignHero(int row, int col, Transform parentTransform)
    {
        GameObject card = HandHandler.Instance.generateCard(startingCards[2 + col], parentTransform);
        card.GetComponent<CardMover>().locked = true; // prevent this card from being dragged
        board[row][col - 1].setCard(card.GetComponent<CardView>());
    }
}
