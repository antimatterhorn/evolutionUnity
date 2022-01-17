using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    public GameObject world;
    public GameObject creaturePrefab;
    public int numGenes=12;
    
    public float maxSpeed=3.0f;

    public Genome myGenome;
    private (string,string,float)[] myBrain;
    public Dictionary<string, dynamic> myNeurons;

    private float myAge = 0f;
    private float myFood;
    private Vector3 lastPosition;
    private float distTraveled;

    private int numSensors;
    public int numInternals=4;
    private int numMotors;

    private WorldController worldController;
    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;

    private bool play = true;
    private Vector2 storedVelocity;

    private Dictionary<string,string> neuronDict;

    private GameObject creatureCollection;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        worldController = FindObjectOfType<WorldController>();
        myGenome = new Genome(numGenes);
        rigidBody = GetComponent<Rigidbody2D>();
        myNeurons = new Dictionary<string, dynamic>();

        myGenome.Randomize();
        spriteRenderer.color = myGenome.Color();
        neuronDict = new Dictionary<string, string>();
        creatureCollection = GameObject.FindGameObjectWithTag("CreatureCollection");
    }

    void Start()
    {   
        myBrain = PopulateBrain();
    }

    void Update()
    {
        distTraveled += (this.transform.position - lastPosition).magnitude;
        spriteRenderer.color = myGenome.Color();
        //rigidBody.rotation = 0f;
        //rigidBody.angularVelocity = 0f;
        
        foreach (KeyValuePair<string,dynamic> neuron in myNeurons)
        {         
            neuron.Value.value = 0;
        }

        if(play)
        {
            myAge += Time.deltaTime;
            // send dendrites to all neurons
            foreach ((string,string,float) dendrite in myBrain)
            {
                myNeurons[dendrite.Item2].value += myNeurons[dendrite.Item1].call()*dendrite.Item3;
            }
            // fire motor neurons
            foreach (KeyValuePair<string,dynamic> neuron in myNeurons)
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

        /*
        float xpos = Random.Range(xmin*0.9f,xmax*0.9f);
        float ypos = Random.Range(ymin*0.9f,ymax*0.9f);
        */
        float xpos = this.transform.position.x + Random.Range(-0.05f,0.05f);
        float ypos = this.transform.position.y + Random.Range(-0.05f,0.05f);
        Vector2 newPos = new Vector2(xpos,ypos);
        GameObject newCreature = (GameObject)Instantiate(creaturePrefab,newPos,Quaternion.identity);
        newCreature.GetComponent<CreatureController>().world = world;
        //newCreature.GetComponent<CircleCollider2D>().radius = 4f;
        newCreature.transform.parent = creatureCollection.transform;
        newCreature.transform.localScale = new Vector3(0.4f,0.4f,1.0f);

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
            tw.WriteString(BrainDump(),"brain.txt");
            //Debug.Log(BrainDump());
        }
        
    }
/*
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other);
        if(other.gameObject.CompareTag("Food"))
        {
            Vector3 foodPos = other.gameObject.transform.position;
            Vector3 thisPos = this.gameObject.transform.position;
            float d1 = (thisPos - foodPos).magnitude;
            float d2 = (thisPos - closestFood).magnitude;
            if(d1 < d2)
                closestFood = foodPos;
        }
    }
*/
    private void AddNeuron(dynamic _neuron, string _id, string _name)
    {
        myNeurons.Add(_id, _neuron);
        neuronDict.Add(_id,_name);
    }

    public (string,string,float)[] PopulateBrain()
    {
        SensePosX Pox = new SensePosX(this,worldController);
        AddNeuron(Pox,"00","Pox");
        SensePosY Poy = new SensePosY(this,worldController);
        AddNeuron(Poy,"01","Poy");
        SenseAge Sa = new SenseAge(this,worldController);
        AddNeuron(Sa,"02","Age");
        SenseRandom Rnd = new SenseRandom(this,worldController);
        AddNeuron(Rnd,"03","Rnd");
        SenseFed SFd = new SenseFed(this,worldController);
        AddNeuron(SFd,"04","Fed");
        SenseFoodX SFx = new SenseFoodX(this,worldController);
        AddNeuron(SFx,"05","SFx");
        SenseFoodY SFy = new SenseFoodY(this,worldController);
        AddNeuron(SFy,"06","SFy");
        SenseHazardX SHx = new SenseHazardX(this,worldController);
        AddNeuron(SHx,"07","SHx");
        SenseHazardY SHy = new SenseHazardY(this,worldController);
        AddNeuron(SHy,"08","SHy");
        SenseAlive SAl = new SenseAlive(this,worldController);
        AddNeuron(SAl,"09","SAl");
        numSensors = 10;

        for (int i = 0; i < numInternals; i++)
        {
            Neuron in1 = new Neuron(this,worldController);
            AddNeuron(in1,"1"+i.ToString(),"N"+i.ToString());
        }

        MoveX Mvx = new MoveX(this,worldController);
        AddNeuron(Mvx, "20","Mvx");
        MoveY Mvy = new MoveY(this,worldController);
        AddNeuron(Mvy, "21","Mvy");
        numMotors = 2;

        return Brain();
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

    private string BrainDump()
    {
        (string,string,float)[] brainMap = Brain();
        string output = "";
        foreach ((string, string, float) wire in brainMap)
        {
            output += neuronDict[wire.Item1]+" "+neuronDict[wire.Item2]+" "+wire.Item3.ToString()+"\n";
        }
        return output;
    }

    public float Age { get => myAge; set => myAge = value; }
    public Genome Genome { get => myGenome; set => myGenome = value; }
    public float Food { get => myFood; set => myFood = value; }
}
