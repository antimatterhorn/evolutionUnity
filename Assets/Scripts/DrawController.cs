using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawController : MonoBehaviour
{
    public GameObject world;
    
    private WorldController worldController;
    private LineRenderer  lineRend;
    private Vector2 initialMousePosition, currentMousePosition;
    
    // Start is called before the first frame update
    void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        lineRend.positionCount = 0;
        worldController = world.GetComponent<WorldController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(worldController.play && worldController.InWorld(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                lineRend.positionCount = 4;
                initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                initialMousePosition.x = Mathf.Min(Mathf.Max(initialMousePosition.x,worldController.xmin),worldController.xmax);
                initialMousePosition.y = Mathf.Min(Mathf.Max(initialMousePosition.y,worldController.ymin),worldController.ymax);
                lineRend.SetPosition(0, new Vector2(initialMousePosition.x,initialMousePosition.y));
                lineRend.SetPosition(1, new Vector2(initialMousePosition.x,initialMousePosition.y));
                lineRend.SetPosition(2, new Vector2(initialMousePosition.x,initialMousePosition.y));
                lineRend.SetPosition(3, new Vector2(initialMousePosition.x,initialMousePosition.y));
            }
        }
        if (Input.GetMouseButton(0))
        {
            if(worldController.play && worldController.InWorld(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                currentMousePosition.x = Mathf.Min(Mathf.Max(currentMousePosition.x,worldController.xmin),worldController.xmax);
                currentMousePosition.y = Mathf.Min(Mathf.Max(currentMousePosition.y,worldController.ymin),worldController.ymax);
                lineRend.SetPosition(0, new Vector2(initialMousePosition.x,initialMousePosition.y));
                lineRend.SetPosition(1, new Vector2(initialMousePosition.x,currentMousePosition.y));
                lineRend.SetPosition(2, new Vector2(currentMousePosition.x,currentMousePosition.y));
                lineRend.SetPosition(3, new Vector2(currentMousePosition.x,initialMousePosition.y));
                worldController.SetBreedCorners(initialMousePosition,currentMousePosition);
            }
        }
    }
}
