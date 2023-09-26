using System.Collections.Generic;

public class NeuronCollection
{
    public int numSensors;
    public int numMotors;
    public Dictionary<string, string> dictNeurons;
    public Dictionary<string, dynamic> Neurons;

    private int numInternals;

    public NeuronCollection(){}
    public NeuronCollection(CreatureController _creature, WorldController _world, int _numInternals)
    {
        Neurons = new Dictionary<string, dynamic>();
        dictNeurons = new Dictionary<string, string>();
        PopulateDict(_creature,_world);
        numSensors = Neurons.Count;
        numInternals = _numInternals;
        AddInternals(_creature,_world);
        AddMotors(_creature,_world);
    }

    public void PopulateDict(CreatureController _creature, WorldController _world)
    {
        Neurons.Add("00", new SensePosX(_creature,_world));
        dictNeurons.Add("00","Pox");

        Neurons.Add("01", new SensePosY(_creature,_world));
        dictNeurons.Add("01","Poy");

        Neurons.Add("02", new SenseAge(_creature,_world));
        dictNeurons.Add("02","Age");

        Neurons.Add("03", new SenseRandom(_creature,_world));
        dictNeurons.Add("03","Rnd");

        Neurons.Add("04", new SenseFed(_creature,_world));
        dictNeurons.Add("04","Fed");

        Neurons.Add("05", new SenseFoodX(_creature,_world));
        dictNeurons.Add("05","SFx");

        Neurons.Add("06", new SenseFoodY(_creature,_world));
        dictNeurons.Add("06","SFy");

        Neurons.Add("07", new SenseHazardX(_creature,_world));
        dictNeurons.Add("07","SHx");

        Neurons.Add("08", new SenseHazardY(_creature,_world));
        dictNeurons.Add("08","SHy");

        Neurons.Add("09", new SenseAlive(_creature,_world));
        dictNeurons.Add("09","SAl");    
    }

    public void AddInternals(CreatureController _creature, WorldController _world)
    {
        for (int i = 0; i < numInternals; i++)
        {
            string Name = "N"+i.ToString();
            string id = "1"+i.ToString();
            Neurons.Add(id, new Neuron(_creature,_world));
            dictNeurons.Add(id,Name);
        }
    }

    public void AddMotors(CreatureController _creature, WorldController _world)
    {
        Neurons.Add("20", new MoveX(_creature,_world));
        dictNeurons.Add("20","Mvx");
        Neurons.Add("21", new MoveY(_creature,_world));
        dictNeurons.Add("21","Mvy");
        numMotors = 2;
    }
}
