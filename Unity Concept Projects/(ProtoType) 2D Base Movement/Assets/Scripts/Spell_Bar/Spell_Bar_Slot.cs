using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell_Bar_Slot : MonoBehaviour {

	//icon is used to display the sprite for the item
	public Image icon;
	public Button removeButton;
	Spell spell;
	Player_Movement playerMove;

	void Start()
	{
		playerMove = Player_Movement.instance;
	}

	public void AddItem(Spell newSpell)
	{
		spell = newSpell;
		icon.sprite = spell.icon;
		icon.enabled = true;
		removeButton.interactable = true;
	}

	public void ClearSlot()
	{
		spell = null;
		icon.sprite = null;
		icon.enabled = false;
		removeButton.interactable = false;
	}

	public void UseItem()
	{
		if(spell != null)
		{
			spell.Use();
			/*if(cost == 0)
			{
				card.RemoveFromInventory();
			}*/
		}
	}

}
