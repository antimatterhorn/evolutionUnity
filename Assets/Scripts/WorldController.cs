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
    
    public float startTime;


    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        List<Genome> newGenomes = new List<Genome>();
        PopulateZoo(newGenomes);
    }

    // Update is called once per frame
    void Update()
    {
        if((Time.time-startTime) > epoch)
        {
            Debug.Log("epoch limit reached");
            List<Genome> newGenomes = new List<Genome>();
            startTime = Time.time;
            for(int i=0;i<creatureCollection.transform.childCount;i++)
            {
                // condittional survival goes here
                GameObject thisCreature = creatureCollection.transform.GetChild(i).gameObject;
                if(thisCreature.transform.position.x>6)
                {
                    Genome thisGenome = thisCreature.GetComponent<CreatureController>().Genome;
                    newGenomes.Add(thisGenome);
                }

                Destroy(thisCreature);
            }
            Debug.Log(newGenomes.Count);
            PopulateZoo(newGenomes);
        }
            
    }

    void PopulateZoo(List<Genome> genomes)
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
            if(genomes.Count>0)
            {

            }
        }
    }
}
