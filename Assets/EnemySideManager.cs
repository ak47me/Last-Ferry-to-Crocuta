using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySideManager : MonoBehaviour
{
    public RectTransform handTransform;   // Reference to the Hand Panel (should be a RectTransform of the Canvas)
    public GameObject cardPrefab;         // The card prefab to instantiate
    public CardData[] startingCards;      // Array of starting cards
    public float cardSpacing = 150f;      // Spacing between the cards

    void Start()
    {
        CreateCardGrid();
    }

    void CreateCardGrid()
    {
        int totalCards = startingCards.Length;

        if (totalCards != 3)
        {
            Debug.LogWarning("Enemy hand must always have 3 cards.");
            return;
        }

        // Get the width of the hand panel
        float handWidth = handTransform.rect.width;

        // Calculate the starting X position, so cards are centered
        float startX = -(handWidth / 2) + cardSpacing;

        for (int i = 0; i < totalCards; i++)
        {
            // Instantiate the card and make it a child of the Hand
            GameObject cardInstance = Instantiate(cardPrefab, handTransform);

            // Reset the scale to ensure it fits properly inside the Hand
            RectTransform cardRectTransform = cardInstance.GetComponent<RectTransform>();
            cardRectTransform.localScale = Vector3.one;

            // Set the card's position relative to the hand panel
            float cardXPosition = startX + (i * cardSpacing);

            // Set anchored position relative to the parent (Hand Panel)
            cardRectTransform.anchoredPosition = new Vector2(cardXPosition, 0);  // Evenly spaced in X-axis

            // No need to rotate the cards as they're in a grid

            // Get the CardDisplay component from the instantiated card
            CardDisplay cardDisplay = cardInstance.GetComponent<CardDisplay>();

            // Call SetupCard to pass the CardData to the CardDisplay
            cardDisplay.SetupCard(startingCards[i]);
        }
    }
}
