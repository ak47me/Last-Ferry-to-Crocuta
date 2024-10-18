
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class MapDialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI optionsText;
    public AudioSource audioSource;  // Reference to the AudioSource component
    public AudioClip confirmationClip; 
    public AudioClip declineClip; 

    private bool dialogueIsPlaying;
    private bool isInCombat;

    public static MapDialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("this is more than one in the scene");
        }

        instance = this;
    }

    public static MapDialogueManager GetInstance()
    {
        return instance;
    }

    public void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        isInCombat = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        optionsText.text = "";
    }

    private void Update()
    {
        if (isInCombat && dialogueIsPlaying)
        {
            // Wait for player input while in combat dialogue mode
            if (Input.GetKeyDown(KeyCode.Y))  // Confirm
            {
                print("Y key is pressed");

                // Play sound
                if (confirmationClip != null && audioSource != null)
                {
                    audioSource.PlayOneShot(confirmationClip);
                }

                // Load the next level
                MainManager.Instance.NextLevel("RevCombatScene");
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                // Play sound
                if (declineClip != null && audioSource != null)
                {
                    audioSource.PlayOneShot(declineClip);
                }

                ExitDialogueMode();
            }
        }

    }


    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        isInCombat = false;
    }

    public void EnterDialogueMode(string s)
    {
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        if (!string.IsNullOrEmpty(s))
        {
            dialogueText.text = s;
            optionsText.text = "";
        }
    }

    public void doStart()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        isInCombat = false;
    }
    public void EnterCombatDialogueMode(string s)
    {
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        isInCombat = true;
        if (!string.IsNullOrEmpty(s))
        {
            dialogueText.text = "Are you sure you want to continue to the Combat? ";
            optionsText.text = "Press y to confirm\nPress n otherwise";
        }


    }
}

