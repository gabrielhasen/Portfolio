using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tile{

	
	public enum TileType { 
		Water, Forest, DarkForest, Abyss,
		Base, 
		Farm1, Farm2, Farm3,			//Supplies food
		House1, House2, House3,			//Increases population
		Barracks						//Trains soldiers
	};

	//Variables
	private World world;
	private Action<Tile> cbTileTypeChanged;
	private TileType type;
	private int y;
	private int x;
	private string name;
	private string word1;
	private int value;
	private int upgrade = 0;
	
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
	public int Value{
		get { return value; }
	}
	public int Upgrade{
		get { return upgrade; }
	}
	public string Name{
		get { return name; }
	}
	public string Word1{
		get { return word1; }
	}


	//Main Functions
	public Tile(World world, int x, int y)
	{
		this.world = world;
		this.x = x;
		this.y = y;
	}

	public void RegisterTileTypeChangedCallback(Action<Tile> callback)
	{
		cbTileTypeChanged = callback;
	}

	public void TypeChange()
	{
		if(Type == TileType.Water)
		{
			name = "Water";
			word1 = "NULL";
		} 
		else if(Type == TileType.Forest)
		{
			name = "Forest";
			word1 = "NULL";
		}
		else if(Type == TileType.DarkForest)
		{
			name = "Dark Forest";
			word1 = "NULL";
		}
		else if(Type == TileType.Abyss)
		{
			name = "Abyss";
			word1 = "Enemies: ";
		}
		else if(Type == TileType.Base)
		{
			name = "Base";
			word1 = "Population: ";
			upgrade = 1;
			value = 5;
		}
		else if(Type == TileType.Farm1)
		{
			name = "Farm";
			word1 = "Food Production: ";
			upgrade = 1;
			value = 5;
		}
		else if(Type == TileType.Farm2)
		{
			name = "Farm";
			word1 = "Food Production: ";
			upgrade = 2;
			value = 10;
		}
	}

}
