using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapNode : MonoBehaviour
{
    private MapNodeData data;
    private ScriptableNode scriptNode;
    public MapNodeData Data { get { return data; } }
    public ScriptableNode ScriptNode { get { return scriptNode; } }
    public SpriteRenderer mainRenderer;
    public SpriteRenderer crossRenderer;
    public Sprite sprite;
    public Sprite crossOut;
    public GameObject lineRenderPrefab;
    private List<LineRenderer> linesFromNode = new List<LineRenderer>();



    // Icon details
    public float scale;
    public float enlargeScale;
    public float bossScale;
    public float selectionScale;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (MapPlayer.Instance.mapPosition == data.mapPosition)
        {
            transform.localScale = new Vector3(scale * selectionScale, scale * selectionScale, 0);
            mainRenderer.color = Color.green;
            
        }
    }
    // Determine what to do when a MapNode is clicked on
    void OnMouseDown()
    {
        Vector2 playerPos = MapPlayer.Instance.mapPosition;
        
        // Base Case: 
        // Case 1: Check if the player position on map is a neighbour of this
        foreach (MapNodeData neighbour in data.Neighbours)
        {
            if (neighbour.mapPosition.x == playerPos.x && neighbour.mapPosition.y == playerPos.y && !data.crossedOut && (Map.Instance.completed[(int)playerPos.y][(int)playerPos.x] || Map.Instance.completed[(int)data.mapPosition.y][(int)data.mapPosition.x]))
            {
                // Signal for the player to move toward this point and end the function
                print("edge connects to you");
                MapNode mapNode = MapPlayer.Instance.currentNode;
                MapPlayer.Instance.currentNode = this;
                MapPlayer.Instance.mapPosition = data.mapPosition;
                MainManager.Instance.SetMapPosition(data.mapPosition);
                mapNode.ResetShape();

<<<<<<< HEAD
                // Update the MainManager with the new map position
                MainManager.Instance.SetMapPosition(data.mapPosition);

                Map.Instance.HandleMapMove((int) data.mapPosition.y, (int) data.mapPosition.x);

                if (data.encounterType == MapNodeData.nodeType.COMBAT)
                {
                    MapDialogueManager.GetInstance().EnterCombatDialogueMode("CombatScene");
=======


                Map.Instance.HandleMapMove((int)data.mapPosition.y, (int)data.mapPosition.x);

                if (data.encounterType == MapNodeData.nodeType.COMBAT)
                {
                    // Pass the cards to the MainManager or another manager handling combat
                    MainManager.Instance.LoadCombatCards(data.assignedCards);
                    SceneManager.LoadScene("RevCombatScene");  //change scene name 
>>>>>>> 60dced96e2b7f117641b1a46bf659893b04b909c
                }
                else if (data.encounterType == MapNodeData.nodeType.CHALLENGE)
                {
                    MainManager.Instance.NextLevel("ChallengeScene");

                }
                else
                {
                    MainManager.Instance.NextLevel("ExploreScene");
                }

                return;
            }
        }

        // Case 2: Check if the player position on map has this as a neighbour
        foreach (MapNodeData neighbour in Map.Instance.map[(int)playerPos.y][(int)playerPos.x].Neighbours)
        {
            if (neighbour.mapPosition.x == data.mapPosition.x && neighbour.mapPosition.y == data.mapPosition.y && !data.crossedOut && (Map.Instance.completed[(int)playerPos.y][(int)playerPos.x] || Map.Instance.completed[(int)data.mapPosition.y][(int)data.mapPosition.x]))
            {
                // Signal for the player to move toward this point and end the function
                print("you connect to edge");
                MapNode mapNode = MapPlayer.Instance.currentNode;
                MapPlayer.Instance.currentNode = this;
                MapPlayer.Instance.mapPosition = data.mapPosition;
                mapNode.ResetShape();

                Map.Instance.HandleMapMove((int)data.mapPosition.y, (int)data.mapPosition.x);

                if (data.encounterType == MapNodeData.nodeType.COMBAT)
                {
<<<<<<< HEAD
                    MapDialogueManager.GetInstance().EnterCombatDialogueMode("CombatScene");
=======
                    MainManager.Instance.LoadCombatCards(data.assignedCards);
                    SceneManager.LoadScene("RevCombatScene");
>>>>>>> 60dced96e2b7f117641b1a46bf659893b04b909c
                }

                return;
            }
        }

        // Case 3: Do nothing because the player should not be able to go here
        // FIXME: Add some juice to show that this is not accessible
        MapDialogueManager.GetInstance().EnterDialogueMode("you do not connect to edge");
        print("you do not connect to edge");
    }

    // Determine what to do when a MapNode is hovered on
    void OnMouseEnter()
    {
        if (!data.crossedOut)
        {
            transform.localScale *= enlargeScale;
        }
    }

    // Determine what to do when a player no longer has mouse on MapNode
    void OnMouseExit()
    {
        if (data.encounterType == MapNodeData.nodeType.BOSS && !data.crossedOut)
        {
            transform.localScale = new Vector3(bossScale, bossScale, 0);
        }
        else if (!data.crossedOut)
        {
            transform.localScale = new Vector3(scale, scale, 0);
        }
    }

    public void InitNode(MapNodeData data, ScriptableNode scriptNode)
    {
        this.data = data;
        this.scriptNode = scriptNode;
        mainRenderer.sprite = scriptNode.sprite;
        sprite = scriptNode.sprite;
        if (data.encounterType == MapNodeData.nodeType.BOSS) transform.localScale *= bossScale;
        else transform.localScale *= scale;

        crossRenderer.enabled = data.crossedOut;

        for (int i = 0; i < data.Neighbours.Count; i++)
        {
            GameObject lrPrefab = Instantiate(lineRenderPrefab, this.transform);
            LineRenderer lr = lrPrefab.GetComponent<LineRenderer>();
            linesFromNode.Add(lr);
        }
    }

    public void CrossOut()
    {
        // Don't need to do this twice if we've already done it
        if (data.crossedOut) return;

        data.crossedOut = true;
        crossRenderer.enabled = true;
        mainRenderer.enabled = false;
    }

    public void ConnectToNeighbours()
    {
        for (int i = 0; i < linesFromNode.Count; i++)
        {
            Vector3[] positions = { data.visualPosition, data.Neighbours[i].visualPosition };
            linesFromNode[i].SetPositions(positions);
        }
    }

    public void ResetShape()
    {
        transform.localScale = new Vector3(scale, scale, 0);
        mainRenderer.color = Color.white;
    }
}
