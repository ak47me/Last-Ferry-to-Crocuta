using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapCamera : MonoBehaviour
{
    // Singleton
    private static MapCamera _instance;
    public static MapCamera Instance { get { return _instance; } }

    public Camera mapCam;
    public Bounds camBounds;
    public float speed = 0f;
    public float dSpeed;
    public bool isMoving = false;
    public float slowDown;
    public float height;
    public float width;
    public float minY;
    public float maxY;

    public bool mouseMove = false;
    public bool keyMove = false;
    public string upKeyPress;
    public string upAltKeyPress;
    public string downKeyPress;
    public string downAltKeyPress;

    void Start()
    {

    }

    void Awake()
    {
        _instance = this;
        // Get the width and height of our camera
        height = mapCam.orthographicSize;
        width = height * mapCam.aspect;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(upKeyPress) || Input.GetKey(upAltKeyPress))
        {
            keyMove = true;
            speed = dSpeed;
        }
        else if (Input.GetKey(downKeyPress) || Input.GetKey(downAltKeyPress))
        {
            keyMove = true;
            speed = dSpeed * -1;
        }
        else
        {
            keyMove = false;
        }

        if (!mouseMove && !keyMove)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        if (!isMoving)
        {
            speed = Mathf.Max(speed - slowDown, 0);
        }

        SetCameraBounds();

        // NOTE TO TYLER: CLAMP REQUIRES (value to clamp, min, max)
        float newY = Mathf.Clamp(transform.position.y + speed * Time.deltaTime, minY, maxY);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void SetCameraBounds()
    {
        Sprite sprite = Map.Instance.mapRenderer.sprite;
        // Get the coordinates of the top of the map sprite and the bottom of the map sprite
        minY = Map.Instance.transform.localScale.y*sprite.bounds.min.y + height;
        maxY = Map.Instance.transform.localScale.y*sprite.bounds.max.y - height;
    }
}
