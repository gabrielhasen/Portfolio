using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : RoomGenerator {

	public int openDirection;
	
	private bool spawned = false;
	DungeonGeneration dungeonGeneration;

	void Start()
	{
		dungeonGeneration = DungeonGeneration.instance;
	}

	private void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.tag == "Player")
		{
			//gets every enemy in the game currently
			foreach (GameObject Enem in allEnemies)
			{
				if(Enem == null)
				{

				}
				else
				{
					Enemy_Movement movement = Enem.GetComponent<Enemy_Movement>();
					movement.innactive = false;
				}
			}
		}
	}
}