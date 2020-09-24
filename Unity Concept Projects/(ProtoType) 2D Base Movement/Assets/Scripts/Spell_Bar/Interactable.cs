using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	GameManager gameManager;
	Transform player;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
			Interact();
        }
	}

	public virtual void Interact()
	{
		// This method is meant to be overwritten
		Debug.Log("Interacting with " + transform.name);
	}

	void Start()
	{
		gameManager = GameManager.instance;
        GameObject playerCharacter = GameObject.FindWithTag("Player");
		player = playerCharacter.transform;
	}	
}
