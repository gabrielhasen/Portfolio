using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreator : MonoBehaviour 
{
	public Character CreateCharacter()
	{
		//Going to have to make characters based off of the town
		//they have spawned in.  So can get race type
		Character temp = new Character();
		SetRace(temp);
		SetJob(temp);
		SetClass(temp);

		return temp;
	}

	private void SetRace(Character _temp)
	{
		//Get city the character is being made in
	}
	private void SetJob(Character _temp)
	{
		//Get rarity of character 
	}
	private void SetClass(Character _temp)
	{
		//Look at job and race.  Decide class from this
	}

}