using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JobType{
	Farmer,
	Apprentice_Farmer,
	Knight,
	King
}

[CreateAssetMenu(fileName = "New Job", menuName = "Job")]
public class Job : ScriptableObject 
{
	public JobType type;
	
	[Range(1,4)] 
	public int JobTier;
	
	public Sprite JobIcon;
	
	[TextArea(3,20)] 
	public string JobLore;
}
