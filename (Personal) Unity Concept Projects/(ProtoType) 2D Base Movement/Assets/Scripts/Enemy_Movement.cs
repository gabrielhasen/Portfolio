/*
 * This script should be inherited by all other
 * enemy scripts
 * 
 * 
 * 
 */
using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy_Movement : MonoBehaviour {

    //tilemaps
    Tilemap walkableTilemap;
    Tilemap wallTilemap;

    public int health = 5;
	public int waitTime;
	public int currentWaitTime;
	private float moveTime = 0.1f;

    //combat variables
    public bool alreadyAction = false;
    public bool innactive = true;
    public int range;
    public int damage = 0;
    public GameObject attackObject;
    public List<GameObject> projectiles;
    public int projectileNumber = 3;

    //other scripts accessed
	Player_Movement playerMove;
	GameManager gameManager;

    //incase of overlap with the player (shouldn't ever happen, but somehow just incase)
    Vector2 previousCell;

	void Start () 
	{
		//Gather cell info
		currentWaitTime = waitTime;
		previousCell = transform.position;
		
		//Gather required info
		playerMove = Player_Movement.instance;
		gameManager = GameManager.instance;
		walkableTilemap = gameManager.walkableTilemap;
		wallTilemap = gameManager.wallTilemap;

		//adds the move function to NextTurnCallBack delegate
		gameManager.NextTurnCallBack += Move;
    }
	
	void Update () 
	{
		playerOverlap();
        if(health < 0)
        {
            this.gameObject.SetActive(false);
            gameManager.NextTurnCallBack -= Move;
        }
    }

	private void Move()
	{
        if(innactive == true)
        {
            return;
        }
        //resets action.  So only one action per movement
        alreadyAction = false;

        //Movement checks;
        if (isMovingTime()) return;

        //Enemy and player checks
        Vector2 playerCell = playerMove.transform.position;
		Vector2 currentCell = transform.position;
		Vector2 moveTo;
		moveTo = enemyAI(playerCell, currentCell);

		//Tilemap checks
		bool isGroundCell = getCell(walkableTilemap, moveTo);
		bool isWallCell = getCell(wallTilemap, moveTo);
        if (isGroundCell && !isWallCell && moveTo != playerCell)
        {
            bool checkOtherEnemies = gameManager.checkMoveEnemies(moveTo);
            if(checkOtherEnemies){
                previousCell = currentCell;
                StartCoroutine(SmoothMovement(moveTo));
            }
            //else it stays at current position
        }
        if(moveTo == playerCell && alreadyAction == false) StartCoroutine(waitingForAttack());
    }

	private Vector2 enemyAI(Vector2 playerCell, Vector2 currentCell)
	{
		Vector2 moveTo;
		//which cell to move to on the x position
		if(currentCell.x < playerCell.x)
		{
			moveTo.x = currentCell.x + 1;
		}
		else if(currentCell.x == playerCell.x)
		{
			moveTo.x = currentCell.x;
		}
		else
		{
			moveTo.x = currentCell.x - 1;
		}
		//checks if wall on x position
		bool isWall = getCell(wallTilemap, new Vector2(moveTo.x, currentCell.y));
		if(isWall)
		{
			moveTo.x = currentCell.x;
		}

		//which cell to move to on the y position
		if(currentCell.y < playerCell.y)
		{
			moveTo.y = currentCell.y + 1;
		}
		else if(currentCell.y == playerCell.y)
		{
			moveTo.y = currentCell.y;
		}
		else
		{
			moveTo.y = currentCell.y - 1;
		}
		// checks if wall on y position
		isWall = getCell(wallTilemap, new Vector2(moveTo.x, moveTo.y));
		if(isWall)
		{
			moveTo.y = currentCell.y;
		}
		return moveTo;
	}
	
    private void combatRange()
    {
        //checks all four directions around the enemy for the player.
        
        Vector2 playerCell = playerMove.transform.position;
        Vector2 currentCell = transform.position;
        Vector2 testCell = currentCell;
        //horizontal positive test
        for (int i = 1; i < range + 1; i++)
        {
            testCell = new Vector2(testCell.x, testCell.y + 1);
            if (getCell(wallTilemap, testCell)) break;
            if (testCell == playerCell)
            {
                alreadyAction = true;
                Debug.Log("y+ in range");
                combat(0, 1);
                return;
            }
        }
        testCell = currentCell;
        for (int i = 1; i < range + 1; i++)
        {
            testCell = new Vector2(testCell.x, testCell.y - 1);
            if (getCell(wallTilemap, testCell)) break;
            if (testCell == playerCell)
            {
                alreadyAction = true;
                Debug.Log("y- in range");
                combat(0, -1);
                return;
            }
        }
        testCell = currentCell;
        for (int i = 1; i < range + 1; i++)
        {
            testCell = new Vector2(testCell.x + 1, testCell.y);
            if (getCell(wallTilemap, testCell)) break;
            if (testCell == playerCell)
            {
                alreadyAction = true;
                Debug.Log("x+ in range");
                combat(1, 0);
                return;
            }
        }
        testCell = currentCell;
        for (int i = 1; i < range + 1; i++)
        {
            testCell = new Vector2(testCell.x - 1, testCell.y);
            if (getCell(wallTilemap, testCell)) break;
            if (testCell == playerCell)
            {
                alreadyAction = true;
                Debug.Log("x- in range");
                combat(-1, 0);
                return;
            }
        }
        testCell = currentCell;
    }

    public virtual void combat(int horizontal, int vertical)
    {
        //finds needed vectors
        Vector2 playerCell = playerMove.transform.position;
        Vector2 currentCell = transform.position;
        Vector2 attackCell = new Vector2(currentCell.x + horizontal, currentCell.y + vertical);

        //instantiate current attackObject
        GameObject temp;
        temp = Instantiate(attackObject, attackCell, Quaternion.identity);
        projectiles.Add(temp);
        temp.GetComponent<Projectile>().setInfo(temp, range, attackCell, horizontal, vertical, damage);
    }
    


    //functions that really shouldn't ever need to be changed
    //pretty easy / self explanitory

    private IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        float inverseMoveTime = 1 / moveTime;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }
        //once moved check for attack

        if (alreadyAction == false) StartCoroutine(waitingForAttack());
    }

    //moves enemy to previous cell if enemy and player overlap
	private void playerOverlap()
	{
		Vector2 playerCell = playerMove.transform.position;
		Vector2 currentCell = transform.position;
		if(playerCell == currentCell)
		{
			transform.position = previousCell;
			previousCell = currentCell;
		}
	}

    private IEnumerator waitingForAttack()
    {
        // wait is to make sure player has moved to the next cell
        yield return new WaitForSeconds(0.0f);
        if (alreadyAction == false)
        {
            combatRange();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Projectile_Player")
        {
            Debug.Log("ENEMY Projectile");
            DamageCalculation(coll);
            coll.gameObject.SetActive(false);
        }
    }

    private void DamageCalculation(Collider2D coll)
    {
        health -= coll.GetComponent<Projectile>().damage;
    }

    //amount of player movements until the enemy can move
    private bool isMovingTime()
    {
        if (currentWaitTime <= 0)
        {
            currentWaitTime = waitTime;
            return false;
        }
        else
        {
            currentWaitTime--;
            return true;
        }
    }

    private TileBase getCell(Tilemap tilemap, Vector2 cellPos)
	{
		TileBase currentTile = tilemap.GetTile(tilemap.WorldToCell(cellPos));
		return currentTile;
	}
}