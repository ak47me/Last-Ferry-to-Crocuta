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
    private bool isHovered = false;

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
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    void Update()
    {
        Vector3 targetScale = isHovered ? originalScale * hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime / hoverDuration);
    }
    public void EndGame()
    {
        // Logic to determine if the game has ended

        cardData.ResetAttackPower();
    }
}
