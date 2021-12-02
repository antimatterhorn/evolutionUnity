using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InternalNeuron
{
    private float mValue = 0F;
    private CreatureController mCreature;
    private WorldController mWorld;

    public InternalNeuron(CreatureController _creature, WorldController _world)
    {
        mCreature = _creature;
        mWorld = _world;
    }

    public float value { get => mValue; set => mValue = value;}
    public float call()  => (float)System.Math.Tanh((double)mValue);
    public void input(float value)
    {
        mValue = value;
    }
}

public class SenseAlive
{
    private CreatureController mCreature;
    private WorldController mWorld;

    public SenseAlive(CreatureController _creature, WorldController _world)
    {
        mCreature = _creature;
        mWorld = _world;
    }

    public float call() => 1.0F;
}

public class SenseAge
{
    private CreatureController mCreature;
    private WorldController mWorld;

    public SenseAge(CreatureController _creature, WorldController _world)
    {
        mCreature = _creature;
        mWorld = _world;
    }

    public float call() => mCreature.Age;
}

public class SensePosX
{
    private CreatureController mCreature;
    private WorldController mWorld;
    private Rigidbody2D rigidBody;

    public SensePosX(CreatureController _creature, WorldController _world)
    {
        mCreature = _creature;
        mWorld = _world;
        rigidBody = mCreature.GetComponent<Rigidbody2D>();
    }

    public float call()
    {
        float xmax = mWorld.xmax;
        float xmin = mWorld.xmin;
        float posx = rigidBody.position.x;

        // this returns +1 if close to right edge
        // -1 if close to left edge
        return 2f*(posx)/(xmax-xmin);
    }
}

public class SensePosY
{
    private CreatureController mCreature;
    private WorldController mWorld;
    private Rigidbody2D rigidBody;

    public SensePosY(CreatureController _creature, WorldController _world)
    {
        mCreature = _creature;
        mWorld = _world;
        rigidBody = mCreature.GetComponent<Rigidbody2D>();
    }

    public float call()
    {
        float ymax = mWorld.ymax;
        float ymin = mWorld.ymin;
        float posy = rigidBody.position.y;

        return 2f*(posy)/(ymax-ymin);
    }
}

public class SenseRandom
{
    public SenseRandom(CreatureController _creature, WorldController _world)
    {

    }

    public float call()
    {
        return Random.Range(-1f,1f);
    }
}

public class MoveX
{
    private CreatureController mCreature;
    private WorldController mWorld;
    private Rigidbody2D rigidBody;

    private float mValue = 0F;

    public MoveX(CreatureController _creature, WorldController _world)
    {
        mCreature = _creature;
        mWorld = _world;
        rigidBody = mCreature.GetComponent<Rigidbody2D>();
    }

    public float value { get => mValue; set => mValue = value;}
    public void call()
    {
        rigidBody.velocity = new Vector2(mValue,rigidBody.velocity.y);
    }

    public void input(float value)
    {
        mValue = value;
    }
}

public class MoveY
{
    private CreatureController mCreature;
    private WorldController mWorld;
    private Rigidbody2D rigidBody;

    private float mValue = 0F;

    public MoveY(CreatureController _creature, WorldController _world)
    {
        mCreature = _creature;
        mWorld = _world;
        rigidBody = mCreature.GetComponent<Rigidbody2D>();
    }

    public float value { get => mValue; set => mValue = value;}
    public void call()
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x,mValue);
    }

    public void input(float value)
    {
        mValue = value;
    }
}