using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Block{
	
	public enum BlockType { 
		NULL, Indescructible, Destructible, Room, Item, Spawn, SpawnPoint, Boss, MiniBoss
    };

	//Variables
	private World world;
	private Action<Block> cbBlockTypeChanged;
	private BlockType type;
	private int y;
	private int x;
	private string name;
	private float value;
    private float mapArea;
	private BlockObject BlockObject;
	
	public BlockType Type{
		get { return type; }
		set { 
				type = value; 
				if(cbBlockTypeChanged != null) 
					cbBlockTypeChanged(this);
			}
	}

	public int X {
		get { return x; }
	}
	public int Y {
		get { return y; }
	}
	public string Name{
		get { return name; }
	}
	public float Value{
		get{ return value; }
    }
    public float MapArea
    {
        get { return mapArea; }
    }
    public World WorldArea
    {
        get { return world; }
    }


    //Main Functions
    public Block(World world, int x, int y)
	{
		this.world = world;
		this.x = x;
		this.y = y;
	}
	public Block(World world, int x, int y, float value, int area)
	{
		this.world = world;
		this.x = x;
		this.y = y;
		this.value = value;
		this.BlockObject = null;
        this.mapArea = area;
	}

	public void RegisterBlockTypeChangedCallback(Action<Block> callback)
	{
		cbBlockTypeChanged = callback;
	}

	public bool TypeChange()
	{
		BlockObject = null;
		if(Type == BlockType.Destructible)
		{
			name = "Destructible";
		}
		else if(Type == BlockType.Destructible)
		{
			name = "Destructible";
		}
		else if(Type == BlockType.NULL)
		{
			name = "NULL";
		} 
		return false;
	}

	public void setObject(BlockObject _BlockObject)
	{
		BlockObject = _BlockObject;
	}

}
