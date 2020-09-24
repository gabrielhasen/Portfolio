using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager Instance;
	void Awake()
	{
		if(Instance != null)
		{
			Debug.LogWarning("More than 1 instance of GameManager");
			return;
		}
		Instance = this;
	}

	BaseManager baseManager;
	public GameObject WorldMap;
	public GameObject HomeMap;
	public bool mapOpen = false;
	public delegate void NextTurn();			//delegate is similar to a signal, as when
	public NextTurn NextTurnCallBack;			//triggered methods listening for this delegate will be called
	public int currentTurn = 1;
	public int time = 1;
	public Tile currentTile;

	// Use this for initialization
	void Start () 
	{
		baseManager = BaseManager.Instance;
		currentTile = null;
		if(WorldMap.activeInHierarchy == true)
			mapOpen = true;
		
		//WorldMap.SetActive(mapOpen);
		//HomeMap.SetActive(!mapOpen);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.M))
		{
			if(mapOpen == true)
			{
				mapOpen = false;
				WorldMap.SetActive(mapOpen);
				HomeMap.SetActive(!mapOpen);
			}
			else
			{
				mapOpen = true;
				WorldMap.SetActive(mapOpen);
				HomeMap.SetActive(!mapOpen);
			}
		}
	}

	public void TurnChange()
	{
		if (NextTurnCallBack != null)
		{
			NextTurnCallBack.Invoke();
			StartCoroutine(waiting());
			currentTurn++;
		}
	}

	IEnumerator waiting()
	{
		//Will have a pop up animation for with text saying new turn with the current
		//turn number.  Maybe some effect explosions too.
		yield return new WaitForSeconds(time);
	}
}
