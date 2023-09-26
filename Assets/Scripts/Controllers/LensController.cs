using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LensController : MonoBehaviour
{
    Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,6));
    }
}
