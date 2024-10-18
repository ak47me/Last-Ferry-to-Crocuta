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
        UNASSIGNED,
        COMBAT,
        ITEM,
        CARDS,
        CHALLENGE,
        BOSS,
        START
    }

    public nodeType encounterType = nodeType.UNASSIGNED;
    // Field to store assigned cards
    public List<CardInfo> assignedCards = null;
    public List<CardInfo> assignedKeyCards = null;
    // New property to hold enemy cards for the node

    // New property to hold enemy cards for the node
    //public List<CardInfo> enemyCards = new List<CardInfo>();

    // Constructor for a MapNode
    public MapNodeData(int x, int y)
    {
        mapPosition.x = x;
        mapPosition.y = y;
        crossedOut = false;
    }

    // Method to check if the node already has assigned cards


    //set to orignal method
    public bool HasAssignedCards()
    {
        return assignedCards != null && assignedCards.Count > 0;
    }
    public bool HasAssignedKeyCards()
    {
        return assignedKeyCards != null && assignedKeyCards.Count > 0;
    }
    // Set up real coordinates in Game Space based on some information
    public void SetVisualPosition(float minX, float minY, float xOffset, float yOffset)
    {
        visualPosition = new Vector3(1, 1, 0);
        visualPosition.x = minX + mapPosition.x * xOffset;
        visualPosition.y = minY + (Map.Instance.height - 1 - mapPosition.y) * yOffset + Random.Range(-0.5f, 0.5f);
    }
}
