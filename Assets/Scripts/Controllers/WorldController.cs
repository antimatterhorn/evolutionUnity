using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public GameObject creatureCollection;
    public GameObject creaturePrefab;
    public GameObject foodCollection;
    public GameObject foodPrefab;
    public GameObject radiationCollection;
    public GameObject radiationPrefab;
    public GameObject lens;
    public float mutationRate;
    public int numCreatures;
    public float creatureScale;
    public float foodRate;
    public float xmin;
    public float xmax;
    public float ymin;
    public float ymax;
    public float epoch;
    public float startTime;
    public float foodTime;

    public bool play= true;
    private Vector2 breedCorner1, breedCorner2;

    public List<GameObject> radiationHazards;

    public KdTree<Transform> foodTree = new KdTree<Transform>();
    public KdTree<Transform> hazardTree = new KdTree<Transform>();

    private float mouseDownTime;
    private float mouseWait = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        List<Genome> newGenomes = new List<Genome>();
        foodTree = new KdTree<Transform>();
        PopulateZoo(newGenomes);
        PopulateFood();
        breedCorner1 = new Vector2(xmin/2,ymax/2);
        breedCorner2 = new Vector2(xmax/2,ymin/2);

        lens.SetActive(false);
        mouseDownTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
       
        if(!play)
        {
            startTime+=Time.deltaTime; // so that the epoch stays the same \
            foodTime +=Time.deltaTime;
        }
            
        if((Time.time-foodTime) > 1f/foodRate)
        {
            PopulateFood();
        }
        if (Input.GetMouseButtonDown(0) && play && (Time.time - mouseDownTime)>mouseWait)
        {
            Vector3 mouseLoc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(InWorld(mouseLoc))
            {
                mouseLoc.z = 0;
                GameObject newRadiation = (GameObject)Instantiate(radiationPrefab,mouseLoc,Quaternion.identity);
                newRadiation.transform.parent = radiationCollection.transform;
                mouseDownTime = Time.time;
                hazardTree.Add(newRadiation.transform);
                radiationHazards.Add(newRadiation); 
            }

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
            //newCreature.GetComponent<CircleCollider2D>().radius = 4f;
            newCreature.transform.parent = creatureCollection.transform;
            newCreature.transform.localScale = new Vector3(creatureScale,creatureScale,1.0f);
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

    void PopulateFood()
    {         
        float xpos = Random.Range(xmin*0.9f,xmax*0.9f);
        float ypos = Random.Range(ymin*0.9f,ymax*0.9f);
        Vector2 newPos = new Vector2(xpos,ypos);
        GameObject newFood = (GameObject)Instantiate(foodPrefab,newPos,Quaternion.identity);
        newFood.transform.parent = foodCollection.transform;
        foodTree.Add(newFood.transform);
        foodTime = Time.time;
    }

    public bool InWorld(Vector2 position)
    {
        if(position.x < xmax && position.x > xmin && position.y < ymax && position.y > ymin)
            return true;
        else
            return false;
    }

    public bool InBox(Vector2 position)
    {
        float xSign = Mathf.Sign(breedCorner1.x - position.x);
        float ySign = Mathf.Sign(breedCorner1.y - position.y);
        Vector2 dist1 = breedCorner1 - position;
        Vector2 dist2 = breedCorner1 - breedCorner2;

        if((dist1.x*xSign < dist2.x*xSign && dist1.y*ySign < dist2.y*ySign))
            return true;
        else
            return false;
    }
}
