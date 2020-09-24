/*
 *	GameManager holds all information for other scripts to access
 *	Also handles signals in which other functions get called from.
 *
 *	Handles wait time.  Have if an enemy attacks / player attacks
 *	time gets added to wait time until the next action can be made.
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;	
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {

	public static GameManager instance;
	void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("More than 1 instance of AllCards");
			return;
		}
		instance = this;
	}

	//use this Tilemap for all other scripts to access the tilemap
    public Tilemap walkableTilemap;
    public Tilemap wallTilemap;
	public bool isMoving = false;
	public bool isAction = false;
	public delegate void NextTurn();			//delegate is similar to a signal, as when
	public NextTurn NextTurnCallBack;			//triggered methods listening for this delegate will be called
	public float time;
	public GameObject[] enemyList;
	public List<Vector2> enemyMoves;

	// Use this for initialization
	void Start () 
	{
        time = 0.3f;
	}

	//is invoked after the player moves.  This signals that the enemy is now okay to move and
	//take its actions
	public void TurnChange()
	{
		if (NextTurnCallBack != null)
		{
			enemyList = GameObject.FindGameObjectsWithTag("Enemy");
			enemyMoves.Clear();
			StartCoroutine(waiting());
			isMoving = true;			//triggering delegate
			NextTurnCallBack.Invoke();
		}
	}

	//handles if the player has used an action.  Such as using an attack.
	public void actionChange()
	{
		isAction = true;
		StartCoroutine(waiting());
	}

	//handles the speed that the turns change.  If time is lower then the player
	//and everything else can move faster.
	IEnumerator waiting()
	{
		yield return new WaitForSeconds(time);
		isMoving = false;
		isAction = false;
	}

	public bool checkMoveEnemies(Vector2 targetCell)
	{
		bool alreadyMoving = false;
		bool alreadyEnemy = false;
		foreach (GameObject en in enemyList)
		{
			if(en != null){
				if(targetCell == new Vector2(en.transform.position.x, en.transform.position.y))
					alreadyEnemy = true;
			}
		}
		foreach (Vector2 moveCell in enemyMoves)
		{
			if(moveCell != null)
			{
				if(targetCell == moveCell)
					alreadyMoving = true;
			}
		}
		if(alreadyMoving || alreadyEnemy)
			return false;
		else{
			enemyMoves.Add(targetCell);
			return true;
		}
	}
}
