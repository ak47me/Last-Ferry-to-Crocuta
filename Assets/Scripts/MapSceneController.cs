using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSceneController : MonoBehaviour
{
    // This function will be called when the player interacts with the Combat button
    public void LoadCombatScene()
    {

        MainManager.Instance.NextLevel("CombatScene");

    }

    // Function for the Challenge Scene
    public void LoadChallengeScene()
    {
        MainManager.Instance.NextLevel("ChallengeScene");

    }

    // Function for the Explore Scene
    public void LoadExploreScene()
    {
        MainManager.Instance.NextLevel("ExploreScene");

    }
}

