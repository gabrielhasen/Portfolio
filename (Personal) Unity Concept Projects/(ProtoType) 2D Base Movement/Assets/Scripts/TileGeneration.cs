using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGeneration : MonoBehaviour {

	//tile info
	public Tilemap walkableMap;
	public Tilemap wallMap;
	public Tile walk;
	public Tile wall;
	//public Tile unbreakableWall;

	//floor and room size info
	int floorSizeX;
	int floorSizeY;
	int minX;
	int maxX;
	int minY;
	int maxY;

	void Start () 
	{
		GenerateMap();
	}

	void GenerateMap()
	{
		floorSizeX = 3;
		floorSizeY = 3;
		minX = -8;
		maxX = 8;
		minY = -6;
		maxY = 6;
		int difX = maxX - minX;
		int difY = maxY - minY;

		for(int x = 0; x <= floorSizeX; x++)
		{
			for(int y = 0; y <= floorSizeY; y++)
			{
				int roomToGenerate = Random.Range(0,4);
				if(roomToGenerate == 0)
				{
					SquareBoarderRoom(x, y, difX, difY);
				}
				else if(roomToGenerate == 1)
				{
					PillarSquareBoarderRoom(x, y, difX, difY);
				}
				else if(roomToGenerate == 2)
				{
					SquareEmptyRoom(x, y, difX, difY);
				}
				else if(roomToGenerate == 3)
				{
					EmptyRoom(x, y, difX, difY);
				}
			}
		}
		MapBoarder();
	}

	void EmptyRoom(int changeX, int changeY, int differenceX, int differenceY)
	{
		int sminX = minX + (changeX * differenceX);
		int smaxX = maxX + (changeX * differenceX);
		int sminY = minY + (changeY * differenceY);
		int smaxY = maxY + (changeY * differenceY);
		int middleX = (smaxX + sminX) / 2;
		int middleY = (smaxY + sminY) / 2;
		for(int x = sminX; x <= smaxX; x++)
		{
			for(int y = sminY; y <= smaxY; y++)
			{
				Vector3Int pos = new Vector3Int(x, y, 0);
				walkableMap.SetTile(pos, walk);
			}
		}
	}

	void SquareEmptyRoom(int changeX, int changeY, int differenceX, int differenceY)
	{
		int sminX = minX + (changeX * differenceX);
		int smaxX = maxX + (changeX * differenceX);
		int sminY = minY + (changeY * differenceY);
		int smaxY = maxY + (changeY * differenceY);
		int middleX = (smaxX + sminX) / 2;
		int middleY = (smaxY + sminY) / 2;
		for(int x = sminX; x <= smaxX; x++)
		{
			for(int y = sminY; y <= smaxY; y++)
			{
				Vector3Int pos = new Vector3Int(x, y, 0);
				if( x == middleX || y == middleY	)
				{
					walkableMap.SetTile(pos, walk);
				}
				else if(x == sminX || x == smaxX || y == sminY || y == smaxY)
				{
					wallMap.SetTile(pos, wall);
				}
				else
				{
					walkableMap.SetTile(pos, walk);
				} 
			}
		}
	}

	void SquareBoarderRoom(int changeX, int changeY, int differenceX, int differenceY)
	{
		int sminX = minX + (changeX * differenceX);
		int smaxX = maxX + (changeX * differenceX);
		int sminY = minY + (changeY * differenceY);
		int smaxY = maxY + (changeY * differenceY);
		int middleX = (smaxX + sminX) / 2;
		int middleY = (smaxY + sminY) / 2;
		for(int x = sminX; x <= smaxX; x++)
		{
			for(int y = sminY; y <= smaxY; y++)
			{
				Vector3Int pos = new Vector3Int(x, y, 0);
				if( x == middleX || y == middleY	)
				{
					walkableMap.SetTile(pos, walk);
				}
				//wall boarder
				else if(x == sminX || x == smaxX || y == sminY || y == smaxY || x == sminX + 1 || x == smaxX - 1 || y == sminY + 1 || y == smaxY - 1)
				{
					wallMap.SetTile(pos, wall);
				}
				else
				{
					walkableMap.SetTile(pos, walk);
				} 
			}
		}
	}

	void PillarSquareBoarderRoom(int changeX, int changeY, int differenceX, int differenceY)
	{
		int sminX = minX + (changeX * differenceX);
		int smaxX = maxX + (changeX * differenceX);
		int sminY = minY + (changeY * differenceY);
		int smaxY = maxY + (changeY * differenceY);
		int middleX = (smaxX + sminX) / 2;
		int middleY = (smaxY + sminY) / 2;
		for(int x = sminX; x <= smaxX; x++)
		{
			for(int y = sminY; y <= smaxY; y++)
			{
				Vector3Int pos = new Vector3Int(x, y, 0);
				if( x == middleX || y == middleY	)
				{
					walkableMap.SetTile(pos, walk);
				}
				//wall boarder
				else if(x == sminX || x == smaxX || y == sminY || y == smaxY || x == sminX + 1 || x == smaxX - 1 || y == sminY + 1 || y == smaxY - 1)
				{
					wallMap.SetTile(pos, wall);
				}
				//pillars
				else if( (x ==sminX + 4 && y == sminY + 4) || (x ==smaxX - 4 && y == sminY + 4) || (x ==sminX + 4 && y == smaxY - 4) || (x ==smaxX - 4 && y == smaxY - 4))
				{
					wallMap.SetTile(pos, wall);
				}
				else
				{
					walkableMap.SetTile(pos, walk);
				} 
			}
		}
	}

	void MapBoarder()
	{
		for(int x = minX; x <= maxX * 7; x++)
		{
			Vector3Int pos = new Vector3Int(x, minY, 0);
			wallMap.SetTile(pos, wall);
			pos = new Vector3Int(x, maxY * 7, 0);
			wallMap.SetTile(pos, wall);
		}
		for(int y = minY; y <= maxY * 7; y++)
		{
			Vector3Int pos = new Vector3Int(minX, y, 0);
			wallMap.SetTile(pos, wall);
			pos = new Vector3Int(maxX * 7, y, 0);
			wallMap.SetTile(pos, wall);
		}
	}
}
