using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public Transform handTransform; // Reference to the Hand object (should be a child of Canvas)
    public GameObject cardPrefab;   // The card prefab to instantiate
    public CardData[] startingCards; // Array of starting cards

    void Start()
    {
        foreach (var card in startingCards)
        {
            // Instantiate the card and make it a child of the Hand
            GameObject cardInstance = Instantiate(cardPrefab, handTransform);

            // Reset the scale to ensure it fits properly inside the Hand
            RectTransform cardRectTransform = cardInstance.GetComponent<RectTransform>();
            cardRectTransform.localScale = Vector3.one;

            // Get the CardDisplay component from the instantiated card
            CardDisplay cardDisplay = cardInstance.GetComponent<CardDisplay>();

            // Call SetupCard to pass the CardData to the CardDisplay
            cardDisplay.SetupCard(card);
        }
    }
}
