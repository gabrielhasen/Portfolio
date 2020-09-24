using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    public GameObject attackObject;
	public int range;
	public int damage;

	public int waitTime;
	public int currentWaitTime;

	GameManager gameManager;
	// Use this for initialization
	void Start () 
	{
		range = 2;
		damage = 1;
		waitTime = 3;
		gameManager = GameManager.instance;
		gameManager.NextTurnCallBack += Shoot;	
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void Shoot()
	{
		if (isMovingTime()) return;

		Vector2 currentCell = transform.position;
       	Vector2 attackCell1 = new Vector2(currentCell.x + 1, currentCell.y + 0);
	   	Vector2 attackCell2 = new Vector2(currentCell.x + 0, currentCell.y + 1);
	   	Vector2 attackCell3 = new Vector2(currentCell.x + -1, currentCell.y + 0);
	   	Vector2 attackCell4 = new Vector2(currentCell.x + 0, currentCell.y + -1);
        //instantiate current attackObject
        GameObject temp1;
        temp1 = Instantiate(attackObject, attackCell1, Quaternion.identity);
        GameObject temp2;
        temp2 = Instantiate(attackObject, attackCell2, Quaternion.identity);
        GameObject temp3;
        temp3 = Instantiate(attackObject, attackCell3, Quaternion.identity);
        GameObject temp4;
        temp4 = Instantiate(attackObject, attackCell4, Quaternion.identity);
        temp1.GetComponent<Projectile>().setInfo(temp1, range, attackCell1, 1, 0, damage);
        temp2.GetComponent<Projectile>().setInfo(temp2, range, attackCell2, 0, 1, damage);
        temp3.GetComponent<Projectile>().setInfo(temp3, range, attackCell3, -1, 0, damage);
        temp4.GetComponent<Projectile>().setInfo(temp4, range, attackCell4, 0, -1, damage);
	}

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

}
