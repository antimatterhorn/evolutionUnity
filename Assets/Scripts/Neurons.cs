using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron
{
    public float value { get;set;}    
    public CreatureController creature { get;set;}
    public WorldController world {get;set;}

    public Neuron(){}
    public Neuron(CreatureController _creature, WorldController _world)
    {
        creature = _creature;
        world = _world;
    }
    public virtual float call()  => (float)System.Math.Tanh((double)value);
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
{    private Rigidbody2D rigidBody;

    public SensePosX(CreatureController _creature, WorldController _world)
    {
        creature = _creature;
        world = _world;
        rigidBody = creature.GetComponent<Rigidbody2D>();
    }

    public override float call()
    {
        float xmax = world.xmax;
        float xmin = world.xmin;
        float posx = rigidBody.position.x;

        // this returns +1 if close to right edge
        // -1 if close to left edge
        return 2f*(posx)/(xmax-xmin);
    }
}

public class SensePosY : Neuron
{
    private Rigidbody2D rigidBody;

    public SensePosY(CreatureController _creature, WorldController _world)
    {
        creature = _creature;
        world = _world;
        rigidBody = creature.GetComponent<Rigidbody2D>();
    }

    public override float call()
    {
        float ymax = world.ymax;
        float ymin = world.ymin;
        float posy = rigidBody.position.y;

        return 2f*(posy)/(ymax-ymin);
    }
}

public class SenseRandom : Neuron
{
    public SenseRandom(CreatureController _creature, WorldController _world) : base (_creature, _world) {}
    public override float call() =>  Random.Range(-1f,1f);
}

public class MoveX : Neuron
{
    private Rigidbody2D rigidBody;

    public MoveX(CreatureController _creature, WorldController _world)
    {
        creature = _creature;
        world = _world;
        rigidBody = creature.GetComponent<Rigidbody2D>();
    }

    public override float call()
    {
        float speed = Mathf.Min(Mathf.Abs(value),creature.maxSpeed)*Mathf.Sign(value);
        rigidBody.velocity = new Vector2(speed,rigidBody.velocity.y);
        return 0f;
    }
}

public class MoveY : Neuron
{
    private Rigidbody2D rigidBody;

    public MoveY(CreatureController _creature, WorldController _world)
    {
        creature = _creature;
        world = _world;
        rigidBody = creature.GetComponent<Rigidbody2D>();
    }

    public override float call()
    {
        float speed = Mathf.Min(Mathf.Abs(value),creature.maxSpeed)*Mathf.Sign(value);
        rigidBody.velocity = new Vector2(rigidBody.velocity.x,speed);
        return 0f;
    }
}