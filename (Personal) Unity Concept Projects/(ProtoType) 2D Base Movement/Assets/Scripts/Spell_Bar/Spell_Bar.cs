using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Bar : MonoBehaviour {

	//Singleton Pattern
	//Will be able to access this with Inventory.instance
	public static Spell_Bar instance;
	void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("More than 1 instance of Inventory");
			return;
		}
		instance = this;
	}

	//delegate is similar to a signal, as when 
	//triggered methods listening for this delegate will be called
	public delegate void OnItemChanged();
	public OnItemChanged OnItemChangedCallBack;

	//Number of slot variables
	int space = 12;
	
	//maybe make this energy?
	//public int money = 0;

	//Data structure to hold the items
	public List<Spell> spells = new List<Spell>();

	//Adds item to inventory
	public bool Add(Spell spell)
	{
		if(spells.Count >= space)
		{
			Debug.Log("Not enough room");
			return false;
		}
		spells.Add(spell);
		if (OnItemChangedCallBack != null)
		{
			//triggering delegate
			OnItemChangedCallBack.Invoke();
		}
		return true;
	}

	//Removes item from the inventory
	public void Remove(Spell spell)
	{
		spells.Remove(spell);
		if (OnItemChangedCallBack != null)
		{
			OnItemChangedCallBack.Invoke();
		}
	}
}

