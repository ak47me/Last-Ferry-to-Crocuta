using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapInfo : ScriptableObject
{
    public List<ScriptableNode> scriptNodes;
    public int numCombat;
    public int numItem;
    public int numCard;
    public int numChallenge;
}
