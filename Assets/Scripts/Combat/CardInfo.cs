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

    [SerializeField] public Vector3 boardScale = new Vector3(1f, 1f, 1f);
    [SerializeField] public Vector3 handScale = new Vector3(1.3f, 1.3f, 1.3f);
}
