using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlayer : MonoBehaviour
{
    private static MapPlayer _instance;
    public static MapPlayer Instance { get { return _instance; } }
    public Vector2 mapPosition;
    public MapNode currentNode;

    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMapPosition(float x, float y)
    {
        mapPosition = new Vector2(x, y);
    }
}
