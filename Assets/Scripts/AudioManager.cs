using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioClip mainSceneMusic;
    public AudioClip combatSceneMusic;
    public AudioClip mapSceneMusic;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // If there's no AudioSource component, add it dynamically
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Initialize()
    {
        // Subscribe to scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Call OnSceneLoaded manually for the initial scene
        Scene currentScene = SceneManager.GetActiveScene();
        OnSceneLoaded(currentScene, LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
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
