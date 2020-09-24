using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject 
{
	public enum ObjectType
	{
		town,cave
	}
	//TILEOBJECT WILL BE INHERITED BY OTHER CLASSs

	private ObjectType type;
	private Sprite sprite_Obj;
	private string name;

	public Sprite Sprite_Obj {
		get { return sprite_Obj; }
	}
	public string Name{
		get { return name; }
	}
	public ObjectType Type{
		get { return type; }
	}

	public TileObject(Sprite _sprite, string _name)
	{
		this.sprite_Obj = _sprite;
		this.name = _name;
	}
}
