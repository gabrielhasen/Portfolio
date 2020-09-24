using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RaceType{
	Human,
	Elf,
	Dwarf
}

[CreateAssetMenu(fileName = "New Race", menuName = "Race")]
public class Race : ScriptableObject 
{
	public RaceType type;
	public Sprite RaceIcon;
	[TextArea(3,20)] public string RaceLore;
}
