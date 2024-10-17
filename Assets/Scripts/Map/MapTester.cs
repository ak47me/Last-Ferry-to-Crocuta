using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTester : MonoBehaviour
{
    public  bool mapCheck = false;
    public bool debug = true;
    public bool checkMapConfig = false;
    public bool checkNodeTypes = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!mapCheck)
        {
            Map.Instance.InitializeMap();
            Map.Instance.GenerateMap();
            
            if (!debug)
            {
                mapCheck = true;
                return;
            }

            foreach (List<MapNodeData> mapRow in Map.Instance.map)
            {
                foreach (MapNodeData mapNode in mapRow)
                {
                    
                    if (mapNode.displayable && checkMapConfig)
                    {
                        Debug.Log("Node: " + mapNode.mapPosition.x.ToString() + " " + mapNode.mapPosition.y.ToString());
                        foreach (MapNodeData neighbour in mapNode.Neighbours)
                        {
                            Debug.Log("Edge: " + neighbour.mapPosition.x.ToString() + " " + neighbour.mapPosition.y.ToString());
                        }
                    }

                    if (checkNodeTypes) Debug.Log(mapNode.mapPosition.y.ToString() + " " + mapNode.mapPosition.x.ToString() + " : " + mapNode.encounterType);
                }
            }
            mapCheck = true;
        }
    }
}
