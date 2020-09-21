using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChunkType
{
    Empty, Door, Health, Bomb, Boss, MiniBoss
}

// all chunks are a 10x10 holder of Blocks
public class Chunk
{
    private int MaxWorldHeight;
    private int MaxWorldWidth;
	private Block[,] Blocks = new Block[10,10];

    //Chunk Information
    public ChunkType Type;
    public bool isEdge;
	public Vector2 buildLocation = new Vector2();
    public List<Block> DestuctibleBlocks = new List<Block>();
    public List<Block> IndestructibleBlocks = new List<Block>();
    public List<Block> OpenBlocks = new List<Block>();
    public List<Block> SpawnLocations = new List<Block>();

	public Chunk(Block[,] inputBlocks, int height, int width)
	{
        MaxWorldHeight = height;
        MaxWorldWidth = width;
		Blocks = inputBlocks;

		for (int x = 0; x < 10; x++)
		{
			for (int y = 0; y < 10; y++)
			{
                Block.BlockType type = Blocks[x, y].Type;

                switch (type) {
                    case Block.BlockType.NULL:
                        OpenBlocks.Add(Blocks[x, y]);
                        break;
                    case Block.BlockType.Indescructible:
                        IndestructibleBlocks.Add(Blocks[x, y]);
                        break;
                    case Block.BlockType.Destructible:
                        DestuctibleBlocks.Add(Blocks[x, y]);
                        break;
                    default:
                        break;
                }
            }
		}
        Type = ChunkType.Empty;
	}

	public void GetSpawnLocation()
	{
		//if shallow Destructible > value && forest > value 2 : is buildable
	}

    public void FindSpawnLocations()
    {
        SpawnLocations.Clear();

        for (int y = 0; y < 10; y++) {
            for (int x = 0; x < 10; x++) {
                if (Blocks[x, y].Type == Block.BlockType.NULL) {
                    int open = GetBlockAround(x, y, Block.BlockType.NULL);
                    if (open >= 4) {
                        Blocks[x, y].Type = Block.BlockType.Spawn;
                        SpawnManager.Instance.spawnPoints.Add(Blocks[x,y]);
                        SpawnLocations.Add(Blocks[x, y]);
                    }
                }
            }
        }
    }

	public void PrintChunk()
	{
		for (int x = 0; x < 10; x++)
		{
			for (int y = 0; y < 10; y++)
			{
				Debug.Log("Block Type: " + Blocks[x,y].Name);
			}
		}
	}

	public void DeleteChunk()
	{
		for (int x = 0; x < 10; x++)
		{
			for (int y = 0; y < 10; y++)
			{
				Blocks[x,y].Type = Block.BlockType.NULL;
			}
		}
	}

    public void DeleteDestructibleBlocks()
    {
        for (int i = 0; i < DestuctibleBlocks.Count; i++) {
            DestuctibleBlocks[i].Type = Block.BlockType.NULL;
        }
    }

    public void DeleteIndestructibleBlocks()
    {
        for (int i = 0; i < IndestructibleBlocks.Count; i++) {
            IndestructibleBlocks[i].Type = Block.BlockType.NULL;
        }
    }

    public Chunk FindRoomLocation()
    {

        //Local Coordinate of each block
        List<Vector2> RoomLocations = new List<Vector2>();
        for (int y = 0; y < 10; y++) {
            for (int x = 0; x < 10; x++) {
                int destruct = GetBlockAround(x, y, Block.BlockType.Destructible);
                if(destruct == 3 && y < 8 && x != 9 && x != 0) {
                    Block temp = GetBlockAt(x, y + 1);
                    if(temp.Type == Block.BlockType.NULL) {
                        RoomLocations.Add( new Vector2(x,y) );
                    }
                }
            }
        }

        if (RoomLocations.Count != 0) {
            int randomNumber = Random.Range(0, RoomLocations.Count);

            //y+1 to get the actual position of the room, not the base under it
            Vector2 returnVector = new Vector2(Blocks[ (int)RoomLocations[randomNumber].x, (int)RoomLocations[randomNumber].y].X,
                Blocks[(int)RoomLocations[randomNumber].x, (int)RoomLocations[randomNumber].y].Y + 1);
            buildLocation = returnVector;
            return this;
        }
        return null;
    }

    /// <summary>
    /// Gets Block in Block[,] array.
    /// </summary>
    /// <returns>The Block</returns>
    public Block GetBlockAt(int x, int y)
    {
        if (x > 9 || x < 0 || y > 9 || y < 0) {
            //Debug.LogError("Block out of range (x,y):  ("+x+","+y+")");
            return null;
        }
        return Blocks[x, y];
    }

    /// <summary>
    /// Finds block of certain type around a Block from Block[,] array.
    /// </summary>
    /// <returns>The number of blocks around Block of that type</returns>
    /// <param name="x"> The x position in Block[,].</param>
    /// <param name="y"> The y position in Block[,].</param>
    /// <param name="compare"> Finds type of "compare" around x & y values.</param>
    public int GetBlockAround(int x, int y, Block.BlockType compare)
    {
        int BlocksAround = 0;
        Block temp = GetBlockAt(x + 1, y);
        BlocksAround += getBlockType(temp, compare);
        temp = GetBlockAt(x - 1, y);
        BlocksAround += getBlockType(temp, compare);
        temp = GetBlockAt(x, y + 1);
        BlocksAround += getBlockType(temp, compare);
        temp = GetBlockAt(x, y - 1);
        BlocksAround += getBlockType(temp, compare);
        return BlocksAround;
    }

    /// <summary>
    /// Applies proper checks before looking at Block.
    /// </summary>
    /// <returns>Returns 1 if block is around it of correct type</returns>
    /// <param name="temp"> The Block being looked at.</param>
    /// <param name="compare"> Checking if the temp Block is of type compare.</param>
	private int getBlockType(Block temp, Block.BlockType compare)
    {
        if (temp != null) {
            if (temp.Type == compare)
                return 1;
        }
        return 0;
    }
}
