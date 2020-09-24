using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockObject 
{
	public enum ObjectType
	{
		town,cave
	}
	//BlockOBJECT WILL BE INHERITED BY OTHER CLASSs

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

	public BlockObject(Sprite _sprite, string _name)
	{
		this.sprite_Obj = _sprite;
		this.name = _name;
	}
}
