using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    private List<CardInfo> currentEnemyCards;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Method to initialize combat with the assigned cards for the current node
    public void InitializeCombat(List<CardInfo> enemyCards)
    {
        currentEnemyCards = enemyCards;

        // Use currentEnemyCards to set up the combat scene
        // Example: display cards on the battlefield or initialize enemy actions
        SetupCombatScene();
    }

    private void SetupCombatScene()
    {
        // Logic to place cards on the battlefield, initialize enemies, etc.
        // This logic should use currentEnemyCards to determine the enemies in the combat encounter

        foreach (CardInfo card in currentEnemyCards)
        {
            Debug.Log("Enemy Card: " + card.name); // Replace with logic to place the card in the combat scene
        }

        // Additional setup code...
    }
}
