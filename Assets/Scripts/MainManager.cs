using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    // Reference to AudioManager
    public AudioManager audioManager;

    // Field to track the current map position
    public Vector2 currentMapPosition;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Check if the AudioManager is already attached; if not, add it
        if (audioManager == null)
        {
            // when first initialized, MainManager will create a child manager name AudioManager which will have AudioManager script attached to it which takes care of funcitonality of playing music accross different scenes
            //so the functionality of playing music across different scenes will still be taken care by AudioManager but instantiation and persistence are now controlled by MainManager

            GameObject audioManagerObject = new GameObject("AudioManager");
            audioManagerObject.transform.SetParent(this.transform); // Make it a child of MainManager
            audioManager = audioManagerObject.AddComponent<AudioManager>();
            audioManager.Initialize(); // Initialize the AudioManager with scene handling


        } // we can now call AudioManager in anyother script as follows : MainManager.Instance.audioManager.PlayMusic(someAudioClip);

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
