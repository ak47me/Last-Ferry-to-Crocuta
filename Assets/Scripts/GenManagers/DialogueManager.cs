using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager _instance;
    public static DialogueManager Instance { get { return _instance; } }

    public TextMeshProUGUI text;
    public GameObject dialogueBox;
    public float dialogueSpd;

    private Dictionary<string, Queue<string>> sceneDialogues; // Stores dialogues for each scene
    private Queue<string> currentDialogue;
    private bool dialogueActive = false;
    private Coroutine typingCoroutine;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Keeps the DialogueManager across scenes
        }
    }

    void Start()
    {
        sceneDialogues = new Dictionary<string, Queue<string>>();
        dialogueBox.SetActive(false);
        FindUIElements(); // Ensure UI elements are found
    }

    public void UpdateUIReferences(TextMeshProUGUI newText, GameObject newDialogueBox)
    {
        text = newText;
        dialogueBox = newDialogueBox;
        Debug.Log(text);

    }

    public void FindUIElements()
    {
        Debug.Log("Inside FindUi elements ");
        GameObject newDialogueBox = GameObject.Find("DialogueCanvas");
<<<<<<< HEAD
        
        

            if (newDialogueBox != null)
            {
=======



        if (newDialogueBox != null)
        {
>>>>>>> 60dced96e2b7f117641b1a46bf659893b04b909c
            TextMeshProUGUI newText = newDialogueBox.transform.Find("DialogueText")?.GetComponent<TextMeshProUGUI>();
            Debug.Log(newText);

            if (newText != null)
            {
                UpdateUIReferences(newText, newDialogueBox);
                Debug.Log("DialogueCanvas and DialogueText found successfully!");
            }
            else
            {
                Debug.LogWarning("DialogueText not found inside DialogueCanvas.");
            }
        }
        else
        {
            Debug.LogWarning("DialogueCanvas not found in the scene.");
        }
    }

    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
                DisplayDialogue();
            }
            else if (currentDialogue.Count > 0) // Change to > 0 to check correctly
            {
                DisplayDialogue();
            }
        }
    }

    public void AddDialogueForScene(string sceneName, List<string> dialogueLines)
    {
        if (!sceneDialogues.ContainsKey(sceneName))
        {
            sceneDialogues[sceneName] = new Queue<string>();
        }

        foreach (string line in dialogueLines)
        {
            sceneDialogues[sceneName].Enqueue(line);
        }
    }

    public void StartDialogueForScene(string sceneName)
    {
        Debug.Log("Inside StartDialogue scene");
<<<<<<< HEAD
        
=======

>>>>>>> 60dced96e2b7f117641b1a46bf659893b04b909c
        // Clear the current dialogue queue to remove leftovers from the previous scene
        currentDialogue?.Clear();

        if (sceneDialogues.ContainsKey(sceneName))
        {
            currentDialogue = new Queue<string>(sceneDialogues[sceneName]);

            if (dialogueActive)
            {
                Debug.LogError("Dialogue is already active.");
                return;
            }

            dialogueBox.SetActive(true);
            DisplayDialogue();
            dialogueActive = true;
        }
        else
        {
            Debug.LogWarning("No dialogue found for this scene.");
        }
    }


    private void DisplayDialogue()
    {
        if (currentDialogue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = currentDialogue.Dequeue();

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
        dialogueActive = false;
<<<<<<< HEAD
        
=======

>>>>>>> 60dced96e2b7f117641b1a46bf659893b04b909c
    }

    IEnumerator TypeSentence(string sentence)
    {
        text.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(dialogueSpd);
        }
    }

    public int DQEntries()
    {
        return currentDialogue != null ? currentDialogue.Count : 0;
    }
}
