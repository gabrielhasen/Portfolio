using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character 
{
	[Header ("SCRIPTABLE OBJECTS")]
	public ClassRPG CharacterClass;
	public Race CharacterRace;
	public Nation CharacterNation;
	
	[Space (20)] [Header ("CHARACTER DISPLAY INFO")]
	public string CharacterName;
	public Sprite CharacterPortrait;
	[Range (1,5)]public int Rarity; //Common(Grey), Uncommon(Green), Rare(Blue), Epic(Purple), Legendary(Orange)

	[Space (20)] [Header ("CHARACTER STATS")]
	public StatRange CharacterLevel;
	public StatRange HealthValue;
	public Stat StatMagic;
	public Stat StatDamage;

	[Space (20)] [Header ("CHARACTER LORE")]
	[TextArea(3,20)] public string CharacterLore;


	//
	// Used to retrieve CHARACTER VALUES
	//

	//
	// Character functions
	//
	public Character()
	{
		
	}
	/*public void InitialCreation()
	{
		StatDamage = new Stat(1);
		CharacterLevel = new StatRange(1,1, statRangeType.Level);
		HealthValue = new StatRange(10,10);
	}*/

	public void LevelUp()
	{
		

	}
}
