using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackFromCombat : MonoBehaviour
{
    public Button backButton; // Drag your button object here in the Inspector

    private void Start()
    {
        backButton.onClick.AddListener(BackGame);
    }

    public void BackGame()
    {
        Debug.Log("BackGame function called");
        SceneManager.LoadScene(2);
    }
}
