using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardData cardData;
    public TMP_Text healthText;  // TMP text for health
    public TMP_Text attackText;  // TMP text for attack
    public Image cardImage;  
   // Reference to the Image component for the card

    private Vector3 originalScale;  // To store the card's original scale
    public float hoverScale = 1.2f; // Scale factor for hover effect
    public float hoverDuration = 0.2f; // Duration of the hover effect
    private bool isHovered = false;  // Track hover state

    void Start()
    {
        // Store the original scale of the card at the start
        originalScale = transform.localScale;

        // Initialize card data
        if (cardData != null)
        {
            SetupCard(cardData);
        }
    }

    public void SetupCard(CardData card)
    {
        cardData = card;

        // Set health and attack text
        healthText.text = card.cardHealth.ToString();
        attackText.text = card.attackPower.ToString();

        // Set card image
        if (cardImage != null && card.cardImage != null)
        {
            cardImage.sprite = card.cardImage;
        }
    }

    // This method is called when the mouse enters the card area
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true; // Set hover state to true
    }

    // This method is called when the mouse exits the card area
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false; // Set hover state to false
    }

    void Update()
    {
        // Update the scale based on hover state
        Vector3 targetScale = isHovered ? originalScale * hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime / hoverDuration);

    }
}
