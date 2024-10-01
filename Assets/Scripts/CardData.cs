using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public int cardHealth;  // Change to int
    public int attackPower;  // Change to int
    public Sprite cardImage; // Add a reference for the card image
}
