using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
	MaxHealth,
	JumpHeight,
	RunSpeed,
	WallJumpDistance,
	DashDistance,
	DashCooldown,
	DashForce,
	RockDamage,
	RockKnockback,
	RockMoveToSpeed,
	RockReturnSpeed
}

[Serializable]
public class Upgrade
{
	[Header("Identification")]
	public UpgradeType Type;
	/// if this is true, the upgrade has been unlocked
	public bool UnlockedStatus;

	[Header("Description")]
	/// the upgrade's name/title
	public string Title;
	/// the upgrade's description
	public string Description;
	/// the amount of points unlocking this upgrade costs
	public int CurrencyCost;

	[Header("Images")]
	/// the image to display while this upgrade is locked
	public Sprite LockedImage;
	/// the image to display when the upgrade is unlocked
	public Sprite UnlockedImage;

	[Header("Progress")]
	/// the max amount of times you can upgrade this Upgrade
	public int ProgressMax;
	/// the current amount of upgrades for this Upgrade
	public int ProgressCurrent;


	[Header("Upgrade Amount")]
	public float StatIncreaseAmount;
	public float StatIncreaseAmountY_IfVector;

	/// <summary>
	/// Takes in an UpgradeType to traverse through PlayerUpgrade.cs list of upgrade to find which to unlock
	/// </summary>
	/// <param name="type">Upgrade unlock type</param>
	public void UnlockUpgrade(UpgradeType type)
    {
        if (UnlockedStatus) {
			return;
        }

		UnlockedStatus = true;
    }

	/// <summary>
	/// Adds the specified value to the current progress.
	/// </summary>
	/// <param name="newProgress">New progress.</param>
	public virtual void AddProgress(int newProgress)
	{
		ProgressCurrent += newProgress;
	}

	/// <summary>
	/// Sets the progress to the value passed in parameter.
	/// </summary>
	/// <param name="newProgress">New progress.</param>
	public virtual void SetProgress(int newProgress)
	{
		ProgressCurrent = newProgress;
	}

	/// <summary>
	/// Copies this achievement (useful when loading from a scriptable object list)
	/// </summary>
	public virtual Upgrade Copy()
	{
		Upgrade clone = new Upgrade();
		// we use Json utility to store a copy of our achievement, not a reference
		clone = JsonUtility.FromJson<Upgrade>(JsonUtility.ToJson(this));
		return clone;
	}
}
