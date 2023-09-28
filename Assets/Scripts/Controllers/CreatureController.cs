using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    public GameObject world;
    public GameObject creaturePrefab;

    public CreatureObject creatureObject;

    public Genome myGenome;
    public NeuronLibrary neuronLibrary;
    private (string,string,float)[] myBrain;

    private float myAge;
    private float myFood;
    private Vector3 lastPosition;
    private float distTraveled;

    private int numSensors;
    private int numInternals;
    private int numMotors;
    private int numGenes;
    [HideInInspector]
    public float maxSpeed;

    private WorldController worldController;
    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;

    private bool play = true;
    private Vector2 storedVelocity;

    //private GameObject creatureCollection;

    private bool updatedSprite = false;
    
    void Awake()
    {
        spriteRenderer  = GetComponent<SpriteRenderer>();
        worldController = FindObjectOfType<WorldController>();

        numInternals    = creatureObject.numInternals;
        numGenes        = creatureObject.numGenes;
        maxSpeed        = creatureObject.maxSpeed;

        myGenome = new Genome(numGenes);
        rigidBody = GetComponent<Rigidbody2D>();

        // this activates for all creatures, but child creatures have their genes overwritten
        myGenome.Randomize(); 
        
        spriteRenderer.sprite   = creatureObject.bodySprite;
        spriteRenderer.color    = myGenome.Color();

        //creatureCollection = GameObject.FindGameObjectWithTag("CreatureCollection");
    }

    void Start()
    {   
        neuronLibrary = new NeuronLibrary(this,worldController,creatureObject.numInternals);
        numSensors = neuronLibrary.numSensors;
        numMotors = neuronLibrary.numMotors;
        
        myBrain = Brain();


    }

    void UpdateSprite()
    {
        SpriteBuilder sb = new SpriteBuilder(spriteRenderer.sprite);
        int count = 0;
        foreach (Phenotype phenotype in creatureObject.phenotypes)
        {
            bool addPart = false;
            foreach (string geneid in phenotype.geneIds)
            {
                foreach ((string,string,float) gene in myBrain)
                {
                    if (neuronLibrary.dictNeurons[gene.Item1] == geneid || neuronLibrary.dictNeurons[gene.Item2] == geneid)
                    {
                        addPart = true;
                        break;
                    }
                }
                if (addPart)
                    break;
            }
            if (addPart)
            {
                sb.Add(phenotype.sprite,phenotype.r,phenotype.theta,phenotype.scale,phenotype.reflected);
                count++;
            }
        }
        spriteRenderer  = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sb.Sprite();
        updatedSprite = true;
    }

    void Update()
    {
        // if (!updatedSprite)
        //     UpdateSprite();
        
        distTraveled += (this.transform.position - lastPosition).magnitude;
        spriteRenderer.color = myGenome.Color();
        
        foreach (KeyValuePair<string,dynamic> neuron in neuronLibrary.Neurons)
        {         
            neuron.Value.value = 0;
        }

        if(play)
        {
            myAge += Time.deltaTime;
            // send dendrites to all neurons
            foreach ((string,string,float) dendrite in myBrain)
            {
                neuronLibrary.Neurons[dendrite.Item2].value += neuronLibrary.Neurons[dendrite.Item1].call()*dendrite.Item3;
            }
            // fire motor neurons
            foreach (KeyValuePair<string,dynamic> neuron in neuronLibrary.Neurons)
            {
                if(neuron.Key[0]=="2"[0])
                    neuron.Value.call();
            }

            if(worldController.radiationHazards.Count > 0)
            {
                foreach (GameObject hazard in worldController.radiationHazards)
                {
                    Vector3 hpos = hazard.transform.position;
                    float dist = (this.transform.position - hpos).magnitude;
                    if(dist<2f)
                    {
                        float a = 10f;
                        float b = -0.5f*Mathf.Log(1f/a,Mathf.Exp(1f));
                        float fac = a*Mathf.Exp(-b*dist);
                        myAge += Time.deltaTime*fac;
                    }
                }
            }

            if(myAge > worldController.epoch)
            //if((myAge > world.GetComponent<WorldController>().epoch) || (distTraveled > 2f*(world.GetComponent<WorldController>().xmax-world.GetComponent<WorldController>().xmin)))
            {
                Destroy(this.gameObject);
            }
        }
        lastPosition = this.transform.position;
    }

    public void Reproduce()
    {
        float xmin = worldController.xmin;
        float ymin = worldController.ymin;
        float xmax = worldController.xmax;
        float ymax = worldController.ymax;

        float creatureScale = worldController.creatureScale;
        /*
        float xpos = Random.Range(xmin*0.9f,xmax*0.9f);
        float ypos = Random.Range(ymin*0.9f,ymax*0.9f);
        */
        float xpos = this.transform.position.x + Random.Range(-0.05f,0.05f);
        float ypos = this.transform.position.y + Random.Range(-0.05f,0.05f);
        Vector2 newPos = new Vector2(xpos,ypos);
        GameObject newCreature = (GameObject)Instantiate(creaturePrefab,newPos,Quaternion.identity);
        newCreature.name = "Child";
        newCreature.GetComponent<CreatureController>().world = world;
        newCreature.transform.parent = this.transform.parent;
        newCreature.transform.localScale = new Vector3(creatureScale,creatureScale,1.0f);

        Genome child = newCreature.GetComponent<CreatureController>().Genome;
        child.Clone(myGenome);
        for(int j=0;j<child.Genes.Count;j++)
        {
            float m = Random.value;
            if(m<=worldController.mutationRate)
                child.Mutate();
        }
    }

    public void PauseUnPause()
    {
        if(play)
        {
            storedVelocity = rigidBody.velocity;
            rigidBody.velocity = new Vector2(0f,0f);
            rigidBody.isKinematic = true;
        }
        else
        {
            rigidBody.isKinematic = false;
            rigidBody.velocity = storedVelocity;
        }       
        play = !play;
    }

    public void OnMouseDown()
    {
        if(!play)
        {
            TextWriter tw = new TextWriter();
            tw.WriteString(RawBrainDump(),"rawbrain.txt");
            tw.WriteString(BrainDump(),"brain.txt");
        }       
    }

    public (string,string,float)[] Brain()
    {
        (string,string,float)[] brainMap = new (string,string,float)[numGenes];
        int i = 0;
        foreach (string gene in myGenome.Genes)
        {
            //(int, int, float) dendrite;
            (int,int) src = myGenome.Source(gene);
            (int,int) snk = myGenome.Sink(gene);
            float str = myGenome.Strength(gene);
            string srcString = "";
            if(src.Item1 == 0)
                srcString = src.Item1.ToString()+(src.Item2 % numSensors).ToString();
            else
                srcString = src.Item1.ToString()+(src.Item2 % numInternals).ToString();
            string snkString = "";
            if(snk.Item1==0)
                snkString = "2"+(snk.Item2 % numMotors).ToString();
            else
                snkString = "1"+(snk.Item2 % numInternals).ToString();
            brainMap[i] = (srcString,snkString,str);
            i++;
        }
        return brainMap;
    }

    private string RawBrainDump()
    {
        string output = "";
        int i = 0;
        foreach (string gene in myGenome.Genes)
        {
            output += gene;
            i++;
        }
        return output;
    }

    private string BrainDump()
    {
        (string,string,float)[] brainMap = Brain();
        string output = "";
        foreach ((string, string, float) wire in brainMap)
        {
            output += neuronLibrary.dictNeurons[wire.Item1]+" "+neuronLibrary.dictNeurons[wire.Item2]+" "+wire.Item3.ToString()+"\n";
        }
        return output;
    }

    public float Age { get => myAge; set => myAge = value; }
    public Genome Genome { get => myGenome; set => myGenome = value; }
    public float Food { get => myFood; set => myFood = value; }
}
