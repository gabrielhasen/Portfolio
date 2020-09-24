using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

	public GameObject Cursor;
	public GameObject Highlight;
	public Signal TileSelect;

	Vector3 lastFramePosition;

	void Start () 
	{
		Highlight.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 currentFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		currentFramePosition.z = -10;

		Tile tileUnderMouse;
		//visuals for cursor
		if(currentFramePosition.x < WorldController.Instance.world.Width - 0.5f && currentFramePosition.x >= -0.5 &&
		   currentFramePosition.y < WorldController.Instance.world.Height - 0.5f && currentFramePosition.y >= -0.5){
			tileUnderMouse = GetTileCoord(currentFramePosition);
		}
		else{
			tileUnderMouse = null;
		}
		if(tileUnderMouse != null){
			Cursor.SetActive(true);
			Vector3 cursorPosition = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
			Cursor.transform.position = cursorPosition;
		}
		else{
			Cursor.SetActive(false);
		}

		//left click
		if(Input.GetMouseButtonUp(0)){
			if(tileUnderMouse != null){
				UIController.Instance.OnTileSelect( tileUnderMouse );
				Highlight.transform.position = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
				Highlight.SetActive(true);
			}
		}

		//right click  -  drag camera
		if(Input.GetMouseButton(1)){
			Vector3 diff = lastFramePosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Camera.main.transform.Translate(diff);
		}

		lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		lastFramePosition.z = -10;

		Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis("Mouse ScrollWheel");
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize,2f,20f);
	}

	Tile GetTileCoord(Vector3 coord)
	{
		int x = Mathf.RoundToInt(coord.x);
		int y = Mathf.RoundToInt(coord.y);
		//Debug.Log("Tile position: ("+x+","+y+")");

		return WorldController.Instance.world.GetTileAt(x, y);
	}
}
