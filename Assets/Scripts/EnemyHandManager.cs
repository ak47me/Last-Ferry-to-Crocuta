using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject cardPrefab;         // The card prefab to instantiate
    public CardData[] startingCards;      // Array of starting cards
    public Transform enemyGrid;           // Reference to the enemy grid that holds slots

    private Transform[] enemyGridSlots;   // Array to store enemy grid slots

    void Start()
    {
        // Populate slots from the enemy grid
        PopulateEnemyGridSlots();

        // Ensure there are matching slots and cards
        if (enemyGridSlots.Length == startingCards.Length)
        {
            DealEnemyCards();
        }
        else
        {
            Debug.LogError("Mismatch between number of grid slots and cards.");
        }
    }

    // Fetch and store all grid slots as children of the enemyGrid
    private void PopulateEnemyGridSlots()
    {
        enemyGridSlots = new Transform[enemyGrid.childCount];

        for (int i = 0; i < enemyGrid.childCount; i++)
        {
            enemyGridSlots[i] = enemyGrid.GetChild(i);  // Get each slot as a child of enemyGrid
        }
    }

    // Deal cards into the grid slots and match their sizes
    private void DealEnemyCards()
    {
        for (int i = 0; i < startingCards.Length; i++)
        {
            // Instantiate a card and make it a child of the corresponding grid slot
            GameObject cardInstance = Instantiate(cardPrefab, enemyGridSlots[i]);

            // Get RectTransforms for the card and grid slot
            RectTransform cardRectTransform = cardInstance.GetComponent<RectTransform>();
            RectTransform gridSlotRectTransform = enemyGridSlots[i].GetComponent<RectTransform>();

            // Match the card's size to the grid slot size
            cardRectTransform.anchorMin = Vector2.zero;
            cardRectTransform.anchorMax = Vector2.one;
            cardRectTransform.sizeDelta = Vector2.zero;  // Reset size to match slot exactly
            cardRectTransform.localScale = Vector3.one;  // Ensure the card is scaled properly

            // Set up the card data
            CardDisplay cardDisplay = cardInstance.GetComponent<CardDisplay>();
            if (cardDisplay != null)
            {
                cardDisplay.SetupCard(startingCards[i]);
            }
        }
    }
}
