using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Spell : ScriptableObject 
{
	new public string name = "New Item";
	public GameObject projectileObject = null;	
	public Sprite icon = null;
	public int damage = 0;
	public int armor = 0;
	public int range = 0;
	public int spellType = 0;
	public int manaCost = 0;
	Player_Movement playerMove;
	//public int cost = 0;

	//Different items will do different things so this is why
	//it is a virtual function, as each item should overwrite this.
	public virtual void Use()
	{
		playerMove = Player_Movement.instance;
		playerMove.setSpell(this);
		Debug.Log("Using " + name);
	}

	public void RemoveFromInventory()
	{
		Spell_Bar.instance.Remove(this);
	}
}
