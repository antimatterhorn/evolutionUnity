using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Creature", menuName = "Creature")]
public class CreatureObject : ScriptableObject
{
    public int numGenes=20;
    public int numInternals=9;
    
    public float maxSpeed=3.0f;

    public Sprite sprite;
    
    //private int numMotors;
    //private int numSensors;


}
