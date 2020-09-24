using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NationType{
	Wild,
	Human
}

[CreateAssetMenu(fileName = "New Nation", menuName = "Nation")]
public class Nation : ScriptableObject 
{
	public NationType type;
	public Sprite NationIcon;
	[TextArea(3,20)] public string RaceLore;
}
