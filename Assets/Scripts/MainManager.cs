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

    // Public fields to set loop start and end times for each scene in the Unity Inspector
    public double MainSceneLoopStart = -1;
    public double MainSceneLoopEnd = -1;
    public double CombatSceneLoopStart = -1;
    public double CombatSceneLoopEnd = -1;
    public double MapSceneLoopStart = -1;
    public double MapSceneLoopEnd = -1;

    // Dictionary to store loop times for each scene
    private Dictionary<string, (double loopStart, double loopEnd)> sceneLoopTimes = new Dictionary<string, (double, double)>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize loop times
        InitializeLoopTimes();

        // Check if the AudioManager is already attached; if not, add it
        if (audioManager == null)
        {
            GameObject audioManagerObject = new GameObject("AudioManager");
            audioManagerObject.transform.SetParent(this.transform); // Make it a child of MainManager
            audioManager = audioManagerObject.AddComponent<AudioManager>();

            // Initialize the AudioManager with scene-to-music mapping
            Dictionary<string, AudioClip> musicMap = new Dictionary<string, AudioClip>
            {
                { "Main", MainSceneMusic },
                { "CombatScene", CombatSceneMusic },
                { "MapScene", MapSceneMusic }
                // Add more scenes and their respective music as needed
            };

            audioManager.Initialize(musicMap, sceneLoopTimes);
        }

        // Listen to the scene change event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void InitializeLoopTimes()
    {
        // Set loop times for each scene based on Inspector values or defaults
        sceneLoopTimes["Main"] = (MainSceneLoopStart >= 0 ? MainSceneLoopStart : 10.0,
                                  MainSceneLoopEnd >= 0 ? MainSceneLoopEnd : 30.0);
        sceneLoopTimes["CombatScene"] = (CombatSceneLoopStart >= 0 ? CombatSceneLoopStart : 15.0,
                                         CombatSceneLoopEnd >= 0 ? CombatSceneLoopEnd : 45.0);
        sceneLoopTimes["MapScene"] = (MapSceneLoopStart >= 0 ? MapSceneLoopStart : 5.0,
                                      MapSceneLoopEnd >= 0 ? MapSceneLoopEnd : 25.0);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Get the name of the current scene and tell the AudioManager to play the appropriate music
        string sceneName = scene.name;
        audioManager.PlayMusicForScene(sceneName);

        
        if (DialogueManager.Instance != null)
        {
            Debug.Log("dialogue manager is present in this scene "+ sceneName);
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
