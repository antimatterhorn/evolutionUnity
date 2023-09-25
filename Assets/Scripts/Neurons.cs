using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron
{
    public float value { get;set;}    
    public CreatureController creature { get;set;}
    public WorldController world {get;set;}
    public Rigidbody2D rigidBody {get;set;}
    public KdTree<Transform> foodTree {get;set;}
    public KdTree<Transform> hazardTree {get;set;}

    public Neuron(){}
    public Neuron(CreatureController _creature, WorldController _world)
    {
        creature = _creature;
        world = _world;
        rigidBody = creature.GetComponent<Rigidbody2D>();
        foodTree = _world.foodTree;
        hazardTree = _world.hazardTree;
    }
    public virtual float call()  => (float)System.Math.Tanh((double)value);
}
/* ********************************
 * SENSORY NEURONS
 * ********************************/
 /* Sensory neurons return values between -1 and 1 */
public class Oscillator : Neuron
{
    public Oscillator(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call() => Mathf.Sin(value*Time.time);
}

public class SenseAlive : Neuron
{
    public SenseAlive(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call() => 1.0F;
}

public class SenseAge : Neuron
{
    public SenseAge(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call() => (creature.Age/world.epoch);
}

public class SensePosX : Neuron
{    
    public SensePosX(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call() => 2f*(rigidBody.position.x/(world.xmax-world.xmin));
}

public class SensePosY : Neuron
{
    public SensePosY(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call() => 2f*(rigidBody.position.y/(world.ymax-world.ymin));
}

public class SenseRandom : Neuron
{
    public SenseRandom(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call() => Random.Range(-1f,1f);
}

public class SenseSpeed : Neuron
{
    public SenseSpeed(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call() => rigidBody.velocity.magnitude/(creature.maxSpeed*Mathf.Sqrt(2));
}

public class SenseStuck : Neuron
{
    public SenseStuck(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call()
    {
        float stuck = 1f-rigidBody.velocity.magnitude/(creature.maxSpeed*Mathf.Sqrt(2));
        return (stuck > 0.9f ? 1f : 0);
    }
}

public class SenseFed : Neuron
{
    public SenseFed(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call() => Mathf.Max(creature.Food,1f);
}

public class SenseFoodX : Neuron
{
    public SenseFoodX(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call()
    {
        if(foodTree.ToList().Count > 0)
        {
            Vector3 closeFood = foodTree.FindClosest(creature.transform.position).position;
            Vector3 direction = (closeFood - creature.transform.position).normalized;
            return direction.x;
        }
        else return 0f;
    }
}

public class SenseFoodY : Neuron
{
    public SenseFoodY(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call()
    {
        if(foodTree.ToList().Count > 0)
        {
            Vector3 closeFood = foodTree.FindClosest(creature.transform.position).position;
            Vector3 direction = (closeFood - creature.transform.position).normalized;
            return direction.y;
        }
        else return 0f;
    }
}

public class SenseFoodD : Neuron
{
    public SenseFoodD(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call()
    {
        if(foodTree.ToList().Count > 0)
        {
            Vector3 closeFood = foodTree.FindClosest(creature.transform.position).position;
            Vector3 direction = (closeFood - creature.transform.position).normalized;
            return direction.magnitude;
            // this is not normalized properly for neurons
        }
        else return 0f;
    }
}

public class SenseHazardX : Neuron
{
    public SenseHazardX(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call()
    {
        float str = 0f;
        if(hazardTree.ToList().Count > 0)
        {
            Vector3 closeHazard = hazardTree.FindClosest(creature.transform.position).position;
            Vector3 direction = (closeHazard - creature.transform.position);
            if(direction.magnitude <=2f)
                str = (2f - direction.x)*0.5f;
        }        
        return str;
    }
}

public class SenseHazardY : Neuron
{
    public SenseHazardY(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call()
    {
        float str = 0f;
        if(hazardTree.ToList().Count > 0)
        {
            Vector3 closeHazard = hazardTree.FindClosest(creature.transform.position).position;
            Vector3 direction = (closeHazard - creature.transform.position);
            if(direction.magnitude <=2f)
                str = (2f - direction.y)*0.5f;
        }        
        return str;
    }
}

public class SenseHazardD : Neuron
{
    public SenseHazardD(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call()
    {
        if(hazardTree.ToList().Count > 0)
        {
            Vector3 closeHazard = hazardTree.FindClosest(creature.transform.position).position;
            Vector3 direction = (closeHazard - creature.transform.position);
            return direction.magnitude;
            // this is not normalized properly for neurons
        }        
        else return 0f;
    }
}
/* ********************************
 * MOTOR NEURONS
 * ********************************/
  /* Motor neurons change the state of the creature in some way and return 0 on call() */
public class MoveX : Neuron
{
    public MoveX(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public new void call()
    {
        float speed = Mathf.Min(Mathf.Abs(value),creature.maxSpeed)*Mathf.Sign(value);
        rigidBody.velocity = new Vector2(speed,rigidBody.velocity.y);
    }
}

public class MoveY : Neuron
{
    public MoveY(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public new void call()
    {
        float speed = Mathf.Min(Mathf.Abs(value),creature.maxSpeed)*Mathf.Sign(value);
        rigidBody.velocity = new Vector2(rigidBody.velocity.x,speed);
    }
}