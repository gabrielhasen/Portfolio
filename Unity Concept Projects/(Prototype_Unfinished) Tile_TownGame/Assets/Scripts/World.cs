using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World{

	Tile[,] tiles;
	int width;
	int height;
	public int Width{ get {return width;} }
	public int Height{ get{return height;} }
	
	//List of all tiles in the world
	public List<Tile> baseList = new List<Tile>();
	public List<Tile> forestList = new List<Tile>();
	public List<Tile> waterList = new List<Tile>();
	public List<Tile> dForestList = new List<Tile>();
	public List<Tile> abyssList = new List<Tile>();
	public List<Tile> farm1List = new List<Tile>();
	public List<Tile> farm2List = new List<Tile>();
	public List<Tile> farm3List = new List<Tile>();
	public List<Tile> house1List = new List<Tile>();
	public List<Tile> house2List = new List<Tile>();
	public List<Tile> house3List = new List<Tile>();

	public World(int width = 5, int height = 5)
	{
		this.width = width;
		this.height = height;

		tiles = new Tile[width,height];
		for (int x = 0; x < width; x++){
			for (int y = 0; y < height; y++){
				tiles[x,y] = new Tile(this, x, y);
			}
		}

		Debug.Log("World created (x,y):  ("+width+","+height+")");
	}

	public void RandomizeTiles()
	{
		for (int x = 0; x < width; x++){
			for (int y = 0; y < height; y++){
				if(Random.Range(0,2) == 0)
				{
					tiles[x,y].Type = Tile.TileType.Forest;
					forestList.Add(tiles[x,y]);
				}
				else if(Random.Range(0,2) == 0)
				{
					tiles[x,y].Type = Tile.TileType.Forest;
					forestList.Add(tiles[x,y]);
				}
				else
				{
					tiles[x,y].Type = Tile.TileType.Water;
					waterList.Add(tiles[x,y]);
				}
			}
		}
		tiles[3,3].Type = Tile.TileType.Base;
		genDarkForest();
	}

	public void GenMap()
	{
		genForest();
		genRiver();
		setTile(tiles[6,4], Tile.TileType.Base);
		genDarkForest();
		genAbyss();
	}

	public void genAbyss()
	{
		int x = 0;
		int y = 0;
		int rand = Random.Range(0,4);
		if(rand == 0){
			x = 0;
			y = Random.Range(0,height - 1);
		}
		else if(rand == 1){
			x = width - 1;
			y = Random.Range(0,height - 1);
		}
		else if(rand == 2){
			x = Random.Range(0,width - 1);
			y = 0;
		}
		else if(rand == 3){
			x = Random.Range(0,width - 1);
			y = height - 1;
		}

		setTile(tiles[x,y], Tile.TileType.Abyss);
	}

	public void genForest()
	{
		//Generate a map of forest
		for (int x = 0; x < width; x++){
			for (int y = 0; y < height; y++){
				tiles[x,y].Type = Tile.TileType.Forest;
				forestList.Add(tiles[x,y]);
			}
		}
	}

	public void genRiver()
	{
		//Place 3 random water tiles
		for(int i = 0; i < 3; i++)
		{
			int randX = Random.Range(3,Width - 3);
			int randY = Random.Range(3,Height - 3);
			tiles[randX, randY].Type = Tile.TileType.Water;
			waterList.Add(tiles[randX,randY]);
		}
		//Select 1 and make it a river
		int rand = Random.Range(0,waterList.Count);
		Tile waterTile = waterList[rand];

		//Get if it expands l/r or u/d
		// 0 -> up
		// 1 -> down
		// 0 -> right
		// 1 -> left
		int ud = Random.Range(0,2);
		int rl = Random.Range(0,2);
		int xChg = 0;	int yChg = 0;

		//UpDown
		while(true)
		{
			if(waterTile.X + xChg >= width || waterTile.Y + yChg >= height
				|| waterTile.X + xChg < 0 || waterTile.Y + yChg < 0)
				break;
			tiles[waterTile.X + xChg, waterTile.Y + yChg].Type = Tile.TileType.Water;
			forestList.Remove(tiles[waterTile.X + xChg, waterTile.Y + yChg]);
			waterList.Add(tiles[waterTile.X + xChg, waterTile.Y + yChg]);
			if(ud == 0)
				yChg++;
			else 
				yChg--;
			if(Random.Range(0,3) == 0)
				xChg++;
		}

		xChg = 0;	yChg = 0;
		//LeftRight
		while(true)
		{
			if(waterTile.X + xChg >= width || waterTile.Y + yChg >= height
				|| waterTile.X + xChg < 0 || waterTile.Y + yChg < 0)
				break;
			tiles[waterTile.X + xChg, waterTile.Y + yChg].Type = Tile.TileType.Water;
			forestList.Remove(tiles[waterTile.X + xChg, waterTile.Y + yChg]);
			waterList.Add(tiles[waterTile.X + xChg, waterTile.Y + yChg]);
			if(rl == 0)
				xChg++;
			else 
				xChg--;
			if(Random.Range(0,3) == 0)
				yChg++;
		}
	}

	public void genDarkForest()
	{
		int rand = Random.Range(0,forestList.Count - 1);
		forestList[rand].Type = Tile.TileType.DarkForest;
		forestList.Remove(forestList[rand]);
		dForestList.Add(forestList[rand]);
	}

	public Tile GetTileAt(int x, int y)
	{
		if(x > width || x < 0 || y > width || y < 0){
			//Debug.LogError("Tile out of range (x,y):  ("+x+","+y+")");
			return null;
		}
		return tiles[x,y];
	}

	public void setTile(Tile curTile, Tile.TileType curType)
	{
		//loop through all tiletype lists and removes the tile that is currently
		//there from the list it is in, and then sets the curTile to the curType
		//list
		if(curTile == null || curType == null){
			return;
		}



		//Remove curTile from correct list
		if(curTile.Type == Tile.TileType.Base){
			Debug.Log("Cant change Base tile");
			return;
		}
		else if(curTile.Type == Tile.TileType.Forest){
			forestList.Remove(curTile);
		}
		else if(curTile.Type == Tile.TileType.Water){
			waterList.Remove(curTile);
		}
		else if(curTile.Type == Tile.TileType.DarkForest){
			dForestList.Remove(curTile);
		}
		else if(curTile.Type == Tile.TileType.Abyss){
			abyssList.Remove(curTile);
		}
		else if(curTile.Type == Tile.TileType.Farm1){
			farm1List.Remove(curTile);
		}
		else if(curTile.Type == Tile.TileType.Farm2){
			farm2List.Remove(curTile);
		}
		else if(curTile.Type == Tile.TileType.Farm3){
			farm3List.Remove(curTile);
		}
		else if(curTile.Type == Tile.TileType.House1){
			house1List.Remove(curTile);
		}
		else if(curTile.Type == Tile.TileType.House2){
			house2List.Remove(curTile);
		}
		else if(curTile.Type == Tile.TileType.House3){
			house3List.Remove(curTile);
		}

		//Add curTile to correct list
		if(curType == Tile.TileType.Base){
			baseList.Add(curTile);
			curTile.Type = Tile.TileType.Base;
		}
		else if(curType == Tile.TileType.Forest){
			forestList.Add(curTile);
			curTile.Type = Tile.TileType.Forest;
		}
		else if(curType == Tile.TileType.Water){
			waterList.Add(curTile);
			curTile.Type = Tile.TileType.Water;
		}
		else if(curType == Tile.TileType.DarkForest){
			dForestList.Add(curTile);
			curTile.Type = Tile.TileType.DarkForest;
		}
		else if(curType == Tile.TileType.Abyss){
			abyssList.Add(curTile);
			curTile.Type = Tile.TileType.Abyss;
		}
		else if(curType == Tile.TileType.Farm1){
			farm1List.Add(curTile);
			curTile.Type = Tile.TileType.Farm1;
		}
		else if(curType == Tile.TileType.Farm2){
			farm2List.Add(curTile);
			curTile.Type = Tile.TileType.Farm2;
		}
		else if(curType == Tile.TileType.Farm3){
			farm2List.Add(curTile);
			curTile.Type = Tile.TileType.Farm3;
		}
		else if(curTile.Type == Tile.TileType.House1){
			house1List.Add(curTile);
			curTile.Type = Tile.TileType.House1;
		}
		else if(curTile.Type == Tile.TileType.House2){
			house2List.Remove(curTile);
			curTile.Type = Tile.TileType.House2;
		}
		else if(curTile.Type == Tile.TileType.House3){
			house3List.Remove(curTile);
			curTile.Type = Tile.TileType.House3;
		}
	}


}
