using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// CREDIT TO FAIAZ BIN NASAR (CMPUT 250 EP) FOR THE BASE IMPLEMENTATION OF THIS
public class DialogueManager : MonoBehaviour
{

    // Mod: Made it a singleton for my use case:
    private static DialogueManager _instance;
    public static DialogueManager Instance { get { return _instance; } }

    public TextMeshProUGUI text;
    public GameObject dialogueBox;
    public float dialogueSpd;

    Queue<string> dialogueEntries;
    bool dialogueActive = false;
    Coroutine typingCoroutine;

    
    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogueEntries = new Queue<string>();
        dialogueBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            if (typingCoroutine != null)
            {
                print("stopping co routine");
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
                DisplayDialogue();
            }
            else if (dialogueEntries.Count >= 0)
            {
                print("starting dialogue display");
                DisplayDialogue();
            }
        }
    }

    public void StartDialogue(List<string> newDialogue)
    {
        if (dialogueActive)
        {
            print("error");
            return;
        }

        dialogueBox.SetActive(true);
        dialogueEntries.Clear();

        foreach (string newSentence in newDialogue)
        {
            print(newSentence);
            dialogueEntries.Enqueue(newSentence);
        }

        text.text = "press E";
        dialogueActive = true;
    }

    public void DisplayDialogue()
    {
        if (dialogueEntries.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = dialogueEntries.Dequeue();

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
        return dialogueEntries.Count;
    }
}
