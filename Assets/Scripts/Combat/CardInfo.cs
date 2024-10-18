using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Info")]
public class CardInfo : ScriptableObject
{
    // Card types that will exist
    public enum cardType
    {
        KeyCard,
        HandCard,
        Enemy
    }

    public enum effectType
    {
        OnFight,
        OnEnd,
        Ongoing,
        OnDeath,
        OnHit,
        None
    }

    public enum subType
    {
        None,
        Helper,
        Item
    }

    // Numbers Info
    public int maxHealth;
    public int health;
    public int def;
    public int baseAtk;
    public int atk;

    // Card Info
    public string cardName;
    public Sprite charSpr;
    public Sprite suitSpr;
    public string effectText;
    public cardType type;
    public effectType effect;
    public subType playType;
    public int usesPerTurn = 1;
    public int usesPerCombat = 1;

    [SerializeField] public Vector3 boardScale = new Vector3(1f, 1f, 1f);
    [SerializeField] public Vector3 handScale = new Vector3(1.3f, 1.3f, 1.3f);

    // Some absolutely insane effect code
    // Idea: store a list of board functions and the cards on board you want them to effect.
    // You pass this information to the board, and the board executes this code.
    [SerializeField] public List<string> effectList;
    [SerializeField] public List<int> targets;
}
