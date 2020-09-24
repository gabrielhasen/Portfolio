using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeList", menuName = "Data System/Upgrade List")]
public class UpgradeList : ScriptableObject
{
    public string UpgradeListID;
    public List<Upgrade> Upgrades;
}
