using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    // Reference to AudioManager
    public AudioManager audioManager;

    // Field to track the current map position
    public Vector2 currentMapPosition;

    // List of audio clips to be assigned to specific scenes
    public AudioClip MainSceneMusic;
    public AudioClip CombatSceneMusic;
    public AudioClip MapSceneMusic;
    public List<CardInfo> currentCombatCards = new List<CardInfo>();
    private Dictionary<Vector2, List<CardInfo>> nodeCardAssignments; // Dictionary to map node positions to specific enemy cards
    public List<CardInfo> handCards = new List<CardInfo>(); // New list for hand cards
    // Public fields to set loop start and end times for each scene in the Unity Inspector
    //public double MainSceneLoopStart = -1;
    //public double MainSceneLoopEnd = -1;
    public double CombatSceneLoopStart = -1;
    public double CombatSceneLoopEnd = -1;
    public double MapSceneLoopStart = -1;
    public double MapSceneLoopEnd = -1;
    // List to hold the enemy card prefabs
    public List<CardInfo> enemyCards = new List<CardInfo>();
    public List<CardInfo> keyCards = new List<CardInfo>();
    // Dictionary to store loop times for each scene
    private Dictionary<string, (double loopStart, double loopEnd)> sceneLoopTimes = new Dictionary<string, (double, double)>();
    private Dictionary<Vector2, List<CardInfo>> cardAssignments = new Dictionary<Vector2, List<CardInfo>>();
    private Dictionary<Vector2, List<CardInfo>> assignedCardsMap = new Dictionary<Vector2, List<CardInfo>>();


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize loop times and load enemy cards early in the game lifecycle
        InitializeLoopTimes();


        LoadEnemyCardPrefabs(); // Make sure this is called before the combat scene is initialized
        LoadKeyCards();
        LoadHandCards(); // Load hand cards



        // Audio manager setup
        if (audioManager == null)
        {
            GameObject audioManagerObject = new GameObject("AudioManager");
            audioManagerObject.transform.SetParent(this.transform); // Make it a child of MainManager
            audioManager = audioManagerObject.AddComponent<AudioManager>();

            // Initialize the AudioManager with scene-to-music mapping
            Dictionary<string, AudioClip> musicMap = new Dictionary<string, AudioClip>
        {
            { "Main", MainSceneMusic },
            { "RevCombatScene", CombatSceneMusic },
            { "MapScene", MapSceneMusic }
        };

            audioManager.Initialize(musicMap, sceneLoopTimes);
        }


        // Listen to the scene change event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void InitializeLoopTimes()
    {

        sceneLoopTimes["RevCombatScene"] = (CombatSceneLoopStart >= 0 ? CombatSceneLoopStart : 15.0,
                                         CombatSceneLoopEnd >= 0 ? CombatSceneLoopEnd : 45.0);
        sceneLoopTimes["MapScene"] = (MapSceneLoopStart >= 0 ? MapSceneLoopStart : 5.0,
                                      MapSceneLoopEnd >= 0 ? MapSceneLoopEnd : 40.0);
    }
    // Method to load enemy card prefabs from the specified path
    public void LoadEnemyCardPrefabs()
    {

        // Load all CardInfo assets from the specified path
        CardInfo[] loadedCards = Resources.LoadAll<CardInfo>("Cards/Enemies");
        Debug.Log("Attempting to load CardInfo assets from Resources/Cards/Enemies. Found: " + loadedCards.Length + " items.");

        foreach (CardInfo CardInfo in loadedCards)
        {
            if (CardInfo != null)
            {
                enemyCards.Add(CardInfo);
                Debug.Log("Loaded CardInfo asset: " + CardInfo.cardName);
            }
            else
            {
                Debug.LogWarning("Encountered a null CardInfo asset during loading.");
            }
        }

        Debug.Log("Loaded " + enemyCards.Count + " enemy card assets.");
    }

    public void LoadKeyCards()
    {
        CardInfo[] loadedKeyCards = Resources.LoadAll<CardInfo>("Cards/Characters");
        Debug.Log("Attempting to load CardInfo assets from Resources/Cards/Characters. Found: " + loadedKeyCards.Length + " items.");

        foreach (CardInfo cardInfo in loadedKeyCards)
        {
            if (cardInfo != null && cardInfo.health > 0) // Check if the card is alive
            {
                keyCards.Add(cardInfo);
                Debug.Log("Loaded Key Card: " + cardInfo.cardName + " with health: " + cardInfo.health);
            }
            else if (cardInfo != null)
            {
                Debug.LogWarning("Card " + cardInfo.cardName + " has zero health and won't be used.");
            }
        }

        Debug.Log("Total alive key cards loaded: " + keyCards.Count);
    }

    public List<CardInfo> SelectKeyCardsForCombat()
    {
        List<CardInfo> selectedCards = new List<CardInfo>();
        List<string> priorityCardNames = new List<string> { "Andrea", "Wes", "Sofia" };

        // Always try to select Andrea, Wes, and Sofia in the given order if they are alive
        foreach (string cardName in priorityCardNames)
        {
            CardInfo keyCard = keyCards.Find(card => card.cardName == cardName && card.health > 0);
            if (keyCard != null)
            {
                selectedCards.Add(keyCard);
            }
        }

        // If less than three cards were selected, fill up with other available cards
        while (selectedCards.Count < 3 && keyCards.Count > selectedCards.Count)
        {
            CardInfo randomCard = keyCards.Find(card => !selectedCards.Contains(card) && card.health > 0);
            if (randomCard != null)
            {
                selectedCards.Add(randomCard);
            }
        }

        Debug.Log("Selected key cards for combat in order: " + string.Join(", ", selectedCards.ConvertAll(card => card.cardName)));
        return selectedCards;
    }


    // Assign cards to a specific map position
    public void AssignCardsToPosition(Vector2 mapPosition, List<CardInfo> cards)
    {
        if (!assignedCardsMap.ContainsKey(mapPosition))
        {
            assignedCardsMap[mapPosition] = new List<CardInfo>(cards);
        }
    }


    // Check if cards are already assigned to a specific map position
    public bool AreCardsAssignedToPosition(Vector2 mapPosition)
    {
        return assignedCardsMap.ContainsKey(mapPosition) && assignedCardsMap[mapPosition].Count > 0;
    }
   

    // Get the assigned cards for a specific map position
    public List<CardInfo> GetAssignedCardsForPosition(Vector2 mapPosition)
    {
        if (assignedCardsMap.TryGetValue(mapPosition, out List<CardInfo> cards))
        {
            return cards;
        }
        return null;
    }


    public void LoadCombatCards(List<CardInfo> cards)
    {
        currentCombatCards.Clear();

        currentCombatCards.AddRange(cards);

        // Log the count of combat cards loaded
        Debug.Log("Loaded combat cards for the next battle: " + currentCombatCards.Count + " cards.");

        // Log the names of the cards being loaded
        foreach (CardInfo card in currentCombatCards)
        {
            if (card != null)
            {
                Debug.Log("Loaded combat card: " + card.cardName);
            }
            else
            {
                Debug.LogWarning("Encountered a null CardInfo asset in combat cards.");
            }
        }
    }


    private void LoadHandCards()
    {
        // Load all CardInfo assets for hand cards
        CardInfo[] loadedHandCards = Resources.LoadAll<CardInfo>("Cards/Hand Cards");
        Debug.Log("Attempting to load CardInfo assets from Resources/Cards/Hand Cards. Found: " + loadedHandCards.Length + " items.");

        foreach (CardInfo cardInfo in loadedHandCards)
        {
            if (cardInfo != null)
            {
                handCards.Add(cardInfo);
                Debug.Log("Loaded Hand Card: " + cardInfo.cardName);
            }
            else
            {
                Debug.LogWarning("Encountered a null CardInfo asset during loading hand cards.");
            }
        }

        Debug.Log("Total hand cards loaded: " + handCards.Count);

        // Update HandHandler with loaded hand cards
        if (HandHandler.Instance != null)
        {
            Debug.Log("There are Hand cards present");
            HandHandler.Instance.cardData = handCards; // Assign loaded cards to HandHandler
            HandHandler.Instance.AddStartingCards(); // Add cards to hand
        }
        else
        {
            Debug.LogError("HandHandler instance is null! Please ensure HandHandler is initialized properly.");
        }
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Get the name of the current scene and tell the AudioManager to play the appropriate music
        string sceneName = scene.name;
        audioManager.PlayMusicForScene(sceneName);


        if (DialogueManager.Instance != null)
        {
            Debug.Log("dialogue manager is present in this scene " + sceneName);
            ProductionSystem.Instance.ResetSystem();
            DialogueManager.Instance.FindUIElements();
            DialogueManager.Instance.StartDialogueForScene(sceneName);
        }
    }

    // Method to update the current map position
    public void SetMapPosition(Vector2 position)
    {
        currentMapPosition = position;
        Debug.Log("Current map position set to: " + currentMapPosition);
    }

    // Method to get the current map position
    public Vector2 GetMapPosition()
    {
        return currentMapPosition;
    }



}
