using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour 
{
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

	WorldController worldController;
	public World world;

	//Panel - Tile Information
	public Text textTileType;
	public Text textDescribeValue;
	public Tile currentMainTile;
	
	void Start () 
	{
		worldController = WorldController.Instance;
		world = worldController.world;
		textTileType.gameObject.SetActive(false);
	}

	public void OnTileSelect(Tile currentTile)
	{
		if(currentTile == null){
			return;
		}
		textTileType.gameObject.SetActive(true);
		currentMainTile = currentTile;
		textTileType.text = currentTile.Name;
	}
}
