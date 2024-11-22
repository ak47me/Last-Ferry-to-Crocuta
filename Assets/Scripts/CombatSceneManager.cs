using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneManager : MonoBehaviour
{
    void Start()
    {
        if (MainManager.Instance != null)
        {
            Vector2 mapPosition = MainManager.Instance.GetMapPosition();
            Debug.Log("Player's map position on entering Combat Scene: " + mapPosition);
        }
        else
        {
            Debug.LogError("MainManager instance is missing.");
        }
    }
}

