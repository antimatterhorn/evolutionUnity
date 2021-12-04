using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public GameObject creatureCollection;
    public GameObject creaturePrefab;
    public float mutationRate;
    public int numCreatures;
    public float xmin;
    public float xmax;
    public float ymin;
    public float ymax;
    public float epoch;
    public float startTime;

    private bool play= true;
    private Vector2 breedCorner1, breedCorner2;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        List<Genome> newGenomes = new List<Genome>();
        PopulateZoo(newGenomes);
        breedCorner1 = new Vector2(xmax*0.8f,ymax);
        breedCorner2 = new Vector2(xmax,ymin);
    }

    // Update is called once per frame
    void Update()
    {
        if(!play)
            startTime+=Time.deltaTime; // so that the epoch stays the same

        if((Time.time-startTime) > epoch)
        {
            Debug.Log("epoch limit reached");
            List<Genome> newGenomes = new List<Genome>();
            startTime = Time.time;
            for(int i=0;i<creatureCollection.transform.childCount;i++)
            {
                // condittional survival goes here
                GameObject thisCreature = creatureCollection.transform.GetChild(i).gameObject;
                Vector2 creaturePos = new Vector2(thisCreature.transform.position.x,thisCreature.transform.position.y);
                float xSign = Mathf.Sign(breedCorner1.x - creaturePos.x);
                float ySign = Mathf.Sign(breedCorner1.y - creaturePos.y);
                
                Vector2 dist1 = breedCorner1 - creaturePos;
                Vector2 dist2 = breedCorner1 - breedCorner2;

                if(dist1.x*xSign < dist2.x*xSign && dist1.y*ySign < dist2.y*ySign)
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

    public void PauseUnPause()
    {
        play = !play;
        for(int i=0;i<creatureCollection.transform.childCount;i++)
        {
            CreatureController creature = creatureCollection.transform.GetChild(i).GetComponent<CreatureController>();
            creature.PauseUnPause();
        }
    }
    
    public void SetBreedCorners(Vector2 position1, Vector2 position2)
    {
        breedCorner1 = position1;
        breedCorner2 = position2;
    }

    public void SliderUpdate(float value)
    {
        epoch = value;
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
                Genome parent = genomes[i%genomes.Count];
                Genome child = newCreature.GetComponent<CreatureController>().Genome;
                child.Clone(parent);
                for(int j=0;j<child.Genes.Count;j++)
                {
                    float m = Random.value;
                    if(m<=mutationRate)
                        child.Mutate();
                }    
            }
        }
    }
}
