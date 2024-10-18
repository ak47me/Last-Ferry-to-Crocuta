using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardData cardData;
    public TMP_Text healthText;
    public TMP_Text attackText;
    public Image cardImage;

    private Vector3 originalScale;
    public float hoverScale = 1.2f;
    public float hoverDuration = 0.2f;
    public bool isHovered = false;
    public float displayDelay = 2f; // Time in seconds to wait before displaying the card
    public float hoverTime = 0f; // Timer to track hover duration
    public GameObject viewPanel; // Reference to the view_panel GameObject
    public viewCard viewCardComponent; // Reference to the viewCard component attached to view_panel
    public CardData hoveredCardData; // Reference to the hovered card's data



    void Start()
    {
        originalScale = transform.localScale;
        cardData.SetOriginalAttackPower(cardData.attackPower);
        if (cardData != null)
        {
            SetupCard(cardData); 
            // Use the new method to initialize display
        }
        else
        {
            Debug.LogWarning("CardData is not assigned in CardDisplay.");
        }
        //viewPanel.SetActive(false); // Initially hide the viewPanel
        if (viewPanel == null)
        {
            Debug.LogError("viewPanel is not assigned.");
        }
        else
        {
            viewPanel.SetActive(false); // Initially hide the viewPanel
        }

        viewCardComponent = viewPanel.GetComponent<viewCard>();


    }

    public void SetupCard(CardData card)
    {
        if (cardData != null)
        {
            // Unsubscribe from the previous card's events
            cardData.OnAttackPowerChanged -= UpdateAttackPowerDisplay;
            cardData.OnHealthPowerChanged -= updateHealthDisplay;
        }

        cardData = card;
        UpdateCardDisplay();

        // Subscribe to attack power and health change events for live updates
        if (cardData != null)
        {
            cardData.OnAttackPowerChanged += UpdateAttackPowerDisplay;
            cardData.OnHealthPowerChanged += updateHealthDisplay; // Subscribe here
        }
    }


    public void UpdateCardDisplay()
    {
        if (cardData != null)
        {
            healthText.text = cardData.cardHealth.ToString();
            attackText.text = cardData.attackPower.ToString();

            if (cardImage != null && cardData.cardImage != null)
            {
                cardImage.sprite = cardData.cardImage;
            }
        }
        else
        {
            Debug.LogError("CardData is null when updating display.");
        }
    }

    private void UpdateAttackPowerDisplay(int newAttackPower)
    {
        // Update the attack power text when notified
        attackText.text = newAttackPower.ToString();
    }

    public void updateHealthDisplay(int newHealth)
    {
        healthText.text = newHealth.ToString();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        hoveredCardData = cardData; // Set the current hovered card data
        //viewPanel.SetActive(true);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false; // Reset hover state
        hoverTime = 0f; // Reset hover time
        viewPanel.SetActive(false); // Hide the viewPanel when not hovering

    }

    void Update()
    {
        Vector3 targetScale = isHovered ? originalScale * hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime / hoverDuration);

        // Handle the hover timer
        if (isHovered)
        {
            hoverTime += Time.deltaTime; // Increment hover time while hovering

            // If hovered for 3 seconds, display card details
            if (hoverTime >= displayDelay)
            {
                if (hoveredCardData != null)
                {
                    Debug.Log("It's going inside");
                    DisplayCardDetails();
                }
            }
        }
        else
        {
            hoverTime = 0f; // Reset hover time if not hovering
        }
    }

    void DisplayCardDetails()
    {
        if (viewPanel != null && hoveredCardData != null)
        {
            Debug.Log("This is working");
            //viewPanel.SetActive(true); // Show the viewPanel
            viewCardComponent.SetupCard(hoveredCardData); // Setup card details in the viewPanel
        }
    }

    public void EndGame()
    {
        // Logic to determine if the game has ended

        cardData.ResetAttackPower();
    }
}
