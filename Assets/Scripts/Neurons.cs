using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron
{
    public float value { get;set;}    
    public CreatureController creature { get;set;}
    public WorldController world {get;set;}
    public Rigidbody2D rigidBody {get;set;}

    public Neuron(){}
    public Neuron(CreatureController _creature, WorldController _world)
    {
        creature = _creature;
        world = _world;
        rigidBody = creature.GetComponent<Rigidbody2D>();
    }
    public virtual float call()  => (float)System.Math.Tanh((double)value);
}
/* ********************************
 * SENSORY NEURONS
 * ********************************/
 /* Sensory neurons return values between -1 and 1 */
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
    public override float call() =>  Random.Range(-1f,1f);
}
/* ********************************
 * MOTOR NEURONS
 * ********************************/
  /* Motor neurons change the state of the creature in some way and return 0 on call() */
public class MoveX : Neuron
{
    public MoveX(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call()
    {
        float speed = Mathf.Min(Mathf.Abs(value),creature.maxSpeed)*Mathf.Sign(value);
        rigidBody.velocity = new Vector2(speed,rigidBody.velocity.y);
        return 0f;
    }
}

public class MoveY : Neuron
{
    public MoveY(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call()
    {
        float speed = Mathf.Min(Mathf.Abs(value),creature.maxSpeed)*Mathf.Sign(value);
        rigidBody.velocity = new Vector2(rigidBody.velocity.x,speed);
        return 0f;
    }
}