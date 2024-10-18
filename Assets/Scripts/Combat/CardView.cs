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

    public int atkV;
    public int hpV;
    public int defV;
    public int tempAtk = 0;
    public int tempdef = 0;

    public int effectUsesC = 1;
    public int effectUsesT = 1;
    public bool canEffect = true;
    public bool canAtk = true;
    public int turnsWoAtk = 0;
    public int turnsWoEff = 0;

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

        atkV = cardInfo.atk;
        hpV = cardInfo.health;
        defV = cardInfo.def;
        effectUsesT = info.usesPerTurn;
        effectUsesC = info.usesPerCombat;

        health.text = (hpV.ToString() + " / " + cardInfo.maxHealth.ToString());
        atk.text = atkV.ToString();
        def.text = defV.ToString();
        cardArt.sprite = cardInfo.charSpr;
        suitArt.sprite = cardInfo.suitSpr;

        if (cardInfo.type == CardInfo.cardType.HandCard)
        {
            atk.gameObject.SetActive(false);
        }
    }

    public void updateDisplayData()
    {
        health.text = (hpV.ToString() + " / " + cardInfo.maxHealth.ToString());
        atk.text = atkV.ToString();
        def.text = defV.ToString();

        if (defV > 0)
        {
            def.gameObject.SetActive(true);
        }
    }

    public void setStats(int healthChange, int atkChange, int defChange)
    {
        hpV = Mathf.Clamp(hpV + healthChange, 0, cardInfo.maxHealth);

        checkDeath();

        atkV = Mathf.Max(atkV + atkChange, 0);
        defV = Mathf.Max(defV + defChange, 0);

        updateDisplayData();
    }

    public void resetAtk()
    {
        if (cardInfo.type != CardInfo.cardType.Enemy || atkV > cardInfo.baseAtk) atkV = cardInfo.baseAtk;

        updateDisplayData();
    }

    public void resetHealth()
    {
        hpV = cardInfo.maxHealth;

        updateDisplayData();
    }

    public void resetDef()
    {
        defV = cardInfo.def;

        updateDisplayData();
    }

    public void resetUses()
    {
        effectUsesT = cardInfo.usesPerTurn;
    }

    public void checkDeath()
    {
        if (hpV <= 0)
        {
            if (cardInfo.effect == CardInfo.effectType.OnDeath && canEffect) Board.Instance.doEffect(boardRow, boardCol, cardInfo);
            else if (!canEffect) incrementTWE();
            Board.Instance.removeCard(boardRow, boardCol);
            Destroy(gameObject);
        }
    }

    public void handleAtk(int dmg)
    {
        defV -= dmg;
        
        if (defV < 0)
        {
            hpV += defV;
            defV = 0;
        }

        if (cardInfo.effect == CardInfo.effectType.OnHit && canEffect) Board.Instance.doEffect(boardRow, boardCol, cardInfo);
        else if (!canEffect) incrementTWE();

        checkDeath();

        updateDisplayData();
    }

    public void incrementTWA()
    {
        turnsWoAtk = Mathf.Max(turnsWoAtk - 1, 0);

        if (turnsWoAtk <= 0)
        {
            canAtk = true;
        }
    }

    public void incrementTWE()
    {
        turnsWoEff = Mathf.Max(turnsWoEff - 1, 0);

        if (turnsWoEff <= 0)
        {
            canEffect = true;
        }
    }
}
