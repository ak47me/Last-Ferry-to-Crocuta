using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public AudioClip mainSceneMusic;
    public AudioClip combatSceneMusic;
    public AudioClip mapSceneMusic;

    private AudioSource audioSource;

    void Awake()
    {
        // Ensure only one instance of the AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate AudioManager objects
            return;
        }

        audioSource = GetComponent<AudioSource>();

        // If there's no AudioSource component, add it dynamically
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        // Subscribe to scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ensure the audio source is available after scene load
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        }

        // Switch music based on the scene name
        switch (scene.name)
        {
            case "Main":
                PlayMusic(mainSceneMusic);
                break;
            case "CombatScene":
                PlayMusic(combatSceneMusic);
                break;
            case "MapScene":
                PlayMusic(mapSceneMusic);
                break;
            default:
                Debug.LogWarning("No music specified for this scene!");
                break;
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing!");
            return;
        }

        // Avoid restarting the same music
        if (audioSource.clip == clip) return;

        audioSource.clip = clip;
        audioSource.loop = true;  // Ensure the music loops
        audioSource.Play();
    }
}
