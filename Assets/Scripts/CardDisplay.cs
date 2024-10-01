using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData;
    public TMP_Text healthText;  // TMP text for health
    public TMP_Text attackText;  // TMP text for attack
    public Image cardImage;      // Reference to the Image component for the card

    public void SetupCard(CardData card)
    {
        cardData = card;
        healthText.text = card.cardHealth.ToString();
        attackText.text = card.attackPower.ToString();

        // Set the card image
        if (cardImage != null)
        {
            cardImage.sprite = card.cardImage; // Set the sprite from CardData
        }
    }
}
