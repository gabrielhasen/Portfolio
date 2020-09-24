using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


/*
Need to add 
	ULR Room	(For GenTDown)
	DLR Room	(For GenTUp)

 */
public class DungeonGeneration : MonoBehaviour {

	public static DungeonGeneration instance;
	void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("More than 1 instance of DunegonGeneration");
			return;
		}
		instance = this;
	}

	GameManager gameManager;

	public GameObject room;
	public int maxRooms = 5;
	public List<GameObject> allRooms;
	public List<GameObject> edgeRooms;
	public bool check = false;

	// Use this for initialization
	void Start () 
	{
		gameManager = GameManager.instance;
		createDungeon();
		/*allRooms.Add(temp);
		Vector2 a = new Vector2(transform.position.x + 10, transform.position.y);
		temp = Instantiate(room, a, Quaternion.identity);
		allRooms.Add(temp);*/
		
	}

	private void createDungeon()
	{
		//StartRoom
		Room scriptRoom = GenRoom(transform.position);
		scriptRoom.openUDLR();

		GenRanDung();
		SetRoomTypes();
	}

	private void SetRoomTypes()
	{
		bool hasTresureRoom = false;
		bool hasBossRoom = false;
		foreach (GameObject r in allRooms)
		{
			Room scriptRoom = r.GetComponent<Room>();
			roomType curType = scriptRoom.type;

			//Is an edge room
			if(curType == roomType.u || curType == roomType.d || curType == roomType.l
			  || curType == roomType.r){
				edgeRooms.Add(r);
			}
		}

		int rand = Random.Range(0,edgeRooms.Count);
		Vector2 roomPos = edgeRooms[rand].transform.position;
		edgeRooms[rand].name = "Treasure Room";
		edgeRooms[rand].GetComponent<Room>().treasureRoom(roomPos);
		int rand2 = rand;
		while(rand2 == rand)
		{
			rand2 = Random.Range(0,edgeRooms.Count);
		}
		edgeRooms[rand2].name = "Boss Room";

	}

	private Room GenRoom(Vector2 genPos)
	{
		GameObject temp = Instantiate(room, genPos, Quaternion.identity);
		Room scriptRoom = temp.GetComponent<Room>();
		allRooms.Add(temp);
		return scriptRoom;
	}

	private void GenRanDung()
	{
		GameObject temp;
		int randL = Random.Range(1,5);
		int randR = Random.Range(1,5);
		int randU = Random.Range(1,6);
		int randD = Random.Range(1,6);
		for(int x = -3; x < 4; x++){
			for(int y = -3; y < 4; y++){
				Vector2 genPos = new Vector2(transform.position.x + x * 20, transform.position.y + y * 16);
				
				//Left
				if(randL == 1)
					GenLeftT(x,y, genPos);
				else if(randL == 2)
					GenLeftStraight(x,y, genPos);
				else if(randL == 3)
					GenLeftSingle(x,y, genPos);
				else if(randL == 4)
					GenLeftDouble(x,y, genPos);

				//Right
				if(randR == 1)
					GenRightT(x,y, genPos);
				else if(randR == 2)
					GenRightStraight(x,y, genPos);
				else if(randR == 3)
					GenRightSingle(x,y, genPos);
				else if(randR == 4)
					GenRightDouble(x,y, genPos);

				//Up
				if(randU == 1)
					GenUpT(x,y, genPos);
				else if(randU == 2)
					GenUpStraight(x,y, genPos);
				else if(randU == 3)
					GenUpTBlock(x,y, genPos);
				else if(randU == 4)
					GenUpSingle(x,y, genPos);
				else if(randU == 5)
					GenUpDouble(x,y, genPos);

				//Down
				if(randD == 1)
					GenDownT(x,y, genPos);
				else if(randD == 2)
					GenDownStraight(x,y, genPos);
				else if(randD == 3)
					GenDownTBlock(x,y, genPos);
				else if(randD == 4)
					GenDownSingle(x,y, genPos);
				else if(randD == 5)
					GenDownDouble(x,y, genPos);
			}
		}
	}

	#region Straight
	private void GenLeftStraight(int x, int y, Vector2 genPos)
	{
		if(y == 0 && x < 0){
			if(y == 0  && x == -3){
				Room scriptRoom = GenRoom(genPos);
				scriptRoom.openR();
			}
			else{
				Room scriptRoom = GenRoom(genPos);
				scriptRoom.openLR();
			}
		}
	}
	private void GenRightStraight(int x, int y, Vector2 genPos)
	{
		if(y == 0 && x > 0){
			if(y == 0  && x == 3){
				Room scriptRoom = GenRoom(genPos);
				scriptRoom.openL();
			}
			else{
				Room scriptRoom = GenRoom(genPos);
				scriptRoom.openLR();
			}
		}
	}
	private void GenUpStraight(int x, int y, Vector2 genPos)
	{
		if(x == 0 && y > 0){
			if(x == 0  && y == 3){
				Room scriptRoom = GenRoom(genPos);
				scriptRoom.openD();
			}
			else{
				Room scriptRoom = GenRoom(genPos);
				scriptRoom.openUD();
			}
		}
	}
	private void GenDownStraight(int x, int y, Vector2 genPos)
	{
		if(x == 0 && y < 0){
			if(x == 0  && y == -3){
				Room scriptRoom = GenRoom(genPos);
				scriptRoom.openU();
			}
			else{
				Room scriptRoom = GenRoom(genPos);
				scriptRoom.openUD();
			}
		}
	}
	#endregion
	#region T
	private void GenLeftT(int x, int y, Vector2 genPos)
	{
		if(y == 0 && x < 0){
			if(y == 0  && x == -3){
				Room scriptRoom = GenRoom(genPos);
				scriptRoom.openUDR();
			}
			else{
				Room scriptRoom = GenRoom(genPos);
				scriptRoom.openLR();
			}
		}
		else if( y == 1  && x == -3)
		{
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openD();
		}
		else if( y == -1  && x == -3)
		{
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openU();
		}
	}
	private void GenRightT(int x, int y, Vector2 genPos)
	{
		if(y == 0 && x > 0){
			if(y == 0  && x == 3){
				Room scriptRoom = GenRoom(genPos);
				scriptRoom.openUDL();
			}
			else{
				Room scriptRoom = GenRoom(genPos);
				scriptRoom.openLR();
			}
		}
		else if( y == 1  && x == 3)
		{
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openD();
		}
		else if( y == -1  && x == 3)
		{
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openU();
		}
	}
	private void GenUpT(int x, int y, Vector2 genPos)
	{
		if(x == 0 && y > 0){
			if(x == 0  && y == 3){
				Room scriptRoom = GenRoom(genPos);
				scriptRoom.openUDLR();
			}
			else{
				Room scriptRoom = GenRoom(genPos);
				scriptRoom.openUD();
			}
		}
		else if( y == 3  && x == -1)
		{
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openR();
		}
		else if( y == 3  && x == 1)
		{
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openL();
		}
	}
	private void GenDownT(int x, int y, Vector2 genPos)
	{
		if(x == 0 && y < 0){
			if(x == 0  && y == -3){
				Room scriptRoom = GenRoom(genPos);
				scriptRoom.openUDLR();
			}
			else{
				Room scriptRoom = GenRoom(genPos);
				scriptRoom.openUD();
			}
		}
		else if( y == -3  && x == -1)
		{
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openR();
		}
		else if( y == -3  && x == 1)
		{
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openL();
		}
	}
	#endregion
	#region TBblock
	private void GenUpTBlock(int x, int y, Vector2 genPos)
	{
		Room scriptRoom;
		if(x == 0 && y > 0){
			if(x == 0  && y == 3){
				scriptRoom = GenRoom(genPos);
				scriptRoom.openD();
			}
			else if(x == 0 && y == 2){
				scriptRoom = GenRoom(genPos);
				scriptRoom.openUDL();
			}
			else{
				scriptRoom = GenRoom(genPos);
				scriptRoom.openUD();
			}
		}
		else if( y == 2  && x == -1)
		{
			scriptRoom = GenRoom(genPos);
			scriptRoom.openR();
		}
	}
	private void GenDownTBlock(int x, int y, Vector2 genPos)
	{
		Room scriptRoom;
		if(x == 0 && y < 0){
			if(x == 0  && y == -3){
				scriptRoom = GenRoom(genPos);
				scriptRoom.openU();
			}
			else if(x == 0 && y == -2){
				scriptRoom = GenRoom(genPos);
				scriptRoom.openUDL();
			}
			else{
				scriptRoom = GenRoom(genPos);
				scriptRoom.openUD();
			}
		}
		else if( y == -2  && x == -1)
		{
			scriptRoom = GenRoom(genPos);
			scriptRoom.openR();
		}
	}
	#endregion
	//Gen SmallTblock
	#region Single
	private void GenLeftSingle(int x, int y, Vector2 genPos)
	{
		if(y == 0 && x == -1){
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openR();
		}
	}
	private void GenRightSingle(int x, int y, Vector2 genPos)
	{
		if(y == 0 && x == 1){
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openL();
		}
	}
	private void GenUpSingle(int x, int y, Vector2 genPos)
	{
		if(y == 1 && x == 0){
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openD();
		}
	}
	private void GenDownSingle(int x, int y, Vector2 genPos)
	{
		if(y == -1 && x == 0){
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openU();
		}
	}
	#endregion
	#region Double
	private void GenLeftDouble(int x, int y, Vector2 genPos)
	{
		if(y == 0 && x == -1){
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openLR();
		}
		else if(y == 0 && x == -2){
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openR();
		}
	}
	private void GenRightDouble(int x, int y, Vector2 genPos)
	{
		if(y == 0 && x == 1){
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openLR();
		}
		else if(y == 0 && x == 2){
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openL();
		}
	}
	private void GenUpDouble(int x, int y, Vector2 genPos)
	{
		if(y == 1 && x == 0){
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openUD();
		}
		else if(y == 2 && x == 0){
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openD();
		}
	}
	private void GenDownDouble(int x, int y, Vector2 genPos)
	{
		if(y == -1 && x == 0){
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openUD();
		}
		else if(y == -2 && x == 0){
			Room scriptRoom = GenRoom(genPos);
			scriptRoom.openU();
		}
	}
	#endregion
	
	/*public void getEnemies()
	{
		gameManager.EveryEnemy.Clear();
		foreach(GameObject room in allRooms)
		{
			List<GameObject> enemies;
			enemies = room.GetComponent<Room>().allEnemies;
			foreach(GameObject en in enemies)
			{
				if(en != null)
				{
					gameManager.EveryEnemy.Add(en);	
				}
			}
		}
	}*/

	IEnumerator waiting(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
	}

	private TileBase getCell(GameObject r, int x, int y)
	{
		Vector2 currentCell = r.transform.position;
		Vector2 targetCell = currentCell + new Vector2(x, y);
		Room roomScript = r.GetComponent<Room>();
		TileBase currentTile = roomScript.walkableMap.GetTile(roomScript.walkableMap.WorldToCell(targetCell));
		return currentTile;
	}

}
