using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Creature", menuName = "Scriptable Objects/Creature")]
public class CreatureObject : ScriptableObject
{
    public int numGenes=20;
    public int numInternals=9;
    
    public float maxSpeed=3.0f;

    public Sprite bodySprite;

    public Phenotype[] phenotypes;

    //private int numMotors;
    //private int numSensors;


}
