using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
            Debug.Log("eaten");
            Destroy(this.gameObject);
        }
    }
}
