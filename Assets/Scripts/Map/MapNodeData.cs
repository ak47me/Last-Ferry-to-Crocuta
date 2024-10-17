using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNodeData
{

    private List<MapNodeData> neighbours = new List<MapNodeData>();
    public List<MapNodeData> Neighbours { get { return neighbours; } }

    public int timeRemaining;
    public Vector2 mapPosition;
    public bool displayable = false;
    public bool visitable = true;
    public Vector3 visualPosition;
    public bool crossedOut;

    public enum nodeType
    {
        UNNASSIGNED,
        COMBAT,
        ITEM,
        CARDS,
        CHALLENGE,
        BOSS,
        START
    }

    public nodeType encounterType = nodeType.UNNASSIGNED;

    // Constructor for a MapNode
    public MapNodeData(int x, int y)
    {
        mapPosition.x = x;
        mapPosition.y = y;
        crossedOut = false;
    }

    // Set up real coordinates in Game Space based on some information
    public void SetVisualPosition(float minX, float minY, float xOffset, float yOffset)
    {
        visualPosition = new Vector3(1, 1, 0);
        visualPosition.x = minX + mapPosition.x * xOffset;
        visualPosition.y = minY + (Map.Instance.height - 1 - mapPosition.y) * yOffset + Random.Range(-0.5f, 0.5f);
    }
}
