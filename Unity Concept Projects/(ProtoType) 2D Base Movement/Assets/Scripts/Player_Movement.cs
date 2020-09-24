using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;	
using UnityEngine.SceneManagement;

public class Player_Movement : MonoBehaviour {

	public static Player_Movement instance;
	void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("More than 1 instance of Player_Movement");
			return;
		}
		instance = this;
	}

    //tilemaps
	Tilemap walkableTilemap;
	Tilemap wallTilemap;

	public GameObject Range;
	public GameObject[] enemyList;
	public GameObject[] rangeList;
    public GameObject attackObject;
    public Vector2 endSpellRange;
    public bool isAttacking = false;
    public float moveTime = 0.1f;
    public int attackHor = 0;
    public int attackVert = 0;
    public Spell currentSpell;
    public GameObject Highlight;

    //other scripts
	GameManager gameManager;
    Player_Information playerInfo;
	Spell_Bar spellBar;
    Spell_Bar_UI spellUI;

    // Use this for initialization
    void Start () 
	{
		spellBar = Spell_Bar.instance;
		gameManager = GameManager.instance;
        playerInfo = Player_Information.instance;
        spellUI = Spell_Bar_UI.instance;
		enemyList = GameObject.FindGameObjectsWithTag("Enemy");
		walkableTilemap = gameManager.walkableTilemap;
		wallTilemap = gameManager.wallTilemap;
	}
	
	// Update is called once per frame
	void Update () 
	{
        //won't be able to move if moving and actions are taking place
		if(gameManager.isMoving || gameManager.isAction) return;
		movementCheck();

        //starts the attack logic when enter is pressed
        if (Input.GetKeyDown(KeyCode.Return) && isAttacking)
        {
            attacking();
        }

        //this whole if/else string is for finding the spell pressed on the hotbar from 1 to =
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(spellBar.spells.Count < 1) return;
            setSpell(spellBar.spells[0]);
            spellUI.UpdateHighlight(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(spellBar.spells.Count < 2) return;
            setSpell(spellBar.spells[1]);
            spellUI.UpdateHighlight(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if(spellBar.spells.Count < 3) return;
            setSpell(spellBar.spells[2]);
            spellUI.UpdateHighlight(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if(spellBar.spells.Count < 4) return;
            setSpell(spellBar.spells[3]);
            spellUI.UpdateHighlight(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if(spellBar.spells.Count < 5) return;
            setSpell(spellBar.spells[4]);
            spellUI.UpdateHighlight(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if(spellBar.spells.Count < 6) return;
            setSpell(spellBar.spells[5]);
            spellUI.UpdateHighlight(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {

            if(spellBar.spells.Count < 7) return;
            setSpell(spellBar.spells[6]);
            spellUI.UpdateHighlight(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {

            if(spellBar.spells.Count < 8) return;
            setSpell(spellBar.spells[7]);
            spellUI.UpdateHighlight(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if(spellBar.spells.Count < 9) return;
            setSpell(spellBar.spells[8]);
            spellUI.UpdateHighlight(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if(spellBar.spells.Count < 10) return;
            setSpell(spellBar.spells[9]);
            spellUI.UpdateHighlight(9);
        }        
        else if (Input.GetKeyDown(KeyCode.Minus))
        {
            if(spellBar.spells.Count < 11) return;
            setSpell(spellBar.spells[10]);
            spellUI.UpdateHighlight(10);
        }
        else if (Input.GetKeyDown(KeyCode.Plus))
        {
            if(spellBar.spells.Count < 12) return;
            setSpell(spellBar.spells[11]);
            spellUI.UpdateHighlight(11);
        }

        //checks keypress for displaying attack range of currently selected spell
        actionCheck();
	}

    private void attacking()
    {
        if(currentSpell == null)
        {
            return;
        }

        //finds needed vectors
        Vector2 currentCell = transform.position;
        Vector2 attackCell = new Vector2(currentCell.x + attackHor, currentCell.y + attackVert);

        //Attack straight spell
        if(currentSpell.spellType == 0)
        {
            GameObject temp;
            temp = Instantiate(attackObject, currentCell, Quaternion.identity);
            temp.GetComponent<Projectile>().setInfo(temp, currentSpell.range, currentCell, attackHor, attackVert, currentSpell.damage);
        }
        //Movement spell
        else if(currentSpell.spellType == 1)
        {
            if(playerInfo.mana <= 0) return;
            transform.position = endSpellRange;
            playerInfo.updateMana(-1 * currentSpell.manaCost);
        }

        //this turn change is here so the projectile can move once fired and not stack ontop of eachother.  As well as
        //allowing the enemy to move.
        gameManager.TurnChange();
    }

	private void Move(int xDir, int yDir)
	{
		destroyRangeList();

        //finds needed vectors
		Vector2 currentCell = transform.position;
		Vector2 targetCell = currentCell + new Vector2(xDir, yDir);

        //finds required cell information
		bool isGroundCell = getCell(walkableTilemap, targetCell);
		bool isWallCell = getCell(wallTilemap, targetCell);
		bool isEnemy = checkEnemyLocations(targetCell);
		if(isGroundCell && !isWallCell && !isEnemy)
		{
			//transform.position = targetCell;
			StartCoroutine(SmoothMovement(targetCell));
		}
	}

	private void movementCheck()
	{
        //To store move directions.
        int horizontal = 0;
        int vertical = 0;
        //To get move directions
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));	
        
		//We can't go in both directions at the same time
        if ( horizontal != 0 )
            vertical = 0;
			
        //If there's a direction, we are trying to move.
        if (horizontal != 0 || vertical != 0)
        {
            Move(horizontal, vertical);
        }
	}

	private void displayAttackRange(int horizontal, int vertical)
	{
        if(currentSpell == null)
        {
            return;
        }
        //so the old display will be destroyed
		destroyRangeList();

        attackHor = horizontal;
        attackVert = vertical;

        //can only attack when display attack range is showing
        isAttacking = true;

		gameManager.actionChange();
		Vector2 currentCell = new Vector2(transform.position.x + horizontal, transform.position.y + vertical);

        //display the range of the current spell
		rangeList = new GameObject[currentSpell.range];

        //Attack straight spell
        if(currentSpell.spellType == 0)
        {    
            Debug.Log("Attack Spell");
            for(int i = 1; i <= currentSpell.range; i++)
            {
                if(getCell(wallTilemap,currentCell)) return;
                rangeList[i-1] = Instantiate(Range, currentCell, Quaternion.identity);
                currentCell = new Vector2(currentCell.x + horizontal, currentCell.y + vertical);
            }
        }
        //Movement spell
        else if(currentSpell.spellType == 1)
        {
            Debug.Log("Movement Spell");
            Vector2 previousCell = currentCell;
            for(int i = 1; i <= currentSpell.range; i++)
            {
                if(getCell(wallTilemap,currentCell)) 
                {
                    if(previousCell != new Vector2(transform.position.x, transform.position.y))
                    {
                        rangeList[i-1] = Instantiate(Range, previousCell, Quaternion.identity);
                        endSpellRange = previousCell;
                        return;
                    }
                    return;
                }
                if(i == currentSpell.range)
                {
                    rangeList[i-1] = Instantiate(Range, currentCell, Quaternion.identity);
                    endSpellRange = currentCell;
                    return;
                }
                previousCell = currentCell;
                currentCell = new Vector2(currentCell.x + horizontal, currentCell.y + vertical);
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Exit")
        {
            coll.gameObject.SetActive(false);
            Debug.Log("Exit");
        }
        else if (coll.tag == "Enemy")
        {
            //coll.gameObject.SetActive(false);
            Debug.Log("Enemy");
        }
        else if (coll.tag == "Projectile")
        {
            Debug.Log("Projectile");
            coll.gameObject.SetActive(false);
            playerInfo.updateHealth( -1 * coll.GetComponent<Projectile>().damage);
        }
    }



    //functions that really shouldn't ever need to be changed
    //pretty easy / self explanitory

	private void actionCheck()
	{
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            displayAttackRange(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            displayAttackRange(0, -1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            displayAttackRange(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            displayAttackRange(1, 0);
        }
	}

    public void setSpell(Spell spell)
    {
        currentSpell = spell;
        attackObject = spell.projectileObject;
    }

    private void clearSpell()
    {
        currentSpell = null;
        attackObject = null;
    }

	private void destroyRangeList()
	{
		if(rangeList == null) return;
		for(int i = 0; i < rangeList.Length; i++)
		{
			Destroy(rangeList[i]);
		}
        isAttacking = false;
	}

	private bool checkEnemyLocations(Vector2 targetCell)
	{
		for(int i = 0; i < enemyList.Length; i++)
		{
			Vector2 enemyPos = enemyList[i].transform.position;
			if(targetCell == enemyPos) return true;
		}
		return false;
	}

	private TileBase getCell(Tilemap tilemap, Vector2 cellPos)
	{
		TileBase currentTile = tilemap.GetTile(tilemap.WorldToCell(cellPos));
		return currentTile;
	}

    private IEnumerator SmoothMovement(Vector3 end)
    {
        gameManager.isMoving = true;
        //while (isMoving) yield return null;

        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        float inverseMoveTime = 1 / moveTime;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, end, inverseMoveTime * Time.deltaTime);
            transform.position = newPosition;
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }
        gameManager.TurnChange();
    }
}

