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
    public int[] health = { 10, 10, 15 };
    public int[] attack = { 4, 5, 7 };

    private float hoverDuration = 3f; // The time required to hover for card details display
    private float[] hoverTimers;      // Array to track how long each card is hovered
    private CardDisplay[] cardDisplays; // Array to store all CardDisplay components

    public GameObject viewPanel; // Reference to the view_panel GameObject
    private viewCard viewCardComponent; // Reference to the viewCard component

    void Start()
    {
        CardData[] characters = { startingCards[3], startingCards[4], startingCards[5] };

        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].cardHealth = health[i];
            characters[i].attackPower = attack[i];
        }

        CreateCardFan();

        // Initialize hover timers
        hoverTimers = new float[startingCards.Length];

        // Initialize viewCard component from the viewPanel
        if (viewPanel != null)
        {
            viewCardComponent = viewPanel.GetComponent<viewCard>();
            viewPanel.SetActive(false); // Initially hide the view panel
        }
        else
        {
            Debug.LogError("viewPanel is not assigned.");
        }
    }

    void CreateCardFan()
    {
        int totalCards = startingCards.Length;
        float startAngle = -fanAngle / 2;   // Start from the leftmost angle of the fan
        float angleStep = fanAngle / (totalCards - 1);  // The step between each card's angle

        cardDisplays = new CardDisplay[totalCards]; // Array to store card displays

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

            // Store the CardDisplay in the array
            cardDisplays[i] = cardDisplay;
        }
    }

    void Update()
    {
        bool anyCardHovered = false;  // Track if any card is currently hovered

        for (int i = 0; i < cardDisplays.Length; i++)
        {
            CardDisplay cardDisplay = cardDisplays[i];

            // Check if the card is hovered
            if (cardDisplay.isHovered)
            {
                // Increment hover timer for this card
                hoverTimers[i] += Time.deltaTime;

                // If hovered for more than hoverDuration, show the details
                if (hoverTimers[i] >= hoverDuration)
                {
                    DisplayCardDetails(cardDisplay.cardData); // Show card details in the view panel
                    anyCardHovered = true;  // Mark that a card is hovered
                }
            }
            else
            {
                // Reset hover timer if not hovering
                hoverTimers[i] = 0f;
            }
        }

        // If no card is hovered, deactivate the view panel
        if (!anyCardHovered && viewPanel.activeSelf)
        {
            viewPanel.SetActive(false);
        }
    }

    // Method to display card details in the view panel
    void DisplayCardDetails(CardData cardData)
    {
        if (viewPanel != null && viewCardComponent != null && cardData != null)
        {
            if (!viewPanel.activeSelf)  // Only activate if it's not already active
            {
                viewPanel.SetActive(true); // Show the view panel
            }

            viewCardComponent.SetupCard(cardData); // Setup card details in the view panel
        }
    }

}
