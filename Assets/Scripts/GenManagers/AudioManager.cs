using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    // Dictionary to map scene names to their respective audio clips
    private Dictionary<string, AudioClip> sceneMusicMap = new Dictionary<string, AudioClip>();

    // Dictionary to store loop times for each scene
    private Dictionary<string, (double loopStart, double loopEnd)> sceneLoopTimes = new Dictionary<string, (double, double)>();

    private int loopStartSamples;
    private int loopEndSamples;
    private int loopLengthSamples;

    private void Awake()
    {
        // Initialize the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Method to initialize the scene-to-music mapping and loop times (called by the MainManager)
    public void Initialize(Dictionary<string, AudioClip> musicMap, Dictionary<string, (double, double)> loopTimes)
    {
        sceneMusicMap = musicMap;
        sceneLoopTimes = loopTimes;
    }

    // Method to play the music for the current scene and set loop points
    public void PlayMusicForScene(string sceneName)
    {
        if (sceneMusicMap.TryGetValue(sceneName, out AudioClip sceneMusic))
        {
            // Check if the audio clip is already playing
            if (audioSource.clip == sceneMusic && audioSource.isPlaying) return;

            // Set the audio clip and play it
            audioSource.clip = sceneMusic;
            audioSource.Play();

            // Set loop points based on the loop times for the current scene
            if (sceneLoopTimes.TryGetValue(sceneName, out (double loopStart, double loopEnd) loopInfo))
            {
                SetLoopPoints(loopInfo.loopStart, loopInfo.loopEnd);
            }
            else
            {
                // Default to no loop if times aren't specified
                loopStartSamples = 0;
                loopEndSamples = (int)(sceneMusic.length * sceneMusic.frequency);
                loopLengthSamples = loopEndSamples; // Loop entire clip
            }
        }
        else
        {
            Debug.LogWarning($"No music found for the scene: {sceneName}");
        }
    }

    private void SetLoopPoints(double loopStart, double loopEnd)
    {
        // Convert loop times to sample points
        loopStartSamples = (int)(loopStart * audioSource.clip.frequency);
        loopEndSamples = (int)(loopEnd * audioSource.clip.frequency);
        loopLengthSamples = loopEndSamples - loopStartSamples;
    }

    private void Update()
    {
        // Loop the audio if the current playback position exceeds the loop end
        if (audioSource.isPlaying && audioSource.timeSamples >= loopEndSamples)
        {
<<<<<<< HEAD
            
=======

>>>>>>> 60dced96e2b7f117641b1a46bf659893b04b909c
            audioSource.timeSamples = loopStartSamples; // Reset to loop start
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
