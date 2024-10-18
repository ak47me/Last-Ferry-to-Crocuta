using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Singleton
    private static Map _instance;
    public static Map Instance { get { return _instance; } }

    // Map Data and Generation Information
    public List<List<MapNodeData>> map = new List<List<MapNodeData>>();
    public List<MapNode> mapView;
    public List<List<bool>> completed = new List<List<bool>>();
    private int startSeed;
    private int currentSeed;
    public int width = 4;
    public int height = 6;
    private List<Vector3> frontier = new List<Vector3>();
    private List<Vector3> connectable = new List<Vector3>();
    private List<int> nodesPerRow = new List<int>();
    public float twoRatio = 0.75f; // chance of a node having 2 children
    bool challengeAssigned = false;

    // Visual Information
    public Sprite background;
    public Camera cam;
    public SpriteRenderer mapRenderer;

    // Objects The Map Notably Uses
    public MapInfo mapInfoList;

    // Visual Node Information
    public GameObject nodePrefab;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    public float xOffset;
    public float yOffset;
    public float xBoundSpace;
    public float yBoundSpace;

    // Player Map Data
    public int encountersDone = 0;

    // Code to trigger on creation of a Map
    void Awake()
    {
        _instance = this;
        startSeed = (int)System.DateTime.Now.Ticks;
        currentSeed = startSeed;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Generates a map using a PCG algorithm
    public void GenerateMap()
    {
        // Determine our starting position
        SetAndModSeed();
        int startCol = width / 2;
        map[height - 1][startCol].displayable = true;
        map[height - 1][startCol].encounterType = MapNodeData.nodeType.START;
        MapPlayer.Instance.setMapPosition((float)startCol, (float)height - 1);
        completed[height - 1][startCol] = true;

        // Prepare a starting map node, and confirm we want to generate
        bool generating = true;
        int curRow = height - 1;
        frontier.Add(new Vector3(startCol, curRow, 1));
        connectable.Add(new Vector3(startCol, curRow, 2));
        nodesPerRow[curRow] += 1;

        // Attempts is a debug variable to ensure we don't break Unity
        int attempts = 0;
        while (generating && attempts < 1000)
        {
            attempts += 1;
            curRow -= 1;

            // Get the next node to neigbhour off of and clear the frontier
            SetAndModSeed();
            int randIdx = Random.Range(0, frontier.Count);
            Vector3 curNode = frontier[randIdx];

            if (curRow == 0)
            {
                SetAndModSeed();
                int minX = 0;
                int maxX = width + 1;
                foreach (Vector3 frontierItem in frontier)
                {
                    // No, these checks are not typos, it's an algorithm thing.
                    if (frontierItem.x - 1 >= 0 && frontierItem.x - 1 > minX)
                    {
                        minX = (int)frontierItem.x - 1;
                    }

                    if (frontierItem.x + 1 < width && frontierItem.x < maxX)
                    {
                        maxX = (int)frontierItem.x + 1;
                    }
                }
                int bossCol = Random.Range(minX, maxX + 1);

                map[0][bossCol].displayable = true;
                map[0][bossCol].encounterType = MapNodeData.nodeType.BOSS;

                foreach (Vector3 frontierItem in frontier)
                {
                    map[0][bossCol].Neighbours.Add(map[(int)frontierItem.y][(int)frontierItem.x]);
                }

                if (map[curRow + 2][bossCol].displayable && !map[curRow + 1][bossCol].displayable)
                {
                    map[0][bossCol].Neighbours.Add(map[curRow + 2][bossCol]);
                }
                generating = false;
                continue;
            }

            // IDEA: COULD WE CLEAR THE FRONTIER LATER TO CUT DOWN ON IF STATEMENTS IN THE BIG FOR LOOP?
            frontier.Clear();

            // Choose a random number of new nodes
            SetAndModSeed();
            float pSuccessor = Random.Range(0f, 1f);
            int numSuccessors;
            if (pSuccessor <= twoRatio)
            {
                numSuccessors = 2;
            }
            else
            {
                numSuccessors = 1;
            }
            bool edgeCheck = false;

            // Create the list of successors
            int startx = (int)curNode.x - 1;
            int endx = (int)curNode.x + 1;
            if (startx < 0)
            {
                startx += 1;
            }
            if (endx >= width)
            {
                endx -= 1;
            }
            List<int> successors = new List<int>();

            for (int i = startx; i <= endx; i++)
            {
                successors.Add(i);
            }


            for (int i = 0; i < numSuccessors; i++)
            {
                //Debug.Log("current Node: " + (curNode.y.ToString()) + " " + curNode.x.ToString());

                // Edge case: we need to ensure two nodes in last row have a common successor.
                if (nodesPerRow[curRow + 1] == 2 && !edgeCheck)
                {

                    // Edge case 1: two map nodes are in the same row but have one column between them
                    if ((int)curNode.x + 2 < width && map[(int)curNode.y][(int)curNode.x + 2].displayable)
                    {
                        map[curRow][(int)curNode.x + 1].displayable = true;
                        map[curRow][(int)curNode.x + 1].Neighbours.Add(map[(int)curNode.y][(int)curNode.x]);
                        map[curRow][(int)curNode.x + 1].Neighbours.Add(map[(int)curNode.y][(int)curNode.x + 2]);
                        successors.Remove((int)curNode.x + 1);
                        //Debug.Log("connected: " + curRow.ToString() + " " + (curNode.x + 1).ToString() + " with " + curNode.y.ToString() + " " + (curNode.x + 2).ToString());
                        //Debug.Log("connected: " + curRow.ToString() + " " + (curNode.x + 1).ToString() + " with " + curNode.y.ToString() + " " + (curNode.x).ToString());
                        frontier.Add(new Vector3(curNode.x + 1, curRow, 1));
                        nodesPerRow[curRow] += 1;


                        // Chance for a third connection here
                        if (map[(int)curNode.y + 1][(int)curNode.x + 1].displayable)
                        {
                            map[curRow][(int)curNode.x + 1].Neighbours.Add(map[(int)curNode.y + 1][(int)curNode.x + 1]);
                            //Debug.Log("connected: " + curRow.ToString() + " " + (curNode.x + 1).ToString() + " with " + (curNode.y + 1).ToString() + " " + (curNode.x + 1).ToString());
                        }

                        assignEncounter(curRow, (int)curNode.x + 1);
                        edgeCheck = true;
                        continue;

                    }
                    else if ((int)curNode.x - 2 >= 0 && map[(int)curNode.y][(int)curNode.x - 2].displayable)
                    {
                        map[curRow][(int)curNode.x - 1].displayable = true;
                        map[curRow][(int)curNode.x - 1].Neighbours.Add(map[(int)curNode.y][(int)curNode.x]);
                        map[curRow][(int)curNode.x - 1].Neighbours.Add(map[(int)curNode.y][(int)curNode.x - 2]);
                        successors.Remove((int)curNode.x - 1);
                        //Debug.Log("connected2: " + curRow.ToString() + " " + (curNode.x - 1).ToString() + " with " + curNode.y.ToString() + " " + (curNode.x - 2).ToString());
                        //Debug.Log("connected2: " + curRow.ToString() + " " + (curNode.x - 1).ToString() + " with " + curNode.y.ToString() + " " + (curNode.x).ToString());
                        frontier.Add(new Vector3(curNode.x - 1, curRow, 1));
                        nodesPerRow[curRow] += 1;

                        // Chance for a third connection here
                        if (map[(int)curNode.y + 1][(int)curNode.x - 1].displayable)
                        {
                            map[curRow][(int)curNode.x - 1].Neighbours.Add(map[(int)curNode.y + 1][(int)curNode.x - 1]);
                            //Debug.Log("connected2: " + curRow.ToString() + " " + (curNode.x - 1).ToString() + " with " + (curNode.y + 1).ToString() + " " + (curNode.x - 1).ToString());
                        }

                        assignEncounter(curRow, (int)curNode.x - 1);
                        edgeCheck = true;
                        continue;
                    }

                    SetAndModSeed();

                    // Edge case 2: two map nodes are in same row, but they are adjacent
                    if ((int)curNode.x + 1 < width && map[(int)curNode.y][(int)curNode.x + 1].displayable)
                    {
                        int successorsIndx = Random.Range(1, successors.Count);
                        int currCol = successors[successorsIndx];
                        successors.RemoveAt(successorsIndx);

                        map[curRow][currCol].displayable = true;
                        map[curRow][currCol].Neighbours.Add(map[(int)curNode.y][(int)curNode.x]);
                        map[curRow][currCol].Neighbours.Add(map[(int)curNode.y][(int)curNode.x + 1]);
                        //Debug.Log("connected3: " + curRow.ToString() + " " + currCol.ToString() + " with " + curNode.y.ToString() + " " + (curNode.x + 1).ToString());
                        //Debug.Log("connected3: " + curRow.ToString() + " " + currCol.ToString() + " with " + curNode.y.ToString() + " " + (curNode.x).ToString());
                        frontier.Add(new Vector3(currCol, curRow, 1));
                        nodesPerRow[curRow] += 1;

                        assignEncounter(curRow, currCol);
                        edgeCheck = true;
                        continue;
                    }
                    else if ((int)curNode.x - 1 >= 0 && map[(int)curNode.y][(int)curNode.x - 1].displayable)
                    {
                        int successorsIndx = Random.Range(0, successors.Count - 1);
                        int currCol = successors[successorsIndx];
                        successors.RemoveAt(successorsIndx);

                        map[curRow][currCol].displayable = true;
                        map[curRow][currCol].Neighbours.Add(map[(int)curNode.y][(int)curNode.x]);
                        map[curRow][currCol].Neighbours.Add(map[(int)curNode.y][(int)curNode.x - 1]);
                        //Debug.Log("connected4: " + curRow.ToString() + " " + currCol.ToString() + " with " + curNode.y.ToString() + " " + (curNode.x - 1).ToString());
                        //Debug.Log("connected4: " + curRow.ToString() + " " + currCol.ToString() + " with " + curNode.y.ToString() + " " + (curNode.x).ToString());
                        frontier.Add(new Vector3(currCol, curRow, 1));
                        nodesPerRow[curRow] += 1;

                        assignEncounter(curRow, currCol);
                        edgeCheck = true;
                        continue;
                    }

                }

                // Get an adjacent column 1 row up for a new successor
                // NOTE: DOES NOT ACCOUNT FOR EDGE CASE WHERE LENGTH OF MAP IS 3
                SetAndModSeed();
                int successorsIdx;

                if (nodesPerRow[curRow + 1] == 1)
                {
                    float pSuccessors = Random.Range(0f, 1f);
                    if (pSuccessors < 0.4) successorsIdx = 0;
                    else if (pSuccessors < 0.6) successorsIdx = 1;
                    else successorsIdx = successors.Count - 1;
                }
                else
                {
                    successorsIdx = Random.Range(0, successors.Count);
                }

                int curCol = successors[successorsIdx];
                successors.RemoveAt(successorsIdx);
                map[curRow][curCol].displayable = true;
                map[curRow][curCol].Neighbours.Add(map[(int)curNode.y][(int)curNode.x]);
                //Debug.Log("connected5: " + curRow.ToString() + " " + curCol.ToString() + " with " + curNode.y.ToString() + " " + curNode.x.ToString());

                if (curRow + 2 <= height - 1 && map[curRow + 2][curCol].displayable && !map[curRow + 1][curCol].displayable)
                {
                    map[curRow][curCol].Neighbours.Add(map[(int)curRow + 2][curCol]);
                    //Debug.Log("connected5: " + curRow.ToString() + " " + curCol.ToString() + " with " + (curRow+2).ToString() + " " + curCol.ToString());
                }

                assignEncounter(curRow, curCol);
                frontier.Add(new Vector3(curCol, curRow, 1));
                nodesPerRow[curRow] += 1;
            }
        }

        // Make real, visible map nodes for the player
        // FIXME: SAVE MAPVIEW INDEX INTO DATA
        foreach (List<MapNodeData> dataRow in map)
        {
            foreach (MapNodeData data in dataRow)
            {
                if (data.displayable)
                {
                    // STEP 1: INSTANTIATE GAME OBJECT BASED ON PREFAB YOU MAKE EARLIER
                    GameObject MapNodeObj = Instantiate(nodePrefab);

                    // STEP 2: SET UP MAPNODE WITH INFORMATION NEEDED
                    MapNode mapNode = MapNodeObj.GetComponent<MapNode>();
                    mapNode.InitNode(data, mapInfoList.scriptNodes[(int)data.encounterType]);

                    // STEP 3: ASSIGN IT A POSITION SOMEHOW
                    mapNode.transform.position = data.visualPosition;

                    // STEP 4: Add it to the actual list
                    mapView.Add(mapNode);

                    if (MapPlayer.Instance.mapPosition == data.mapPosition)
                    {
                        MapPlayer.Instance.currentNode = mapNode;
                    }
                }
            }
        }

        // Last step: connect lines between MapNodes
        ConnectMapNodes();
    }

    public void InitializeMap()
    {
        // Initialize the visual bounds of the map for nodes
        Bounds nodeBounds = mapRenderer.sprite.bounds;
        minX = nodeBounds.min.x * transform.localScale.x + xBoundSpace;
        maxX = nodeBounds.max.x * transform.localScale.x - xBoundSpace;
        minY = nodeBounds.min.y * transform.localScale.y + yBoundSpace;
        maxY = nodeBounds.max.y * transform.localScale.y;
        xOffset = 1.2f * (maxX - minX) / width;
        yOffset = 1f * (maxY - minY) / height;

        for (int i = 0; i < height; i++)
        {
            map.Add(new List<MapNodeData>());
            completed.Add(new List<bool>());
            nodesPerRow.Add(0);

            for (int j = 0; j < width; j++)
            {
                map[i].Add(new MapNodeData(j, i));
                map[i][j].SetVisualPosition(minX, minY, xOffset, yOffset);
                completed[i].Add(false);
            }
        }
    }

    private void SetAndModSeed()
    {
        currentSeed += 1;
        Random.InitState(currentSeed);
    }

    private void assignEncounter(int y, int x)
    {
        bool neighbour3 = map[y][x].Neighbours.Count >= 3;
        bool combatCondition = CombatRowCondition(y);
        bool challengable = y <= height / 2;
        Vector2 mapPosition = map[y][x].mapPosition; // Track map position

        // Check if cards are already assigned for the current map position in MainManager
        if (MainManager.Instance != null && MainManager.Instance.AreCardsAssignedToPosition(mapPosition))
        {
            Debug.Log("Cards already assigned to the current map position: " + mapPosition);
            map[y][x].assignedCards = MainManager.Instance.GetAssignedCardsForPosition(mapPosition);
            return; // Exit if cards have already been assigned to this position
        }

        if ((neighbour3 || combatCondition) && (challengable && !challengeAssigned))
        {
            challengeAssigned = true;
            map[y][x].encounterType = MapNodeData.nodeType.CHALLENGE;
        }
        else if (combatCondition)
        {
            map[y][x].encounterType = MapNodeData.nodeType.COMBAT;
            // Assign cards to the current map position only if they haven't been assigned before
            if (map[y][x].assignedCards == null)
            {
                Debug.Log("Assigning enemy cards to node on map position: " + mapPosition);
                
                map[y][x].assignedCards = GetRandomEnemyCards(3); // Assign 3 random cards to the node
                //map[y][x].assignedKeyCards = MainManager.Instance.SelectKeyCardsForCombat();
                MainManager.Instance.AssignCardsToPosition(mapPosition, map[y][x].assignedCards); // Update MainManager with the assigned cards
                //MainManager.Instance.AssignCardsToPosition(mapPosition, map[y][x].assignedKeyCards);
                Debug.Log("Completed operation successfully in assign encounter");
            }
        }
        else if (neighbour3)
        {
            if (map[y + 2][x].encounterType != MapNodeData.nodeType.COMBAT)
            {
                map[y][x].encounterType = MapNodeData.nodeType.COMBAT;
                if (map[y][x].assignedCards == null)
                {
                    Debug.Log("Assigning enemy cards to node on map position: " + mapPosition);
                    Debug.Log("Assigning character cards to node on map position: " + mapPosition);
                    map[y][x].assignedCards = GetRandomEnemyCards(3); // Assign 3 random cards to the node
                    //map[y][x].assignedKeyCards = MainManager.Instance.SelectKeyCardsForCombat();
                    MainManager.Instance.AssignCardsToPosition(mapPosition, map[y][x].assignedCards); // Update MainManager with the assigned cards
                    //MainManager.Instance.AssignCardsToPosition(mapPosition, map[y][x].assignedKeyCards);
                    Debug.Log("Completed operation successfully in assign encounter");
                }
                return;
            }

            SetAndModSeed();

            float itemOverCombat = Random.Range(0f, 1f);
            if (itemOverCombat < 0.5)
            {
                map[y][x].encounterType = MapNodeData.nodeType.COMBAT;
                if (map[y][x].assignedCards == null)
                {
                    Debug.Log("Assigning enemy cards to node on map position: " + mapPosition);
                    //Debug.Log("Assigning character cards to node on map position: " + mapPosition);
                    map[y][x].assignedCards = GetRandomEnemyCards(3); // Assign 3 random cards to the node
                    //map[y][x].assignedKeyCards = MainManager.Instance.SelectKeyCardsForCombat();
                    MainManager.Instance.AssignCardsToPosition(mapPosition, map[y][x].assignedCards); // Update MainManager with the assigned cards
                    //MainManager.Instance.AssignCardsToPosition(mapPosition, map[y][x].assignedKeyCards);
                    Debug.Log("Completed operation successfully in assign encounter");
                }
            }
            else map[y][x].encounterType = MapNodeData.nodeType.ITEM;
        }
        else
        {
            SetAndModSeed();
            float itemCondition = Random.Range(0f, 1f);
            if (itemCondition < 0.75)
                map[y][x].encounterType = MapNodeData.nodeType.ITEM;
            else
                map[y][x].encounterType = MapNodeData.nodeType.CARDS;
        }
    }


    // Helper method to get random enemy cards from MainManager's list
    private List<CardInfo> GetRandomEnemyCards(int count)
    {
        List<CardInfo> randomCards = new List<CardInfo>();
        List<CardInfo> availableCards = new List<CardInfo>(MainManager.Instance.enemyCards); // Create a copy to avoid modifying the original list

        // Ensure that the count does not exceed the available cards
        count = Mathf.Min(count, availableCards.Count);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, availableCards.Count);
            randomCards.Add(availableCards[randomIndex]);
            availableCards.RemoveAt(randomIndex); // Remove the selected card to avoid duplicates
        }

        return randomCards;
    }



    public void crossOutNodes()
    {
        foreach (MapNode node in mapView)
        {
            if (node.Data.mapPosition.y > height - 1 - encountersDone)
            {
                node.CrossOut();
            }
        }
    }

    private bool CombatRowCondition(int rowIdx)
    {
        bool oddRows = (height - 1) % 2 == 0 && rowIdx % 2 != 0;
        bool evenRows = (height - 1) % 2 != 0 && rowIdx % 2 == 0;

        // Returns false if both starting row and rowIdx are even or both are odd.
        return oddRows || evenRows;
    }

    public void ConnectMapNodes()
    {
        for (int i = 0; i < mapView.Count; i++)
        {
            mapView[i].ConnectToNeighbours();
        }
    }

    // TODO: Load save data from previous scene here
    public void UpdateMapPostEncounter()
    {
        encountersDone += 1;
        Vector2 completedPoint = MapPlayer.Instance.mapPosition;
        completed[(int)completedPoint.y][(int)completedPoint.x] = true;

        crossOutNodes();
    }

    // TODO: Implement this to load in the next scene and save info about the current one
    public void HandleMapMove(int y, int x)
    {
        //FIXME: THIS IS TEMPORARY should load next scene and save
        //THere should also be an edge case for when you get to the boss
        UpdateMapPostEncounter();
    }
}
