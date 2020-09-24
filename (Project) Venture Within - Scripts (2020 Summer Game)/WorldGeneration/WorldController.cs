using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    #region Singleton
    public static WorldController Instance;
    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
            return;
        }
    }
    #endregion
    #region Variables
    [Header("Items")]
    public int numberOfHealth;

    [Header("Level Generation")]
    public Sprite Stone;
	public Sprite Dirt;
    public Sprite Room;
    public Sprite Treasure;
    public Sprite LightArea;
    public Material TileMaterial;
    public Material UnlitTileMaterial;
    public GameObject RoomDoor;
    public GameObject MiniBossDoor;
    public GameObject BossDoor;
    public GameObject ItemGO;
    public int mapWidth;
	public int mapHeight;
	public float noiseScale;

	public int seed;

	public bool useFalloff;
	public bool useErode;
	public bool useBreakUp;
	public bool generateLargerContinents;
	public bool leftSide;

	public Vector2 offset;

	public int octaves;
	public float persistance;
	public float lacunarity;


	public bool AutoUpdate;
    public bool GenerateRandomSeed;

    public GameObject ChunkGO;

	#endregion

	public World world1 { get; protected set; }
    public World world2 { get; protected set; }
    public World world3 { get; protected set; }


    /// <summary>
    /// Used by inspector button to generate world
    /// </summary>
    public void GUIstart()
	{
        ItemData.Instance.GUILoad();
		Start();
	}

    /// <summary>
    /// Used by inspector button to destroy world
    /// </summary>
    public void GUIdelete()
	{
		while(transform.childCount != 0){
			DestroyImmediate(transform.GetChild(0).gameObject);
		}
        SpawnManager.Instance.spawnPoints.Clear();
	}

    /// <summary>
    /// Creates the first 3 worlds and calls the nosie map generation on all 3 of them.
    /// Will then put them into containers and create the visuals and components for each block
    /// </summary>
	void Start () 
	{
        GUIdelete();
        if (GenerateRandomSeed) {
            seed = Random.Range(1, 999999);
        }
        world1 = new World(mapWidth, mapHeight, new float[mapWidth, mapHeight], 0, numberOfHealth);
        world2 = new World(mapWidth, mapHeight, new float[mapWidth, mapHeight], 1, numberOfHealth);
        world3 = new World(mapWidth, mapHeight, new float[mapWidth, mapHeight], 2, numberOfHealth);
        world1.GenerateVentSection(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset, 
								useFalloff, useErode, useBreakUp, generateLargerContinents, leftSide);
        world2.GenerateVentSection(mapWidth, mapHeight, seed + 1, noiseScale, octaves, persistance, lacunarity, offset,
                                useFalloff, useErode, useBreakUp, generateLargerContinents, leftSide);
        world3.GenerateVentSection(mapWidth, mapHeight, seed + 2, noiseScale, octaves, persistance, lacunarity, offset,
                                useFalloff, useErode, useBreakUp, generateLargerContinents, leftSide);
        CreateWorldContainers();
        GenerateStartRoom();
        PlaceMinimapInfo();
    }

    private void GenerateStartRoom()
    {
        world1.chunks[1, 1].DeleteChunk();
    }

    private void PlaceMinimapInfo()
    {
        MinimapPerWorld(world1, 1);
        MinimapPerWorld(world2, 2);
        MinimapPerWorld(world3, 3);
    }
    private void MinimapPerWorld(World world, int LevelArea)
    {
        GameObject MinimapContainer = new GameObject();
        MinimapContainer.name = LevelArea.ToString() + "_MinimapContainer";
        MinimapContainer.tag = "MinimapTiles";
        MinimapContainer.transform.SetParent(this.transform, true);

        int chunkSize_X = mapWidth / 10;
        int chunkSize_Y = mapHeight / 10;
        Vector2 pos = new Vector2(4.5f, 4.5f);
        for (int y = 0; y < chunkSize_Y; y++) {
            for (int x = 0; x < chunkSize_X; x++) {
                if (LevelArea == 1) pos = new Vector2(4.5f + (10 * x), + 4.5f + ( 10 * y));
                if (LevelArea == 2) pos = new Vector2(4.5f + (10 * x), +4.5f + (10 * y) + (world.Height));
                if (LevelArea == 3) pos = new Vector2(4.5f + (10 * x), +4.5f + (10 * y) + (world.Height * 2));
                GameObject temp = Instantiate(ChunkGO, pos, Quaternion.identity, MinimapContainer.transform);
                MinimapDisplayType(temp, x, y, world);
            }
        }
    }

    private void MinimapDisplayType(GameObject temp, int x, int y, World world)
    {
        ChunkMinimapInfo chunkInfo = temp.GetComponent<ChunkMinimapInfo>();
        if(chunkInfo == null) {
            Debug.LogError("ChunkMinimapInfo is non existing on instantiated chunk gameObject");
            return;
        }
        switch (world.chunks[x,y].Type) {
            case ChunkType.Empty:
                chunkInfo.Type = ChunkType.Empty;
                chunkInfo.SetUnseen();
                break;
            case ChunkType.Door:
                chunkInfo.Type = ChunkType.Door;
                chunkInfo.SetDoor();
                break;
            case ChunkType.Boss:
                chunkInfo.Type = ChunkType.Boss;
                chunkInfo.SetBoss();
                break;
            case ChunkType.MiniBoss:
                chunkInfo.Type = ChunkType.MiniBoss;
                chunkInfo.SetMiniBoss();
                break;
            case ChunkType.Health:
                chunkInfo.Type = ChunkType.Health;
                chunkInfo.SetUnseen();
                break;
            case ChunkType.Bomb:
                chunkInfo.Type = ChunkType.Bomb;
                chunkInfo.SetUnseen();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Creates all the containers for the 3 different worlds
    /// </summary>
    private void CreateWorldContainers()
    {
        GameObject World1_Container = new GameObject();
        World1_Container.name = "World1_Container";
        World1_Container.transform.SetParent(this.transform, true);
        CreateBlocksForWorld(World1_Container, world1, 1);

        GameObject World2_Container = new GameObject();
        World2_Container.name = "World2_Container";
        World2_Container.transform.SetParent(this.transform, true);
        CreateBlocksForWorld(World2_Container, world2, 2);

        GameObject World3_Container = new GameObject();
        World3_Container.name = "World3_Container";
        World3_Container.transform.SetParent(this.transform, true);
        CreateBlocksForWorld(World3_Container, world3, 3);
    }

    /// <summary>
    /// Creates all tiles for each world and places them in proper position
    /// </summary>
    /// <param name="container"> The Gameobject all the tiles are placed under.</param>
    /// <param name="world"> The world to get the tiles in.</param>
    /// <param name="LevelArea"> Where to place the tiles on the Y axis.</param>
    private void CreateBlocksForWorld(GameObject container, World world, int LevelArea)
    {
        for (int y = 0; y < world.Height; y++) {
            for (int x = 0; x < world.Width; x++) {
                Block Block_data = world.GetBlockAt(x, y);

                GameObject Block_go = new GameObject();
                Block_go.name = "Block (" + x + "," + y + ")";

                //Based on what area the world is will place them on the correct Y coordinate
                if (LevelArea == 1) Block_go.transform.position = new Vector3(Block_data.X, Block_data.Y);
                if (LevelArea == 2) Block_go.transform.position = new Vector3(Block_data.X, Block_data.Y + (world.Height) );
                if (LevelArea == 3) Block_go.transform.position = new Vector3(Block_data.X, Block_data.Y + (world.Height * 2) );

                Block_go.transform.SetParent(container.transform, true);
                SpriteRenderer temp = Block_go.AddComponent<SpriteRenderer>();
                temp.material = TileMaterial;

                //lambda (anonymous function)
                Block_data.RegisterBlockTypeChangedCallback((Block) => { OnBlockTypeChanged(Block, Block_go); });
            }
        }
        world.PerlinBlocks();
        world.SetEntrance();
    }

    /// <summary>
    /// Assigned to Block object, RegisterBlockTypeChangedCallback function.  So whenever you change the block type
    /// it also handles the logic of changing the visuals and components of that block type.  Keeping data of blocks
    /// and visuals seperate.
    /// </summary>
    /// <param name="Block_data"> The data storage of a single Block.</param>
    /// <param name="Block_go"> The gameobject (visuals/components) of a block.</param>
	void OnBlockTypeChanged(Block Block_data, GameObject Block_go)
	{
		bool isObject = Block_data.TypeChange();

        //Determines block type to apply proper logic
		if(Block_data.Type == Block.BlockType.Destructible) {
            CreateDestructibleTile(Block_go, Dirt, 2);
        }
		else if(Block_data.Type == Block.BlockType.Indescructible) {
            CreateIndestructibleTile(Block_go, Stone);
        }
        else if (Block_data.Type == Block.BlockType.Room) {
            CreateRoomDoor(Block_go);
        }
        else if (Block_data.Type == Block.BlockType.Boss) {
            CreateBossDoor(Block_go);
        }
        else if (Block_data.Type == Block.BlockType.MiniBoss) {
            CreateMiniBossDoor(Block_go);
        }
        else if (Block_data.Type == Block.BlockType.Item) {
            CreateItem(Block_go, Block_data);
        }
        else if(Block_data.Type == Block.BlockType.SpawnPoint) {
            CreateBlankTile(Block_go, null);
        }
        else if(Block_data.Type == Block.BlockType.Spawn) {
            CreateBlankTile(Block_go, null);
        }
        else if(Block_data.Type == Block.BlockType.NULL){
            CreateBlankTile(Block_go, null);
		}
		else{
			Debug.LogError("OnBlockTypeChanged:  Unrecognized Type");
		}
	}

    private void CreateBlankTile(GameObject block_go, Sprite sprite)
    {
        SpriteRenderer renderer = block_go.GetComponent<SpriteRenderer>();
        renderer.sprite = sprite;

        if (block_go.GetComponent<BoxCollider2D>() != null)
            DestroyImmediate(block_go.GetComponent<BoxCollider2D>());

        //Destoys health on block
        if (block_go.GetComponent<HealthTile>() != null)
            DestroyImmediate(block_go.GetComponent<HealthTile>());

        //block_go.layer = 8;
        block_go.layer = LayerMask.NameToLayer("VisibleArea");
        renderer.sprite = LightArea;
        renderer.material = UnlitTileMaterial;
    }

    /// <summary>
    /// Assign proper sprite and changes a Block_go to be destructible.
    /// </summary>
    /// <param name="block_go"> The gameobject (visuals/components) of a block.</param>
    /// <param name="sprite"> The sprite visual assigned to Block_go.</param>
    /// <param name="healthAmount"> The amount of health the HealthTile script will be assigned.</param>
    private void CreateDestructibleTile(GameObject block_go, Sprite sprite, int healthAmount)
    {
        SpriteRenderer renderer = block_go.GetComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        block_go.tag = "Environment";

        if (block_go.GetComponent<BoxCollider2D>() == null)
            block_go.AddComponent<BoxCollider2D>();

        //Makes tile destructible
        HealthTile HealthBlock;
        if (block_go.GetComponent<HealthTile>() == null)
            HealthBlock = block_go.AddComponent<HealthTile>();
        else
            HealthBlock = block_go.GetComponent<HealthTile>();
        HealthBlock.InitialHealth = healthAmount;
        HealthBlock.DestroyOnDeath = false;
        //HealthBlock.lightMaterial = UnlitTileMaterial;
        renderer.material = TileMaterial;

        block_go.layer = 8;
    }

    /// <summary>
    /// Assign proper sprite and changes a Block_go to be indestructible.
    /// </summary>
    /// <param name="block_go"> The gameobject (visuals/components) of a block.</param>
    /// <param name="sprite"> The sprite visual assigned to Block_go.</param>
    private void CreateIndestructibleTile(GameObject block_go, Sprite sprite)
    {
        SpriteRenderer renderer = block_go.GetComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        block_go.tag = "Environment";

        if (block_go.GetComponent<BoxCollider2D>() == null)
            block_go.AddComponent<BoxCollider2D>();

        //Destoys health on block
        if (block_go.GetComponent<HealthTile>() != null)
            DestroyImmediate(block_go.GetComponent<HealthTile>());

        renderer.material = TileMaterial;
        block_go.layer = 8;
    }

    private void CreateRoomDoor(GameObject block_go)
    {
        block_go.GetComponent<SpriteRenderer>().sprite = null;
        if (block_go.GetComponent<BoxCollider2D>() != null) {
            DestroyImmediate(block_go.GetComponent<BoxCollider2D>());
        }
        Vector3 pos = block_go.transform.position;
        pos.y = pos.y + 0.5f;
        Instantiate(RoomDoor, pos, Quaternion.identity, block_go.transform);
    }

    private void CreateBossDoor(GameObject block_go)
    {
        block_go.GetComponent<SpriteRenderer>().sprite = null;
        if (block_go.GetComponent<BoxCollider2D>() != null) {
            DestroyImmediate(block_go.GetComponent<BoxCollider2D>());
        }
        Vector3 pos = block_go.transform.position;
        pos.y = pos.y + 0.5f;
        Instantiate(BossDoor, pos, Quaternion.identity, block_go.transform);
    }

    private void CreateMiniBossDoor(GameObject block_go)
    {
        block_go.GetComponent<SpriteRenderer>().sprite = null;
        if (block_go.GetComponent<BoxCollider2D>() != null) {
            DestroyImmediate(block_go.GetComponent<BoxCollider2D>());
        }
        Vector3 pos = block_go.transform.position;
        pos.y = pos.y + 0.5f;
        Instantiate(MiniBossDoor, pos, Quaternion.identity, block_go.transform);
    }

    private void CreateItem(GameObject block_go, Block block_data)
    {
        block_go.GetComponent<SpriteRenderer>().sprite = null;

        if (block_go.GetComponent<BoxCollider2D>() != null)
            DestroyImmediate(block_go.GetComponent<BoxCollider2D>());

        if (block_go.GetComponent<HealthTile>() != null)
            DestroyImmediate(block_go.GetComponent<HealthTile>());

        Vector3 pos = block_go.transform.position;
        pos.y = pos.y + 0.5f;

        //Generate items, and if they have all been fulfilled they default to health
        if (block_data.WorldArea.currentNumberOfHealth < block_data.WorldArea.numberOfHealth) {
            GameObject item = Instantiate(ItemData.Instance.GrabHealthPickup(), pos, Quaternion.identity, block_go.transform);
            block_data.WorldArea.currentNumberOfHealth++;
        }
        else {
          GameObject item = Instantiate(ItemData.Instance.GrabBombItem(), pos, Quaternion.identity, block_go.transform);
        }
    }

    /// <summary>
    /// On validate is called everytime a value is changed in the inspector.
    /// Can also be used to lock values.  Keeping values in range they should be in.
    /// </summary>
    private void OnValidate() 
	{
		if(mapWidth < 1) mapWidth = 1;
		if(mapHeight < 1) mapHeight = 1;
		if(lacunarity < 1) lacunarity = 1;
		if(octaves < 0) octaves = 0;
	}
}
