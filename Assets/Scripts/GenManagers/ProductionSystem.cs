using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProductionSystem : MonoBehaviour
{
    private static ProductionSystem _instance;
    public static ProductionSystem Instance { get { return _instance; } }

    private bool hasGiven = false;

    void Awake()
    {
        // Ensure that there is only one instance of the ProductionSystem
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // Only execute this block once when the dialogue hasn't been given
        if (!hasGiven)
        {
            hasGiven = true;
            CheckAndSetDialogue();
        }
    }

    private void CheckAndSetDialogue()
{
    string currentScene = SceneManager.GetActiveScene().name;

    // Clear the dialogue queue when a new scene is loaded to prevent leftover dialogues
    DialogueManager.Instance.EndDialogue();

    if (currentScene == "DialogueDemoScene")
    {
        List<string> dialogueLines = new List<string>
        {
            "Hello! Welcome to the start of the Tech Demo where I display that dialogue works! WOWZA! Press E to continue.",
            "If you are reading this, you ended up in a timeline where options 1 and 2 in my production system are true.",
            "Press E and then W to see what you get next time or press S to continue to the map."
        };

        DialogueManager.Instance.AddDialogueForScene(currentScene, dialogueLines);
        DialogueManager.Instance.StartDialogueForScene(currentScene);
    }
    else if (currentScene == "CombatScene")
    {
        List<string> combatDialogue = new List<string>
        {
            "Welcome to combat!"
        };

        DialogueManager.Instance.AddDialogueForScene(currentScene, combatDialogue);
        DialogueManager.Instance.StartDialogueForScene(currentScene);
    }
    else if(currentScene == "Main")
        {
            List<string> MainSceneDialogue = new List<string>
            {
                "Hey There! Welcome to the village. Your family is waiting for you! Help them reach Corcuta safely!"
            };

            DialogueManager.Instance.AddDialogueForScene(currentScene, MainSceneDialogue);
            DialogueManager.Instance.StartDialogueForScene(currentScene);

        }
  
}


    public void ResetSystem()
    {
        hasGiven = false;
        Debug.Log("System has reset");
    }
}
