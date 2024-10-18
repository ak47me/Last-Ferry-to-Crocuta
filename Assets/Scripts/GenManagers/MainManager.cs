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

    // Public fields to set loop start and end times for each scene in the Unity Inspector
    //public double MainSceneLoopStart = -1;
    //public double MainSceneLoopEnd = -1;
    public double CombatSceneLoopStart = -1;
    public double CombatSceneLoopEnd = -1;
    public double MapSceneLoopStart = -1;
    public double MapSceneLoopEnd = -1;
    // List to hold the enemy card prefabs
    public List<CardInfo> enemyCards = new List<CardInfo>();
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
