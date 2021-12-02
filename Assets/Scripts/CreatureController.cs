using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    public GameObject world;

    public Genome myGenome;
    public Dictionary<string, dynamic> myNeurons;

    private int myAge;
    private float myFood;
    private Vector2 lastPosition;

    private WorldController worldController;
    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    
    

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        worldController = world.GetComponent<WorldController>();
        myGenome = new Genome();
        rigidBody = GetComponent<Rigidbody2D>();
        myNeurons = new Dictionary<string, dynamic>();

    }

    // Start is called before the first frame update
    void Start()
    {
        myGenome.Randomize();
        
        spriteRenderer.color = myGenome.Color();
        
        SensePosX Sx = new SensePosX(this,worldController);
        AddNeuron(Sx,"00");        
        InternalNeuron in1 = new InternalNeuron(this,worldController);
        AddNeuron(in1,"10");
        InternalNeuron in2 = new InternalNeuron(this,worldController);
        AddNeuron(in2, "11");
        
        foreach (string gene in myGenome.Genes)
        {
            Debug.Log(gene.Substring(16,16));
            Debug.Log(myGenome.Source(gene).ToString() + " " + myGenome.Sink(gene).ToString()
                + "  " +  myGenome.Strength(gene).ToString());
        }
        
        //Debug.Log(mNeurons["00"].call());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddNeuron(dynamic _neuron, string _id)
    {
        myNeurons.Add(_id, _neuron);
    }

    public int Age { get => myAge; set => myAge = value; }
    public Genome Genome { get => myGenome; set => myGenome = value; }
    public float Food { get => myFood; set => myFood = value; }
}
