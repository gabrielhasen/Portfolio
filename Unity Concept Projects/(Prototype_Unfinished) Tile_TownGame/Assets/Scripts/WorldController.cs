using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldController : MonoBehaviour {

	#region Singleton
	public static WorldController Instance { get; protected set; }
	void Awake(){
		if(Instance != null){
			Debug.LogWarning("More than 1 instance of WorldController");
			return;
		}
		Instance = this;
	}
	#endregion
	#region Sprites
	public Sprite ForestSprite;
	public Sprite WaterSprite;
	public Sprite HomeSprite;
	public Sprite DarkForestSprite;
	public Sprite AbyssSprite;
	public Sprite Farm1Sprite;
	public Sprite Farm2Sprite;
	public Sprite House1Sprite;
	public Sprite House2Sprite;
	#endregion

	public World world { get; protected set; }

	public void GUIstart()
	{
		Start();
	}

	public void GUIdelete()
	{
		while(transform.childCount != 0){
			DestroyImmediate(transform.GetChild(0).gameObject);
		}
	}

	// Use this for initialization
	void Start () 
	{
		GUIdelete();
		//Create a world with Water tiles
		world = new World(13, 9);
		//world = new World(100, 100);

		//Creates GameObject for each tile and gives them the correct position
		for(int x = 0; x < world.Width; x++){
			for(int y = 0; y < world.Height; y++){
				Tile tile_data = world.GetTileAt(x, y);

				GameObject tile_go = new GameObject();
				tile_go.name = "Tile ("+x+","+y+")";
				tile_go.transform.position = new Vector3( tile_data.X, tile_data.Y );
				tile_go.transform.SetParent(this.transform, true);

				tile_go.AddComponent<SpriteRenderer>();

				//lambda (anonymous function)
				tile_data.RegisterTileTypeChangedCallback( (tile) => { OnTileTypeChanged(tile, tile_go); } );
			}
		}
		//world.RandomizeTiles();
		world.GenMap();
	}

	void OnTileTypeChanged(Tile tile_data, GameObject tile_go)
	{
		tile_data.TypeChange();
		if(tile_data.Type == Tile.TileType.Forest){
			tile_go.GetComponent<SpriteRenderer>().sprite = ForestSprite;
		}
		else if(tile_data.Type == Tile.TileType.Water){
			tile_go.GetComponent<SpriteRenderer>().sprite = WaterSprite;
		}
		else if(tile_data.Type == Tile.TileType.Base){
			tile_go.GetComponent<SpriteRenderer>().sprite = HomeSprite;
		}
		else if(tile_data.Type == Tile.TileType.DarkForest){
			tile_go.GetComponent<SpriteRenderer>().sprite = DarkForestSprite;
		}
		else if(tile_data.Type == Tile.TileType.Abyss){
			tile_go.GetComponent<SpriteRenderer>().sprite = AbyssSprite;
		}
		else if(tile_data.Type == Tile.TileType.Farm1){
			tile_go.GetComponent<SpriteRenderer>().sprite = Farm1Sprite;
		}
		else if(tile_data.Type == Tile.TileType.Farm2){
			tile_go.GetComponent<SpriteRenderer>().sprite = Farm2Sprite;
		}
		else{
			Debug.LogError("OnTileTypeChanged:  Unrecognized Type");
		}
	}
}
