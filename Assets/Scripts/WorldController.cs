using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public GameObject creatureCollection;
    public GameObject creature;
    public int numCreatures;
    public float xmin;
    public float xmax;
    public float ymin;
    public float ymax;
    


    // Start is called before the first frame update
    void Start()
    {
        PopulateZoo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PopulateZoo()
    {
        for(int i=0;i<numCreatures;i++)
        {
            float xpos = Random.Range(xmin*0.9f,xmax*0.9f);
            float ypos = Random.Range(ymin*0.9f,ymax*0.9f);
            Vector2 newPos = new Vector2(xpos,ypos);
            GameObject newCreature = (GameObject)Instantiate(creature,newPos,Quaternion.identity);
            newCreature.GetComponent<CreatureController>().world = this.gameObject;
            newCreature.transform.parent = creatureCollection.transform;
            newCreature.transform.localScale = new Vector3(0.5f,0.5f,1.0f);

        }
    }
}
