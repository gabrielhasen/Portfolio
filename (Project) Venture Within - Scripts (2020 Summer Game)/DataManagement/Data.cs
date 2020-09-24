using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used when creating a new achievement / data to collect
public enum DataType
{
	FirstTimeInVent,
	TimesInNewVent,
	ItemPickup
}

[Serializable]
public class Data
{
	[Header("DESCRIPTOR INFORMATION")]
	public DataType Type;
	public string Title;
	[TextArea(3, 10)]
	public string Description;

	[Header("PROGRESS INFORMATION")]
	[Space(20)]
	public int CurrentProgress;
	public int AchieveAt;

	[Header("FINISHED INFORMATION")]
	[Space(20)]
	public bool FinishedStatus;
	public float TimeAchieved;

	public void DataFinished(float _time)
	{
		if (FinishedStatus) {
			return;
		}

		FinishedStatus = true;
		TimeAchieved = _time;
	}

	/// <summary>
	/// Adds the specified value to the current progress.
	/// </summary>
	/// <param name="newProgress">New progress.</param>
	public virtual void AddProgress(int _newProgress, float _time)
	{
		CurrentProgress += _newProgress;
		if(CurrentProgress >= AchieveAt) {
			DataFinished(_time);
        }
	}

	/// <summary>
	/// Sets the progress to the value passed in parameter.
	/// </summary>
	/// <param name="newProgress">New progress.</param>
	public virtual void SetProgress(int _newProgress, float _time)
	{
		CurrentProgress = _newProgress;
		if (CurrentProgress >= AchieveAt) {
			DataFinished(_time);
		}
	}

	/// <summary>
	/// Copies this Data (useful when loading from a scriptable object list)
	/// </summary>
	public virtual Data Copy()
	{
		Data clone = new Data();
		// we use Json utility to store a copy of our data, not a reference
		clone = JsonUtility.FromJson<Data>(JsonUtility.ToJson(this));
		return clone;
	}
}
