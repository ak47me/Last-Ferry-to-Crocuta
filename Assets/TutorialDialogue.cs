using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogue : MonoBehaviour
{   
    private void Start()
    {
        DialogueMapManager.GetInstance().EnterDialogueMode("We are in the Dialogue Panel");
    }
}
