using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile{

	
	public enum TileType { 
		NULL, Water, WaterDeep, Sand, GrassLand, Forest,
		Dirt, Stone, Snow,
		Object
	};

	//Variables
	private World world;
	private Action<Tile> cbTileTypeChanged;
	private TileType type;
	private int y;
	private int x;
	private string name;
	private float value;
	private TileObject tileObject;
	
	public TileType Type{
		get { return type; }
		set { 
				type = value; 
				if(cbTileTypeChanged != null) 
					cbTileTypeChanged(this);
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
	public TileObject TileObjectData {
		get { return tileObject; }
	}


	//Main Functions
	public Tile(World world, int x, int y)
	{
		this.world = world;
		this.x = x;
		this.y = y;
	}
	public Tile(World world, int x, int y, float value)
	{
		this.world = world;
		this.x = x;
		this.y = y;
		this.value = value;
		this.tileObject = null;
	}

	public void RegisterTileTypeChangedCallback(Action<Tile> callback)
	{
		cbTileTypeChanged = callback;
	}

	public bool TypeChange()
	{
		if(Type == TileType.Object)
		{
			name = tileObject.Name;
			return true;
		}
		
		tileObject = null;
		if(Type == TileType.Water)
		{
			name = "Water";
		} 
		else if(Type == TileType.WaterDeep)
		{
			name = "WaterDeep";
		} 
		else if(Type == TileType.Sand)
		{
			name = "Sand";
		} 
		else if(Type == TileType.GrassLand)
		{
			name = "Grass Land";
		}
		else if(Type == TileType.Forest)
		{
			name = "Forest";
		}
		else if(Type == TileType.Dirt)
		{
			name = "Dirt";
		} 
		else if(Type == TileType.Stone)
		{
			name = "Stone";
		} 
		else if(Type == TileType.Snow)
		{
			name = "Snow";
		}
		else if(Type == TileType.NULL)
		{
			name = "NULL";
		} 
		return false;
	}

	public void setObject(TileObject _TileObject)
	{
		tileObject = _TileObject;
	}

}
