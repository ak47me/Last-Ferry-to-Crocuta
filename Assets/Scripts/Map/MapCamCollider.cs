using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamCollider : MonoBehaviour
{
    public float dSpeed;
    public string moveKey;
    public string altMoveKey;
    public bool mouseMove = false;

    void OnMouseOver()
    {
        MapCamera.Instance.mouseMove = true;
        MapCamera.Instance.speed = dSpeed;
    }

    void OnMouseExit()
    {
        MapCamera.Instance.mouseMove = false;
    }
}
