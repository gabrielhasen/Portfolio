using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Information : MonoBehaviour {

	public static Player_Information instance;
	void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("More than 1 instance of AllCards");
			return;
		}
		instance = this;
	}

	public GameObject characterStats;
	public Text textHealth;
	public Text textArmor;
	public Text textDamage;
    public Sprite playerIcon;

	public string playerName;
	public int health;
	public int armor;
	public int damage;
	public int mana;


	void Start () 
	{
        playerName = "Player Name";
		health = 5;
		armor = 0;
		damage = 1;
		mana = 3;
		updateVisuals();
	}
	
	void Update () 
	{
        if (health <= 0) Debug.Log("Ya dead");
		if(Input.GetKeyDown(KeyCode.E))
		{
			characterStats.SetActive(!characterStats.activeSelf);
		}
	}

	private void updateVisuals()
	{
        textHealth.text = "Health:\t\t\t";
        textArmor.text = "Armor:\t\t\t";
        textDamage.text = "Damage:\t\t";

        textHealth.text += health.ToString();
		textArmor.text += armor.ToString();
		textDamage.text += damage.ToString();
	}

    public void updateHealth(int damage)
    {
        health = health - damage;
        updateVisuals();
    }

    public void updateArmor(int arm)
    {
        armor = armor + arm;
        updateVisuals();
    }

    public void updateDamage(int dam)
    {
        damage = damage + dam;
        updateVisuals();
    }

    public void updateMana(int man)
    {
        mana = mana + man;
        updateVisuals();
    }
}
