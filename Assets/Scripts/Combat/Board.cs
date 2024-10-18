using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private static Board _instance;
    public static Board Instance { get { return _instance; } }
    public List<List<BoardPosition>> board = new List<List<BoardPosition>>();
    public GameObject uiCanvas;
    public List<CardInfo> startingCards;

    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
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
                    assignEnemy(i, j, pos.transform);
                }
                else if (i == 2)
                {
                    assignHero(i, j, pos.transform);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void removeCard(int row, int col)
    {
        board[row][col].clearCard();
    }

    // FIXME: assign the enemy saved from Map here instead of one from the starting cards list
    // (or just make the starting cards list the cards you saved)
    public void assignEnemy(int row, int col, Transform parentTransform)
    {
        GameObject card = HandHandler.Instance.generateCard(startingCards[col-1], parentTransform);
        card.GetComponent<CardMover>().locked = true; // prevent this card from being dragged
        board[row][col-1].setCard(card.GetComponent<CardView>());
    }

    // FIXME: assign the hero (enzo, claudio, sofia, etc.) saved from Map here instead of one from the starting cards list
    // (or just make the starting cards list the cards you saved)
    public void assignHero(int row, int col, Transform parentTransform)
    {
        GameObject card = HandHandler.Instance.generateCard(startingCards[2 + col], parentTransform);
        card.GetComponent<CardMover>().locked = true; // prevent this card from being dragged
        board[row][col-1].setCard(card.GetComponent<CardView>());
    }
}
