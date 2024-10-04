using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public int _attackPower;// Backing field for attackPower
    public int _health;
    public Sprite cardImage;
    public string ability;
    public int posX;
    public int posY;

    // Original attack power for resetting
    private int _originalAttackPower;

    // Event to notify when attackPower changes
    public event Action<int> OnAttackPowerChanged;
    public event Action<int> OnHealthPowerChanged;

    public int attackPower
    {
        get { return _attackPower; }
        set
        {
            if (_attackPower != value)
            {
                _attackPower = value;
                OnAttackPowerChanged?.Invoke(_attackPower); // Invoke event when value changes
            }
        }
    }
    public int cardHealth
    {
        get { return _health; }
        set
        {
            if (_health != value)
            {
                _health = value;
                OnHealthPowerChanged?.Invoke(_health); // Invoke event when value changes
            }
        }

    }

    public void SetOriginalAttackPower(int value)
    {
        _originalAttackPower = value;
    }

    public void ResetAttackPower()
    {
        attackPower = _originalAttackPower; // Resets the attack power to the original value
    }

    public enum CardType
    {
        Enemy,
        Resource,
        Character
    }

    public CardType cardType;
}
