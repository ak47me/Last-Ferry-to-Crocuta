using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    public RectTransform handTransform;   // Reference to the Hand Panel (should be a RectTransform of the Canvas)
    public GameObject cardPrefab;         // The card prefab to instantiate
    public CardData[] startingCards;      // Array of starting cards
    public float radius = 150f;           // Radius of the fan arc
    public float fanAngle = 30f;          // Maximum angle for the fan

    void Start()
    {

        CreateCardFan();
    }

    void CreateCardFan()
    {
        int totalCards = startingCards.Length;
        float startAngle = -fanAngle / 2;   // Start from the leftmost angle of the fan
        float angleStep = fanAngle / (totalCards - 1);  // The step between each card's angle

        for (int i = 0; i < totalCards; i++)
        {
            // Instantiate the card and make it a child of the Hand
            GameObject cardInstance = Instantiate(cardPrefab, handTransform);

            // Reset the scale to ensure it fits properly inside the Hand
            RectTransform cardRectTransform = cardInstance.GetComponent<RectTransform>();
            cardRectTransform.localScale = Vector3.one;

            // Calculate the card's position in the arc (circular distribution)
            float angle = startAngle + i * angleStep;
            float radians = angle * Mathf.Deg2Rad;

            // Set the card position relative to the center of the hand
            Vector3 cardPosition = new Vector3(Mathf.Sin(radians) * radius, 0, 0); // Fan out horizontally

            // Set anchored position relative to the parent (Hand Panel)
            cardRectTransform.anchoredPosition = cardPosition;

            // Rotate the card to face outward, creating the fan effect
            cardRectTransform.localRotation = Quaternion.Euler(0, 0, -angle);

            // Get the CardDisplay component from the instantiated card
            CardDisplay cardDisplay = cardInstance.GetComponent<CardDisplay>();

            // Call SetupCard to pass the CardData to the CardDisplay
            cardDisplay.SetupCard(startingCards[i]);
        }
    }
}
