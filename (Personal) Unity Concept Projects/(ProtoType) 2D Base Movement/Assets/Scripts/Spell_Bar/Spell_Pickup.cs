using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Pickup : Interactable {

	public Spell spell;

	public override void Interact()
	{
		base.Interact();
		PickUp();
	}
	
	void PickUp()
	{
		Debug.Log("Picking up " + spell.name);
		bool pickedUp = Spell_Bar.instance.Add(spell);
		if(pickedUp) Destroy(gameObject);
	}
}
