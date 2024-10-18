using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneManager : MonoBehaviour
{
    private static CombatSceneManager _instance;
    public static CombatSceneManager Instance { get { return _instance; } }

    private List<CardInfo> enemyCards = new List<CardInfo>();
    private List<CardInfo> keyCards = new List<CardInfo>();

    void Awake()
    {
        // Ensure that there is only one instance of CombatSceneManager (Singleton Pattern)
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Make CombatSceneManager persistent across scenes
        }
        else
        {
            Debug.LogWarning("Multiple instances of CombatSceneManager detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }


    void Start()
    {
        if (MainManager.Instance != null)
        {
            Vector2 mapPosition = MainManager.Instance.GetMapPosition();
            Debug.Log("Player's map position on entering Combat Scene: " + mapPosition);

            // Retrieve the assigned cards for the node if they exist
            enemyCards = MainManager.Instance.GetAssignedCardsForPosition(mapPosition);
            keyCards = MainManager.Instance.SelectKeyCardsForCombat();
            if (enemyCards != null && enemyCards.Count > 0 && keyCards!=null && keyCards.Count>0)
            {
                Debug.Log("Using previously assigned cards for combat at position: " + mapPosition);
                // Display or use these cards in the combat scene
                Debug.Log("Loaded combat enemy cards for the battle in combat scene manager: " + enemyCards.Count + " cards.");
                Debug.Log("Loaded combat key cards for the battle from Main in combat scene manager"+ keyCards.Count + "cards.");
                foreach (CardInfo card in enemyCards)
                {
                    if (card != null)
                    {
                        Debug.Log("Loaded combat card: " + card.cardName);
                    }
                }
                foreach (CardInfo card in keyCards)
                {
                    if (card != null)
                    {
                        Debug.Log("Loaded combat key card: " + card.cardName);
                    }
                } 
            }
            else
            {
                Debug.LogError("No cards were assigned to this position.");
            }
        }
        else
        {
            Debug.LogError("MainManager instance is missing.");
        }
    }

    // Method to expose enemy cards to the Board class
    public List<CardInfo> GetEnemyCards()
    {
        return enemyCards;
    }
    public List<CardInfo> GetKeyCards()
    {
        return keyCards;
    }
}
