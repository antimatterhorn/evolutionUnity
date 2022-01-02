using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    public GameObject world;
    private WorldController wc;
    
    // Start is called before the first frame update
    void Start()
    {
        world = GameObject.FindGameObjectWithTag("GameController");
        wc = world.GetComponent<WorldController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other);
        if(other.gameObject.CompareTag("Creature") && other.GetType() == typeof(BoxCollider2D))
        {
            other.GetComponentInParent<CreatureController>().Food += 1f;
            wc.foodTree.Remove(this.gameObject.transform);
            Destroy(this.gameObject);
        }
    }
}
