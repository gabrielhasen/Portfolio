using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum roomType
{
	u,d,l,r,ud,lr,udl,udr,udlr
}

public class RoomGenerator : MonoBehaviour {

	public Tilemap walkableMap;
	public Tilemap wallMap;
	public Tile walk;
	public Tile wall;

	public GameObject Enemy;
	public List<GameObject> allEnemies;

	// Floor and room size info
	int floorSizeX;
	int floorSizeY;
	int minX;		//must always be negative
	int maxX;		//must always be positive
	int minY;		//must always be negative
	int maxY;		//must always be positive
	int middleX;
	int middleY;
	int curPosX;
	int curPosY;
	public int directions;
	public bool up; public bool down; public bool left; public bool right;
	public roomType type;

	public void Awake () 
	{
		up = false; down = false; left = false; right = false;
		curPosX = Mathf.RoundToInt(transform.position.x);
		curPosY = Mathf.RoundToInt(transform.position.y);
		floorSizeX = 3;
		floorSizeY = 3;
		minX = -10;
		maxX = 10;
		minY = -8;
		maxY = 8;
		int middleX = (maxX + minX) / 2;
		int middleY = (maxY + minY) / 2;
		int difX = maxX - minX;
		int difY = maxY - minY;
		//DefaultRoom();
	}

	public void DefaultRoom()
	{
		for(int x = minX; x <= maxX; x++)
		{
			for(int y = minY; y <= maxY; y++)
			{
				Vector3Int pos = new Vector3Int(curPosX + x, curPosY + y, 0);
				wallBoarder(pos,x,y);
				walkableMap.SetTile(pos, walk);
			}
		}
	}

	private void wallBoarder(Vector3Int pos, int x, int y)
	{
		if(x == minX || x == maxX || y == minY || y == maxY || x == minX + 1 || x == maxX - 1 || y == minY + 1 || y == maxY - 1)
		{
			wallMap.SetTile(pos, wall);
		}
	}

	private void ReplaceTileWall(Vector3Int pos)
	{
		wallMap.SetTile(pos, null);
		walkableMap.SetTile(pos, walk);
	}

	public void UpWallReplace()
	{
		Vector3Int pos = new Vector3Int(curPosX , curPosY + maxY, 0);
		ReplaceTileWall(pos);
		pos = new Vector3Int(curPosX , curPosY + maxY -1, 0);
		ReplaceTileWall(pos);
	}

	public void DownWallReplace()
	{
		Vector3Int pos = new Vector3Int(curPosX, curPosY + minY, 0);
		ReplaceTileWall(pos);
		pos = new Vector3Int(curPosX , curPosY + minY + 1, 0);
		ReplaceTileWall(pos);
	}

	public void LeftWallReplace()
	{
		Vector3Int pos = new Vector3Int(curPosX + minX, curPosY, 0);
		ReplaceTileWall(pos);
		pos = new Vector3Int(curPosX + minX + 1, curPosY, 0);
		ReplaceTileWall(pos);
	}

	public void RightWallReplace()
	{
		Vector3Int pos = new Vector3Int(curPosX + maxX, curPosY, 0);
		ReplaceTileWall(pos);
		pos = new Vector3Int(curPosX + maxX -1, curPosY, 0);
		ReplaceTileWall(pos);
	}


	public void openUDLR()
	{
		DefaultRoom();
		UpWallReplace();
		DownWallReplace();
		LeftWallReplace();
		RightWallReplace();
		directions = 4;
		up = true;
		down = true;
		left = true;
		right = true;
		type = roomType.udlr;
	}

	public void openU()
	{
		DefaultRoom();
		UpWallReplace();
		directions = 1;
		up = true;
		type = roomType.u;
	}

	public void openD()
	{
		DefaultRoom();
		DownWallReplace();
		directions = 1;
		down = true;
		type = roomType.d;
	}

	public void openL()
	{
		DefaultRoom();
		LeftWallReplace();
		directions = 1;
		left = true;
		type = roomType.l;
	}

	public void openR()
	{
		DefaultRoom();
		RightWallReplace();
		directions = 1;
		right = true;
		type = roomType.r;
	}

	public void openLR()
	{
		DefaultRoom();
		LeftWallReplace();
		RightWallReplace();
		directions = 2;
		left = true;
		right = true;
		type = roomType.lr;
	}

	public void openUD()
	{
		DefaultRoom();
		UpWallReplace();
		DownWallReplace();
		directions = 2;
		up = true;
		down = true;
		type = roomType.ud;
	}

	public void openUDL()
	{
		DefaultRoom();
		UpWallReplace();
		DownWallReplace();
		LeftWallReplace();
		directions = 3;
		up = true;
		down = true;
		left = true;
		type = roomType.udl;
	}

	public void openUDR()
	{
		DefaultRoom();
		UpWallReplace();
		DownWallReplace();
		RightWallReplace();
		directions = 3;
		up = true;
		down = true;
		right = true;
		type = roomType.udr;
	}

	public void treasureRoom(Vector2 pos)
	{
		Vector2 spawn = new Vector2(pos.x - 5, pos.y + 3);
		GameObject temp = Instantiate(Enemy, spawn, Quaternion.identity);
		allEnemies.Add(temp);
		spawn = new Vector2(pos.x - 5, pos.y - 3);
		temp = Instantiate(Enemy, spawn, Quaternion.identity);
		allEnemies.Add(temp);
		spawn = new Vector2(pos.x + 5, pos.y + 3);
		temp = Instantiate(Enemy, spawn, Quaternion.identity);
		allEnemies.Add(temp);
		spawn = new Vector2(pos.x + 5, pos.y - 3);
		temp = Instantiate(Enemy, spawn, Quaternion.identity);
		allEnemies.Add(temp);
	}


}
