using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleLayerController : MonoBehaviour
{
    public GameObject Bubble;
    public int bubbleCount;

    private float moveWait = 5f;
    private float moveTime;
    public float moveSpeed = 0.1f;
    private Vector2 moveDir = new Vector2(0f,0f);
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < bubbleCount; i++)
        {
            float xpos = this.transform.position.x + Random.Range(-15f,15f);
            float ypos = this.transform.position.y + Random.Range(-15f,15f);
            float scale = Random.Range(0.1f,0.2f);
            Vector2 newPos = new Vector2(xpos,ypos);
            GameObject newBubble = (GameObject)Instantiate(Bubble,newPos,Quaternion.identity);
            newBubble.transform.parent = transform;
            newBubble.transform.localScale = new Vector3(scale,scale,1.0f);
        }
        moveWait = Random.Range(2f,6f);       
        moveTime = Time.time - moveWait; 
    }

    // Update is called once per frame
    void Update()
    {
        if((Time.time - moveTime)>moveWait)
        {
            moveWait = Random.Range(2f,6f);
            moveTime = Time.time;
            float newXDir = Random.Range(-1f,1f);
            float newYDir = Random.Range(-1f,1f);
            moveDir = new Vector2(newXDir,newYDir).normalized;
        }
        else
        {
            transform.position = new Vector3(transform.position.x+moveDir.x*moveSpeed*Time.deltaTime, transform.position.y+moveDir.y*moveSpeed*Time.deltaTime,transform.position.z);

        }
    }
}
