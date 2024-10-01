using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HexagonInteraction : MonoBehaviour
{
    // This will be called when the player clicks on the hexagon
    private void OnMouseDown()
    {
        // Check which hexagon was clicked based on the name or tag
        if (gameObject.name == "CombatHexagon")
        {
            SceneManager.LoadScene("CombatScene");
        }
        else if (gameObject.name == "ChallengeHexagon")
        {
            SceneManager.LoadScene("ChallengeScene");
        }
        else if (gameObject.name == "ExploreHexagon")
        {
            SceneManager.LoadScene("ExploreScene");
        }
    }


}
