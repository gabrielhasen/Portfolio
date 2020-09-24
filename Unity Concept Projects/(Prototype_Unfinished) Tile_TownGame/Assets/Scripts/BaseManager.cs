using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour {

	public static BaseManager Instance;
	void Awake()
	{
		if(Instance != null)
		{
			Debug.LogWarning("More than 1 instance of BaseManager");
			return;
		}
		Instance = this;
	}

	GameManager gameManager;
	WorldController worldController;
	UIController uiController;
	public Tile baseTile;
	private int population;
	private int food;
	private int armySize;
	private int actions;

	public int Population{
		get { return population; }
		set { 
				population = value;
			}
	}
	public int Food{
		get { return food; }
		set { 
				food = value;
			}
	}
	public int ArmySize{
		get { return armySize; }
		set { 
				armySize = value;
			}
	}

	// Use this for initialization
	void Start () 
	{
		population = 5;
		food = 20;
		armySize = 0;
		gameManager = GameManager.Instance;
		worldController = WorldController.Instance;
		uiController = UIController.Instance;
		gameManager.NextTurnCallBack += endTurn;
		uiController.OnEndTurn();
	}
	
	void endTurn()
	{
		
		//Loops through all lists and removes variables based on types.
		/*	forestList 
			waterList 
			dForestList
			abyssList 
			farm1List 
			farm2List 
			farm3List  */
		//Removes food for each population
		food = food - population;
		
		

		//Checks if anything is 0.  If it is don't look through tiles and add values
		//but instead show game over screen.
		if(population <= 0){
			Debug.Log("DEFEAT");
		}else if (food < 0){
			Debug.Log("DEFEAT");
		}

		population = 0;
		//Loops through all lists of tiles and adds variables based on types.
		if(worldController.world.baseList.Count > 0){
			foreach (Tile t in worldController.world.baseList)
			{
				population = population + 5;
			}
		}
		if(worldController.world.house1List.Count > 0){
			foreach (Tile t in worldController.world.house1List)
			{
				
			}
		}
		if(worldController.world.farm1List.Count > 0){
			foreach (Tile t in worldController.world.farm1List)
			{
				food = food + 6;
			}
		}
		//Calls uiController to update ui after values have been changed.
		uiController.OnEndTurn();
	}

	void setValues()
	{

	}

}
