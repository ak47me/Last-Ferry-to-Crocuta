
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI optionsText;


    public static WinManager instance;

    private bool dialogueIsPlaying;
    private bool WonCombat;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("this is more than one in the scene");
        }

        instance = this;
    }

    public static WinManager GetInstance()
    {
        return instance;
    }


    private void Update()
    {
        if (WonCombat && dialogueIsPlaying)
        {
            // Wait for player input while in combat dialogue mode
            if (Input.GetKeyDown(KeyCode.Y))  // Confirm
            {
                print("Y key is pressed");

           

                // Load the next level
                MainManager.Instance.NextLevel("MapScene");
            }
           
            
        }

    }


    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        WonCombat = false;
    }

    public void EnterDialogueMode(string s)
    {
        dialogueIsPlaying = true;
        WonCombat = true;
        dialoguePanel.SetActive(true);
        if (!string.IsNullOrEmpty(s))
        {
            dialogueText.text = s;
            optionsText.text = "";
        }
    }

}
