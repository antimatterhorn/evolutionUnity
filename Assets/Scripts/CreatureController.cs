using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Dictionary<string,string> neuronDict;
    
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
    }

    void Start()
    {   
        myBrain = PopulateBrain();
    }

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
        if(!play)
        {
            TextWriter tw = new TextWriter();
            tw.WriteString(BrainDump(),"brain.txt");
            //Debug.Log(BrainDump());
        }
        
    }

    private void AddNeuron(dynamic _neuron, string _id)
    {
        myNeurons.Add(_id, _neuron);
    }

    public (string,string,float)[] PopulateBrain()
    {
        SensePosX Pox = new SensePosX(this,worldController);
        AddNeuron(Pox,"00");
        neuronDict.Add("00","Pox");
        SensePosY Poy = new SensePosY(this,worldController);
        AddNeuron(Poy,"01");
        neuronDict.Add("01","Poy");
        SenseAge Sa = new SenseAge(this,worldController);
        AddNeuron(Sa,"02");
        neuronDict.Add("02","Age");
        SenseRandom Rnd = new SenseRandom(this,worldController);
        AddNeuron(Rnd,"03");
        neuronDict.Add("03","Rnd");
        numSensors = 4;

        Neuron in1 = new Neuron(this,worldController);
        AddNeuron(in1,"10");
        neuronDict.Add("10","N0");
        Neuron in2 = new Neuron(this,worldController);
        AddNeuron(in2, "11");
        neuronDict.Add("11","N1");
        numInternals = 2;

        MoveX Mvx = new MoveX(this,worldController);
        AddNeuron(Mvx, "20");
        neuronDict.Add("20","Mvx");
        MoveY Mvy = new MoveY(this,worldController);
        AddNeuron(Mvy, "21");
        neuronDict.Add("21","Mvy");
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
