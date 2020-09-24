using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClassType{
	Warrior,
	Mage,
	Cleric,
	Thief
}

[CreateAssetMenu(fileName = "New Class", menuName = "Class")]
public class ClassRPG : ScriptableObject 
{
	public ClassType type;
	public Sprite ClassIcon;
	[TextArea(3,20)] public string ClassLore;
}
