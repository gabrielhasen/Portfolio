using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
	Block[,] Blocks;
	int width;
	int height;
	public int Width{ get {return width;} }
	public int Height{ get{return height;} }
	public Chunk[,] chunks;
    public int mapArea;

    public int numberOfHealth;
    public int currentNumberOfHealth;

    /// <summary>
    /// Initial creation of the world for dimensions and noise value array
    /// </summary>
	public World(int width, int height, float[,] value, int area, int healthMax)
	{
		this.width = width;
		this.height = height;
		this.chunks = new Chunk[width/10, height/10];
        this.mapArea = area;
		Blocks = new Block[width,height];
        numberOfHealth = healthMax;
	}

    /// <summary>
    /// Creates the noise map
    /// </summary>
	public void GenerateVentSection(int mapWidth, int mapHeight, int seed, float noiseScale, int octaves, float persistance, float lacunarity, Vector2 offset,
		bool useFalloff, bool useErode, bool useBreakUp, bool generateLargerContinents, bool leftSide)
	{
		int random = Random.Range(0,100000);
		float[,] noise = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale + 2, octaves, persistance, lacunarity, offset);
        noise = GenerateWorlds(noise, mapWidth, mapHeight, random, offset, useErode, useFalloff, useBreakUp);
        setBlockValues(noise);
    }

    /// <summary>
    /// Creates two different noise maps
    /// NoiseMap2 : Breaks up the clean edges of each noise map
    /// 		 : Will subtract and add to the original noise map
    /// 
    /// NoiseMap3 : Breaks up the original noise map by subtrating large chunks
    /// </summary>
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

    /// <summary>
    /// Creates initial Block data storage, and assignes values to that Block from the generated noise map.
    /// </summary>
    /// <param name="value"> Array of values to be assigned to each Block from the noise map </param>
	private void setBlockValues(float[,] value)
	{
		for (int y = 0; y < height; y++){
			for (int x = 0; x < width; x++){
				Blocks[x,y] = new Block(this, x, y, value[x,y], mapArea);
			}
		}
	}

    /// <summary>
    /// Divides the world map into multiples of 10x10 chunks
    /// 
    /// Error:
    /// If the world is not divisible by 10 on width and height
    /// there will be an error
    /// </summary>
    private void GenerateChunks()
	{
		int sizeX = width/10;
		int sizeY = height/10;
		for (int j = 0; j < sizeY; j++){
			for (int i = 0; i < sizeX; i++){
				
				Block[,] tempBlocks = new Block[10,10];

				for (int y = 0; y < 10; y++){
					for (int x = 0; x < 10; x++){
						tempBlocks[x,y] = Blocks[x + (10 * i),y + (10 * j)];
					}
				}
				chunks[i,j] = new Chunk(tempBlocks, height, width);
			}
		}
        GenerateRooms();
    }

    private void GenerateRooms()
    {
        List<Chunk> AllRoomLocations = new List<Chunk>();
        int chunkSizeX = width / 10;
        int chunkSizeY = height / 10;
        bool everyOtherAdd = true;

        // shows the chunks to change
        for (int y = 0; y < chunkSizeY; y++) {
            for (int x = 0; x < chunkSizeX; x++) {
                if (mapArea == 0 && x == 1 && y == 1) {
                    break;
                }

                else {
                    Chunk temp = chunks[x, y].FindRoomLocation();
                    if (temp != null) {
                        //if (everyOtherAdd) {
                        AllRoomLocations.Add(temp);
                        everyOtherAdd = false;
                        /*}
                        else {
                            everyOtherAdd = true;
                        }*/
                    }
                }
            }
        }

        //shuffle the list, then go through the list and place objects so not all next to each other
        Shuffle(AllRoomLocations);
        for (int i = 0; i < AllRoomLocations.Count; i++) {
            if(i == 0) {
                if (mapArea == 2) {
                    AllRoomLocations[i].Type = ChunkType.Boss;
                    PlaceRoom(AllRoomLocations[i].buildLocation, Block.BlockType.Boss);
                }
                else {
                    AllRoomLocations[i].Type = ChunkType.MiniBoss;
                    PlaceRoom(AllRoomLocations[i].buildLocation, Block.BlockType.MiniBoss);
                }
            }
            else if(i < 4) {
                AllRoomLocations[i].Type = ChunkType.Door;
                PlaceRoom(AllRoomLocations[i].buildLocation, Block.BlockType.Room);
            }
            else {
                AllRoomLocations[i].Type = ChunkType.Health;
                PlaceItem(AllRoomLocations[i].buildLocation);
            }
        }
        FindSpawnLocations();
    }

    private void FindSpawnLocations()
    {
        int chunkSizeX = width / 10;
        int chunkSizeY = height / 10;
        for (int y = 0; y < chunkSizeY; y++) {
            for (int x = 0; x < chunkSizeX; x++) {
                chunks[x, y].FindSpawnLocations();
            }
        }
    }

    private void PlaceRoom(Vector2 position, Block.BlockType typeBlock)
    {
        int x = (int)position.x;
        int y = (int)position.y;
        Blocks[x, y].Type = Block.BlockType.Indescructible;
        Blocks[x - 1, y].Type = Block.BlockType.Indescructible;
        Blocks[x + 1, y].Type = Block.BlockType.Indescructible;

        Blocks[x, y + 1].Type = typeBlock;
        Blocks[x + 1, y + 1].Type = Block.BlockType.NULL;
        Blocks[x - 1, y + 1].Type = Block.BlockType.NULL;

        Blocks[x, y + 2].Type = Block.BlockType.NULL;
        Blocks[x + 1, y + 2].Type = Block.BlockType.NULL;
        Blocks[x - 1, y + 2].Type = Block.BlockType.NULL;
    }

    private void PlaceItem(Vector2 position)
    {
        int x = (int)position.x;
        int y = (int)position.y;

        Blocks[x, y].Type = Block.BlockType.Item;
        Blocks[x, y + 1].Type = Block.BlockType.NULL;
        Blocks[x, y - 1].Type = Block.BlockType.Indescructible;

        Blocks[x + 1, y].Type = Block.BlockType.NULL;
        Blocks[x - 1, y].Type = Block.BlockType.NULL;

        Blocks[x + 1, y + 1].Type = Block.BlockType.NULL;
        Blocks[x - 1, y + 1].Type = Block.BlockType.NULL;
    }

    public static void Shuffle(List<Chunk> alpha)
    {
        for (int i = 0; i < alpha.Count; i++) {
            Chunk temp = alpha[i];
            int randomIndex = Random.Range(i, alpha.Count);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }
    }

    /// <summary>
    /// Based on value assigned to a Block from the noise map, will assign the correct type to that block.
    /// (Remember when a Block.Type is changed it also calls the WorldController function OnBlockTypeChanged() to update visuals
    ///  this is to keep visuals and data seperate)
    /// </summary>
	public void PerlinBlocks()
	{
		for (int x = 0; x < width; x++){
			for (int y = 0; y < height; y++){
				float BlockValue = Blocks[x,y].Value;
				if(BlockValue <= 0.24f){
					Blocks[x,y].Type = Block.BlockType.Indescructible;
                }
				else if(BlockValue <= 0.46f){
					Blocks[x,y].Type = Block.BlockType.Destructible;
				}
				else if(BlockValue <= 0.69f){
					Blocks[x,y].Type = Block.BlockType.NULL;
				}
				else{
					Blocks[x,y].Type = Block.BlockType.Destructible;
				}
			}
		}
        GenerateChunks();
    }

    /// <summary>
    /// Creates the entrance from the actual vent, to the randomly generated vent section.  Accomplishes
    /// this by deleting a section of tiles to allow acess through the indestrucible Blocks.
    /// </summary>
    public void SetEntrance()
    {
        int middleY = height / 2;
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (y > middleY - 3 && y < middleY + 2 &&
                    x > width - 18) {
                   Blocks[x, y].Type = Block.BlockType.NULL;
                }
            }
        }
    }

    /// <summary>
    /// Gets Block in Block[,] array.
    /// </summary>
    /// <returns>The Block</returns>
    public Block GetBlockAt(int x, int y)
	{
		if(x > width - 1 || x < 0 || y > height - 1 || y < 0){
			//Debug.LogError("Block out of range (x,y):  ("+x+","+y+")");
			return null;
		}
		return Blocks[x,y];
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
		if(temp != null){
			if(temp.Type == compare)
				return 1;
		}
		return 0;
	}

    public void DeleteChunk(int x, int y)
    {
        chunks[x, y].DeleteChunk();
    }

    /*private void setSand()
	{
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				Block temp = GetBlockAt(x,y);
				if(temp != null){
					if(temp.Type == Block.BlockType.GrassLand){
						if(GetBlockAround(x, y, Block.BlockType.Destructible) > 0){
							Blocks[x,y].Type = Block.BlockType.Sand;
						}
					}
				}
				
			}
		}
	}*/
}
