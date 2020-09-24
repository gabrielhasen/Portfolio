using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;

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
	public Sprite WaterSprite;
	public Sprite WaterDeepSprite;
	public Sprite SandSprite;
	public Sprite GrassLandSprite;
	public Sprite ForestSprite;
	public Sprite DirtSprite;
	public Sprite StoneSprite;
	public Sprite SnowSprite;
	public int mapWidth;
	public int mapHeight;
	public float noiseScale;

	public bool randomSeed;
	public int seed;

	public bool useFalloff;
	public bool useErode;
	public bool useBreakUp;
	public bool generateLargerContinents;
	public bool leftSide;

	public Vector2 offset;

	public int octaves;
	//[Range(0,1)]
	public float persistance;
	public float lacunarity;


	public bool AutoUpdate;

	#endregion

	public World world  { get; protected set; }

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

	void Start () 
	{
		GUIdelete();
		world = new World(mapWidth * 2, mapHeight * 2, new float[mapWidth * 2, mapHeight * 2]);
		world.GenerateContinents(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset, 
								useFalloff, useErode, useBreakUp, generateLargerContinents, leftSide, randomSeed);

		//Creates GameObject for each tile and gives them the correct position
		for(int y = 0; y < world.Height; y++){
			for(int x = 0; x < world.Width; x++){
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
		world.PerlinTiles();
	}

	void OnTileTypeChanged(Tile tile_data, GameObject tile_go)
	{
		bool isObject = tile_data.TypeChange();
		if(isObject && tile_data.TileObjectData != null)
		{
			tile_go.GetComponent<SpriteRenderer>().sprite = tile_data.TileObjectData.Sprite_Obj;
		}
		if(tile_data.Type == Tile.TileType.Water){
			tile_go.GetComponent<SpriteRenderer>().sprite = WaterSprite;
		}
		else if(tile_data.Type == Tile.TileType.WaterDeep){
			tile_go.GetComponent<SpriteRenderer>().sprite = WaterDeepSprite;
		}
		else if(tile_data.Type == Tile.TileType.Sand){
			tile_go.GetComponent<SpriteRenderer>().sprite = SandSprite;
		}
		else if(tile_data.Type == Tile.TileType.GrassLand){
			tile_go.GetComponent<SpriteRenderer>().sprite = GrassLandSprite;
		}
		else if(tile_data.Type == Tile.TileType.Forest){
			tile_go.GetComponent<SpriteRenderer>().sprite = ForestSprite;
		}
		else if(tile_data.Type == Tile.TileType.Dirt){
			tile_go.GetComponent<SpriteRenderer>().sprite = DirtSprite;
		}
		else if(tile_data.Type == Tile.TileType.Stone){
			tile_go.GetComponent<SpriteRenderer>().sprite = StoneSprite;
		}
		else if(tile_data.Type == Tile.TileType.Snow){
			tile_go.GetComponent<SpriteRenderer>().sprite = SnowSprite;
		}
		else if(tile_data.Type == Tile.TileType.NULL){
			tile_go.GetComponent<SpriteRenderer>().sprite = null;
		}
		else{
			Debug.LogError("OnTileTypeChanged:  Unrecognized Type");
		}
	}

	//On validate is called everytime a value is changed in the inspector
	//So can be used to lock values
	private void OnValidate() 
	{
		if(mapWidth < 1) mapWidth = 1;
		if(mapHeight < 1) mapHeight = 1;
		if(lacunarity < 1) lacunarity = 1;
		if(octaves < 0) octaves = 0;
	}
}
