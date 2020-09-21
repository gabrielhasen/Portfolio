/*using System.Collections;
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

	//Panel - Block Information
	public Text textBlockType;
	public Text textDescribeValue;
	public Block currentMainBlock;
	
	void Start () 
	{
		worldController = WorldController.Instance;
		world = worldController.world;
		textBlockType.gameObject.SetActive(false);
	}

	public void OnBlockSelect(Block currentBlock)
	{
		if(currentBlock == null){
			return;
		}
		textBlockType.gameObject.SetActive(true);
		currentMainBlock = currentBlock;
		textBlockType.text = currentBlock.Name;
	}
}*/
