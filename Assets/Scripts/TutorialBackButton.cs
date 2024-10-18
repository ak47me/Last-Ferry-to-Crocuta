using UnityEngine;
using UnityEngine.UI; // Required for accessing UI elements
using UnityEngine.SceneManagement; // Required for scene management

public class TutorialBackButton : MonoBehaviour
{
    private Button backButton; // Reference to the Back button

    void Start()
    {
        // Find the button with the text "Back"
        backButton = GameObject.Find("Back").GetComponent<Button>();

        // Check if the button was found
        if (backButton != null)
        {
            // Add an OnClick listener to the button
            backButton.onClick.AddListener(OnBackButtonClick);
        }
        else
        {
            Debug.LogError("Back button not found! Please ensure the button is named 'Back'.");
        }
    }

    // Function to handle the back button click
    private void OnBackButtonClick()
    {
        // Load the Main scene
        SceneManager.LoadScene("Main");
    }
}
