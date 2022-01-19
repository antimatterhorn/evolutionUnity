using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    private float moveWait = 5f;
    private float moveTime;
    public float moveSpeed = 0.1f;
    private Vector2 moveDir = new Vector2(0f,0f);
    // Start is called before the first frame update
    void Start()
    {
        moveTime = Time.time - moveWait;
    }

    // Update is called once per frame
    void Update()
    {
        if((Time.time - moveTime)>moveWait)
        {
            moveWait = Random.Range(2f,5f);
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
