using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class CreatureController : MonoBehaviour
{
    public GameObject world;
    [SerializeField] int numGenes=8;
    public float maxSpeed=3.0f;

    public Genome myGenome;
    private (string,string,float)[] myBrain;
    public Dictionary<string, dynamic> myNeurons;

    private float myAge = 0f;
    private float myFood;
    private Vector2 lastPosition;

    private int numSensors;
    private int numInternals;
    private int numMotors;

    private WorldController worldController;
    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;

    private bool play = true;
    private Vector2 storedVelocity;
    

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        worldController = FindObjectOfType<WorldController>();
        myGenome = new Genome(numGenes);
        rigidBody = GetComponent<Rigidbody2D>();
        myNeurons = new Dictionary<string, dynamic>();

        myGenome.Randomize();
        spriteRenderer.color = myGenome.Color();
    }

    // Start is called before the first frame update
    void Start()
    {
        SensePosX Pox = new SensePosX(this,worldController);
        AddNeuron(Pox,"00");
        SensePosY Poy = new SensePosY(this,worldController);
        AddNeuron(Poy,"01");
        SenseAge Sa = new SenseAge(this,worldController);
        AddNeuron(Sa,"02");
        SenseRandom Rnd = new SenseRandom(this,worldController);
        AddNeuron(Rnd,"03");  
        numSensors = 4;

        Neuron in1 = new Neuron(this,worldController);
        AddNeuron(in1,"10");
        Neuron in2 = new Neuron(this,worldController);
        AddNeuron(in2, "11");
        numInternals = 2;

        MoveX Mvx = new MoveX(this,worldController);
        AddNeuron(Mvx, "20");
        MoveY Mvy = new MoveY(this,worldController);
        AddNeuron(Mvy, "21");
        numMotors = 2;
        
        myBrain = Brain();

        //Debug.Log(myGenome.ToString());
    
        /*
        foreach ((string,string,float) dendrite in myBrain)
        {
            Debug.Log(dendrite.Item1+"->"+dendrite.Item2+": "+dendrite.Item3.ToString());
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.color = myGenome.Color();
        rigidBody.rotation = 0f;
        rigidBody.angularVelocity = 0f;
        
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

    void OnMouseDown()
    {
        //world.GetComponent<WorldController>().BrainText.GetComponent<Text>().text = myGenome.ToString();
        
    }

    private void AddNeuron(dynamic _neuron, string _id)
    {
        myNeurons.Add(_id, _neuron);
    }

    private (string,string,float)[] Brain()
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

    public float Age { get => myAge; set => myAge = value; }
    public Genome Genome { get => myGenome; set => myGenome = value; }
    public float Food { get => myFood; set => myFood = value; }
}
