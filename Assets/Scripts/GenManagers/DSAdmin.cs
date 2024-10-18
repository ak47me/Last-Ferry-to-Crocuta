using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DSAdmin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && DialogueManager.Instance.DQEntries() <= 0)
        {
            ProductionSystem.Instance.ResetSystem();
        }
        else if (Input.GetKeyDown(KeyCode.S) && DialogueManager.Instance.DQEntries() <= 1)
        {
            SceneManager.LoadScene("MapScene");
        }
    }
}
