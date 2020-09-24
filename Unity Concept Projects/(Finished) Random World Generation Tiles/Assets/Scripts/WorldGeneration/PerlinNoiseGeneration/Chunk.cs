using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// all chunks are a 10x10 holder of tiles
public class Chunk
{
	Tile[,] tiles = new Tile[10,10];

	//Chunk Information
	public bool isWater;
	public bool isBuildable;
	public Vector2 buildLocation = new Vector2();

	public Chunk(Tile[,] inputTiles)
	{
		tiles = inputTiles;
		for (int x = 0; x < 10; x++)
		{
			for (int y = 0; y < 10; y++)
			{
				//Debug.Log("Tile Type: " + inputTiles[x,y].Name);
			}
		}
	}

	public void AnalyzeChunk()
	{
		//if shallow water > value && forest > value 2 : is buildable
	}

	public void PrintChunk()
	{
		for (int x = 0; x < 10; x++)
		{
			for (int y = 0; y < 10; y++)
			{
				Debug.Log("Tile Type: " + tiles[x,y].Name);
			}
		}
	}
	public void DeleteChunk()
	{
		for (int x = 0; x < 10; x++)
		{
			for (int y = 0; y < 10; y++)
			{
				tiles[x,y].Type = Tile.TileType.NULL;
			}
		}
	}
}
