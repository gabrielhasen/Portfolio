using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GeomancerIdentifier
{
    Sphalerite,
    Chalcopyrite,
    Pyrite,
	Mother
}

[Serializable]
public class Geomancer
{
	[Header("Identification")]
	public GeomancerIdentifier Type;
	/// if this is true, the upgrade has been unlocked
	public bool UnlockedStatus;

	[Header("Description")]
	/// the upgrade's name/title
	public string Name;
	/// the upgrade's description
	public string Description;

	[Header("Images")]
	/// the image to display while this upgrade is locked
	public Sprite Portrait;

	/// <summary>
	/// Takes in an UpgradeType to traverse through PlayerUpgrade.cs list of upgrade to find which to unlock
	/// </summary>
	/// <param name="type">Upgrade unlock type</param>
	public void UnlockGeomancer()
	{
		if (UnlockedStatus) {
			return;
		}

		UnlockedStatus = true;
	}

	public virtual Geomancer Copy()
	{
		Geomancer clone = new Geomancer();
		// we use Json utility to store a copy of our achievement, not a reference
		clone = JsonUtility.FromJson<Geomancer>(JsonUtility.ToJson(this));
		return clone;
	}
}
