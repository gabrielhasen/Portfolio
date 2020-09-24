using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World{

	Tile[,] tiles;
	int width;
	int height;
	public int Width{ get {return width;} }
	public int Height{ get{return height;} }
	public Chunk[,] chunks;

	public World(int width, int height, float[,] value)
	{
		this.width = width;
		this.height = height;
		this.chunks = new Chunk[width/10, height/10];
		tiles = new Tile[width,height];
	}

	// Generated 3 different float[,] noise maps.  Each map is then applied to the generate worlds function to make it into a continent.
	// These 3 noise maps are going to be of size x/y, x/y, x/y*2.  Then these float noise maps are combined into a float[,] of size
	// x*2/y*2 and this is our world of multiple continents.
	public void GenerateContinents(int mapWidth, int mapHeight, int seed, float noiseScale, int octaves, float persistance, float lacunarity, Vector2 offset,
		bool useFalloff, bool useErode, bool useBreakUp, bool generateLargerContinents, bool leftSide, bool randomSeed)
	{
		int random ;
		//Map 1 generation
		if(randomSeed)
			random = Random.Range(0,100000);
		else
			random = seed;
		float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, random, noiseScale + 2, octaves, persistance, lacunarity, offset);
		noiseMap = GenerateWorlds(noiseMap, mapWidth, mapHeight, random, offset, useErode, useFalloff, useBreakUp);

		//Map 2 generation
		if(randomSeed)
			random = Random.Range(0,100000);
		else
			random = seed + 1;
		float[,] noiseMap2 = Noise.GenerateNoiseMap(mapWidth,mapHeight, random, noiseScale + 2, octaves, persistance, lacunarity, offset);
		noiseMap2 = GenerateWorlds(noiseMap2, mapWidth, mapHeight, random, offset, useErode, useFalloff, useBreakUp);

		//Map 3 generation
		if(randomSeed)
			random = Random.Range(0,100000);
		else
			random = seed + 2;
		float[,] noiseMap3 = Noise.GenerateNoiseMap(mapWidth, mapHeight*2, random, noiseScale, octaves, persistance, lacunarity, offset);
		noiseMap3 = GenerateWorlds(noiseMap3, mapWidth, mapHeight * 2, random, offset, useErode, useFalloff, useBreakUp);

		//Map 3 generation
		if(randomSeed)
			random = Random.Range(0,100000);
		else
			random = seed + 3;
		float[,] noiseMapIslands = Noise.GenerateNoiseMap(mapWidth * 2, mapHeight * 2, random, 15, octaves, persistance, lacunarity, offset);
		noiseMapIslands = GenerateWorlds(noiseMapIslands, mapWidth * 2, mapHeight * 2, random, offset, useErode, useFalloff, useBreakUp);

		float[,] noise = new float[mapWidth * 2, mapHeight * 2];
		if(leftSide)
		{
			int add = 0;
			for (int y = 0; y < mapHeight * 2; y++)
			{
				for(int x = 0; x < mapWidth; x++)
				{
					noise[x,y] = noiseMap3[x,y];
				}
			}
			for (int y = 0; y < mapHeight; y++)
			{
				add = 0;
				for(int x = mapWidth; x < mapWidth * 2; x++)
				{
					noise[x,y] = noiseMap2[add,y];
					add++;
				}
			}
			add = 0;
			int addX = 0;
			for (int y = mapHeight; y < mapHeight * 2; y++)
			{
				addX = 0;
				for(int x = mapWidth; x < mapWidth * 2; x++)
				{
					noise[x,y] = noiseMap[addX,add];
					addX++;
				}
				add++;
			}
		}
		else
		{
			int addX = 0;
			for (int y = 0; y < mapHeight; y++)
			{
				for(int x = 0; x < mapWidth; x++)
				{
					noise[x,y] = noiseMap2[x,y];
				}
			}
			int add = 0;
			for (int y = mapHeight; y < mapHeight * 2; y++)
			{
				for(int x = 0; x < mapWidth; x++)
				{
					noise[x,y] = noiseMap[x,add];
				}
				add++;
			}
			add = 0;
			for (int y = 0; y < mapHeight * 2; y++)
			{
				add = 0;
				for(int x = mapWidth; x < mapWidth * 2; x++)
				{
					noise[x,y] = noiseMap3[add,y];
					add++;
				}
			}
		}
		if(!generateLargerContinents)
		{
			setTileValues(noise);
		}
		else
		{
			for (int y = 0; y < mapHeight * 2; y++)
			{
				for (int x = 0; x < mapWidth * 2; x++)
				{
					if(noise[x,y] < 0.4f) 
					{
						noise[x,y] = Mathf.Clamp01(noise[x,y] + noiseMapIslands[x,y]/3f);
					}
				}	
			}
			setTileValues(noise);
		}
	}
	// Creates two different noise maps
	// NoiseMap2 : Breaks up the clean edges of each noise map
	//			 : Will subtract and add to the original noise map
	//
	// NoiseMap3 : Breaks up the original noise map by subtrating large chunks
	private float[,] GenerateWorlds(float[,] noiseMap, int _width, int _height, int seed, Vector2 offset, bool useErode,
		bool useFalloff, bool useBreakUp)
	{
		float[,] falloffMap = FalloffGenerator.GenerateFalloffMap(_width,_height);
		float[,] noiseMap2 = Noise.GenerateNoiseMap(_width,_height, seed, 14, 2, 5, 2, offset);
		float[,] noiseMap3 = Noise.GenerateNoiseMap(_width,_height, seed, 60, 4, 0.1f, 0.1f, offset);
		for (int y = 0; y < _height; y++)
		{
			for (int x = 0; x < _width; x++)
			{
				if(useErode && noiseMap[x,y] > noiseMap2[x,y])
				{
					noiseMap[x,y] -= noiseMap2[x,y]/10;
				}
				if(useFalloff)
				{
					noiseMap[x,y] = Mathf.Clamp01(noiseMap[x,y] - falloffMap[x,y]);
				}
				if(useBreakUp)
				{
					noiseMap[x,y] -= Mathf.Clamp01(noiseMap[x,y] - noiseMap3[x,y]);
				}
				if(useErode && noiseMap[x,y] > noiseMap2[x,y] && noiseMap[x,y] < 0.67)
				{
					noiseMap[x,y] += noiseMap2[x,y]/6;
				}
			}
		}
		return noiseMap;
	}
	private void setTileValues(float[,] value)
	{
		for (int y = 0; y < height; y++){
			for (int x = 0; x < width; x++){
				tiles[x,y] = new Tile(this, x, y, value[x,y]);
			}
		}
		GenerateChunks();
	}
	// Divides the world map into multiples of 10x10 chunks
	//
	// Error:
	// If the world is not divisible by 10 on width and height
	// there will be an error
	private void GenerateChunks()
	{
		int sizeX = width/10;
		int sizeY = height/10;

		for (int j = 0; j < sizeY; j++){
			for (int i = 0; i < sizeX; i++){
				
				Tile[,] tempTiles = new Tile[10,10];

				for (int y = 0; y < 10; y++){
					for (int x = 0; x < 10; x++){
						tempTiles[x,y] = tiles[x + (10 * i),y + (10 * j)];
					}
				}
				
				chunks[i,j] = new Chunk(tempTiles);
			}
		}
	}

	public void PerlinTiles()
	{
		for (int x = 0; x < width; x++){
			for (int y = 0; y < height; y++){
				float TileValue = tiles[x,y].Value;
				if(TileValue <= 0.3f){
					tiles[x,y].Type = Tile.TileType.WaterDeep;
				}
				else if(TileValue <= 0.4f){
					tiles[x,y].Type = Tile.TileType.Water;
				}/*
				else if(TileValue <= 0.434f){
					tiles[x,y].Type = Tile.TileType.Sand;
				}*/
				else if(TileValue <= 0.65f){
					tiles[x,y].Type = Tile.TileType.GrassLand;
				}
				else if(TileValue <= 0.75){
					tiles[x,y].Type = Tile.TileType.Forest;
				}
				else if(TileValue <= 0.83){
					tiles[x,y].Type = Tile.TileType.Dirt;
				}
				else if(TileValue <= 0.9f){
					tiles[x,y].Type = Tile.TileType.Stone;
				}
				else if(TileValue <= 1f){
					tiles[x,y].Type = Tile.TileType.Snow;
				}
			}
		}
		setSand();
	}
	private void setSand()
	{
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				Tile temp = GetTileAt(x,y);
				if(temp != null){
					if(temp.Type == Tile.TileType.GrassLand){
						if(GetTileAround(x, y, Tile.TileType.Water) > 0){
							tiles[x,y].Type = Tile.TileType.Sand;
						}
					}
				}
				
			}
		}
	}

	public void SetTileObject(int x, int y, TileObject _TileObject)
	{
		tiles[x,y].setObject(_TileObject);	
		tiles[x,y].Type = Tile.TileType.Object;
	}

	public Tile GetTileAt(int x, int y)
	{
		if(x > width - 1 || x < 0 || y > height - 1 || y < 0){
			//Debug.LogError("Tile out of range (x,y):  ("+x+","+y+")");
			return null;
		}
		return tiles[x,y];
	}

	public int GetTileAround(int x, int y, Tile.TileType compare)
	{
		int tilesAround = 0;
		Tile temp = GetTileAt(x + 1, y);
		tilesAround += getTileType(temp, compare);
		temp = GetTileAt(x - 1, y);
		tilesAround += getTileType(temp, compare);
		temp = GetTileAt(x, y + 1);
		tilesAround += getTileType(temp, compare);
		temp = GetTileAt(x, y - 1);
		tilesAround += getTileType(temp, compare);
		return tilesAround;
	}
	private int getTileType(Tile temp, Tile.TileType compare)
	{
		if(temp != null){
			if(temp.Type == compare)
				return 1;
		}
		return 0;
	}
}
