using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public GameObject creatureCollection;
    public GameObject creaturePrefab;
    public int numCreatures;
    public float xmin;
    public float xmax;
    public float ymin;
    public float ymax;
    public float epoch;
    
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        List<GameObject> newCreatures = new List<GameObject>();
        PopulateZoo(newCreatures);
    }

    // Update is called once per frame
    void Update()
    {
        if((Time.time-startTime) > epoch)
        {
            Debug.Log("epoch limit reached");
            List<GameObject> newCreatures = new List<GameObject>();
            startTime = Time.time;
            for(int i=0;i<creatureCollection.transform.childCount;i++)
            {
                // condittional survival goes here
                Destroy(creatureCollection.transform.GetChild(i).gameObject);
            }
            PopulateZoo(newCreatures);
        }
            
    }

    void PopulateZoo(List<GameObject> creatures)
    {
        
        for(int i=0;i<numCreatures;i++)
        {
            float xpos = Random.Range(xmin*0.9f,xmax*0.9f);
            float ypos = Random.Range(ymin*0.9f,ymax*0.9f);
            Vector2 newPos = new Vector2(xpos,ypos);
            GameObject newCreature = (GameObject)Instantiate(creaturePrefab,newPos,Quaternion.identity);
            newCreature.GetComponent<CreatureController>().world = this.gameObject;
            newCreature.transform.parent = creatureCollection.transform;
            newCreature.transform.localScale = new Vector3(0.5f,0.5f,1.0f);

        }
    }
}
