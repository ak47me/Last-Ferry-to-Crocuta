using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    // we can make instance of MainManager instance as follows MainManager.Instance.something
    public static MainManager Instance;
    // Field to track the current map position
    public Vector2 currentMapPosition;
    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        // for first time :
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Method to update the current map position
    public void SetMapPosition(Vector2 position)
    {
        currentMapPosition = position;
        Debug.Log("Current map position set to: " + currentMapPosition);
    }

    // Method to get the current map position
    public Vector2 GetMapPosition()
    {
        return currentMapPosition;
    }
}