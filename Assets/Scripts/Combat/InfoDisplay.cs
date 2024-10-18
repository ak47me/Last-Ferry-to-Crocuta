using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoDisplay : MonoBehaviour
{
    private static InfoDisplay _instance;
    public static InfoDisplay Instance { get { return _instance; } }

    public TMP_Text cardName;
    public TMP_Text health;
    public TMP_Text atkDef;
    public TMP_Text effect;
    public Image cardImage;
    public Image suitImage;
    public GameObject background;

    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setInfo(CardView card)
    {
        setHealth(card.cardInfo.maxHealth, card.hpV);
        if (card.cardInfo.type != CardInfo.cardType.HandCard) setAtkDef(card.atkV, card.defV); // Do not set ATK/DEF for a helper/item
        else clear(atkDef);
        setName(card.cardInfo.cardName);
        setEffect(card.cardInfo.effectText);
        setImages(card.cardInfo.charSpr, card.cardInfo.suitSpr);
    }

    public void setHealth(int max, int current)
    {
        health.text = "Health\n" + current.ToString() + "/" + max.ToString();
    }

    public void setAtkDef(int atk, int def)
    {
        atkDef.text = "Attack/Defense\n" + atk.ToString() + "/" + def.ToString();
    }

    public void setName(string cardName)
    {
        this.cardName.text = cardName;
    }

    public void setEffect(string effect)
    {
        this.effect.text = effect;
    }

    public void setImages(Sprite card, Sprite suit)
    {
        cardImage.sprite = card;
        suitImage.sprite = suit;
    }

    public void enable()
    {
        background.SetActive(true);
    }

    public void disable()
    {
        background.SetActive(false);
    }

    public void clear(TMP_Text textMesh)
    {
        textMesh.text = "";
    }
}
