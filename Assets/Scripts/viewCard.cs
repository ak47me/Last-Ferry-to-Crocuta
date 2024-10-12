using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;

public class viewCard : MonoBehaviour
{
    public CardData currentCard;
    public TMP_Text name;
    public TMP_Text info;
    public Image cardImage;


    public void SetupCard(CardData currentCard)
    {
        cardImage.sprite = currentCard.cardImage;
        name.text = currentCard.cardName.ToString();
        info.text = currentCard.cardInfo.ToString();


    }


}
