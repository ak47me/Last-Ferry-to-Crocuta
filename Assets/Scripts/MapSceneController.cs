using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSceneController : MonoBehaviour
{
    // This function will be called when the player interacts with the Combat button
    public void LoadCombatScene()
    {
        SceneManager.LoadScene("CombatScene");  // This assumes you will have a CombatScene created in the future
    }

    // Function for the Challenge Scene
    public void LoadChallengeScene()
    {
        SceneManager.LoadScene("ChallengeScene");
    }

    // Function for the Explore Scene
    public void LoadExploreScene()
    {
        SceneManager.LoadScene("ExploreScene");
    }
}

