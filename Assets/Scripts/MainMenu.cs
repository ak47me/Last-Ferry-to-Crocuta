using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource;  // Reference to the AudioSource
    public AudioClip buttonClickClip;  // The sound clip to play on button click
    public void PlayGame()
    {
        PlayButtonSound();

        MainManager.Instance.NextLevel("MapScene");

    }

    public void Tutorial()
    {
        PlayButtonSound();

        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        PlayButtonSound();

        Application.Quit(); 
    }

    private void PlayButtonSound()
    {
        if (audioSource != null && buttonClickClip != null)
        {
            audioSource.PlayOneShot(buttonClickClip);
        }
    }
}
