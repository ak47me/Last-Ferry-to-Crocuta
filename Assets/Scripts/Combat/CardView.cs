using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public CardInfo cardInfo;

    public Image cardArt;
    public Image suitArt;

    public TMP_Text atk;
    public TMP_Text def;
    public TMP_Text health;

    public int boardRow;
    public int boardCol;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setUpCard(CardInfo info)
    {
        cardInfo = info;
        health.text = (cardInfo.health.ToString() + " / " + cardInfo.maxHealth.ToString());
        atk.text = cardInfo.atk.ToString();
        def.text = cardInfo.def.ToString();
        cardArt.sprite = cardInfo.charSpr;
        suitArt.sprite = cardInfo.suitSpr;

        if (cardInfo.type == CardInfo.cardType.HandCard)
        {
            atk.gameObject.SetActive(false);
        }
    }

    public void updateDisplayData()
    {
        health.text = (cardInfo.health.ToString() + " / " + cardInfo.maxHealth.ToString());
        atk.text = cardInfo.atk.ToString();
        def.text = cardInfo.def.ToString();
    }
}
