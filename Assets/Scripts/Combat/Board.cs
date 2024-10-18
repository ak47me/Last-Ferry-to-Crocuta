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
    private List<CardInfo> keyCards;
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
        // Wait until CombatSceneManager has the enemy and key cards ready
        while (CombatSceneManager.Instance == null ||
               CombatSceneManager.Instance.GetEnemyCards() == null ||
               CombatSceneManager.Instance.GetEnemyCards().Count == 0 ||
               CombatSceneManager.Instance.GetKeyCards() == null ||
               CombatSceneManager.Instance.GetKeyCards().Count == 0)
        {
            yield return null; // Wait for the next frame
        }

        // Get the enemy cards and key cards from CombatSceneManager
        enemyCards = CombatSceneManager.Instance.GetEnemyCards();
        keyCards = CombatSceneManager.Instance.GetKeyCards();

        // Now proceed with the board initialization
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
                    // Assign enemy cards using the enemyCards list
                    assignEnemy(i, j, pos.transform);
                }
                else if (i == 2)
                {
                    // Assign hero cards using the orderedKeyCards list
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
        if (keyCards != null && keyCards.Count >= col)
        {
            GameObject card = HandHandler.Instance.generateCard(keyCards[col-1], parentTransform);
            card.GetComponent<CardMover>().locked = true; // prevent this card from being dragged
            board[row][col-1].setCard(card.GetComponent<CardView>());

        }
        else
        {
            Debug.LogError("Not enough hand cards available in CombatSceneManager.");
        }
        
    }
}
