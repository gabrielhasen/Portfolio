using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Manager_WorldInfo : MonoBehaviour 
{
	public List<ClassRPG> ClassList = new List<ClassRPG>();
	public List<Race> RaceList = new List<Race>();
	public List<Job> JobList = new List<Job>();
	private List<Job> JobTier1 = new List<Job>();
	private List<Job> JobTier2 = new List<Job>();
	private List<Job> JobTier3 = new List<Job>();
	private List<Job> JobTier4 = new List<Job>();
	
	private void Awake() 
	{
		LoadScriptableObjects();
		JobsByTier();
	}

	// WHEN LOADING IN NEW SCRIPTABLE OBJECTS, LOAD THEM FROM THIS FUNCTION
	//
	// Loads ScriptableObjects into specified lists. ScriptableObjects are
	// from the "Resource" folder.
	private void LoadScriptableObjects()
	{
		bool hasBeenLoaded = false;

		//Loading all Class ScriptableObjects
		Object[] tempClass = Resources.LoadAll<ClassRPG>("Class");
		foreach (ClassRPG item in tempClass)
		{
			ClassList.Add(item);
		}
		//Loading all Race ScriptableObjects
		Object[] tempRace= Resources.LoadAll<Race>("Race");
		foreach (Race item in tempRace)
		{
			RaceList.Add(item);
		}
		//Loading all Job ScriptableObjects
		Object[] tempJob= Resources.LoadAll<Job>("Job");
		foreach (Job item in tempJob)
		{
			JobList.Add(item);
		}

		// Pass or fail for sucessfully loading all scriptableobjects
		if(ClassList.Count > 0 && RaceList.Count > 0 && JobList.Count > 0)
		{
			hasBeenLoaded = true;
		}
		if(!hasBeenLoaded)
		{
			Debug.LogError("[Manager_WorldInfo]:\nFailed to load all ScriptableObjects\nClassList:\t"
			+ ClassList.Count +"\nRaceList:\t\t" + RaceList.Count + "\nJobList:\t\t"+ JobList.Count);
		}
	}
	private void JobsByTier()
	{
		foreach (Job job in JobList)
		{
			if(job.JobTier == 1) JobTier1.Add(job);
			else if(job.JobTier == 2) JobTier2.Add(job);
			else if(job.JobTier == 3) JobTier3.Add(job);
			else if(job.JobTier == 4) JobTier4.Add(job);
		}
	}
}
