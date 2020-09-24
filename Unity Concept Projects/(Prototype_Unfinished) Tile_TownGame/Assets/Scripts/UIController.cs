using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public static UIController Instance { get; protected set; }
	void Awake()
	{
		if(Instance != null)
		{
			Debug.LogWarning("More than 1 instance of UIController");
			return;
		}
		Instance = this;
	}

	BaseManager baseManager;
	WorldController worldController;
	GameManager gameManager;
	public World world;

	//Panel - Tile Information
	public Text textTileType;
	public Text textDescribeValue;
	public Text textValue;
	public Tile currentMainTile;
	public Button upgradeButton;

	//Panel - End Turn
	public Text textCurrentTurn;

	//Panel - Base Information
	public Text populationValue;
	public Text foodValue;
	public Text armyValue;
	public Text actionValue;	//Actions per turn
	
	void Start () 
	{
		baseManager = BaseManager.Instance;
		worldController = WorldController.Instance;
		gameManager = GameManager.Instance;
		world = worldController.world;
		textTileType.gameObject.SetActive(false);
		textDescribeValue.gameObject.SetActive(false);
		textValue.gameObject.SetActive(false);
		upgradeButton.gameObject.SetActive(false);
	}

	//Updates display text at end of turn to show new information.
	public void OnEndTurn()
	{
		if(!gameManager.WorldMap.activeSelf) return;
		populationValue.text = baseManager.Population.ToString();
		foodValue.text = baseManager.Food.ToString();
		armyValue.text = baseManager.ArmySize.ToString();
	}

	public void OnTileSelect(Tile currentTile)
	{
		if(currentTile == null){
			return;
		}
		textTileType.gameObject.SetActive(true);
		currentMainTile = currentTile;
		textTileType.text = currentTile.Name;

		//Checks if it's not a basic environmental tile
		if(currentTile.Word1 != "NULL"){
			textDescribeValue.gameObject.SetActive(true);
			textValue.gameObject.SetActive(true);
			textDescribeValue.text = currentTile.Word1;
			textValue.text = currentTile.Value.ToString();

			//Checks if the current tile is upgradable
			if(currentTile.Upgrade != 0){
				upgradeButton.gameObject.SetActive(true);
			}
			else{
				upgradeButton.gameObject.SetActive(false);
			}
		}
		else{
			textDescribeValue.gameObject.SetActive(false);
			upgradeButton.gameObject.SetActive(false);
		}
	}

	public void endTurn()
	{
		gameManager.TurnChange();
	}

	public void ButtonPress()
	{
		world = worldController.world;
		world.setTile(currentMainTile, Tile.TileType.Farm1);
		OnTileSelect(currentMainTile);
	}

	public void ButtonChangeTo_Forest()
	{
		world = worldController.world;
		world.setTile(currentMainTile, Tile.TileType.Forest);
		OnTileSelect(currentMainTile);
	}
	public void ButtonChangeTo_Water()
	{
		world = worldController.world;
		world.setTile(currentMainTile, Tile.TileType.Water);
		OnTileSelect(currentMainTile);
	}
	public void ButtonChangeTo_Farm1()
	{
		world = worldController.world;
		world.setTile(currentMainTile, Tile.TileType.Farm1);
		OnTileSelect(currentMainTile);
	}
}
