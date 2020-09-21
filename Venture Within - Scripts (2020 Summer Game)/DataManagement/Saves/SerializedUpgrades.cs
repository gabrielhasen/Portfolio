using UnityEngine;
using System.Collections;
using System;

namespace DataSystem
{
    [Serializable]
    /// <summary>
    /// Serialized class to help store / load upgrades from files
    /// </summary>
    public class SerializedUpgrades
    {
        public UpgradeType Type;
        public bool UnlockStatus;
        public int CurrentUpgradeLevel;
        public int MaxUpgradeLevel;

        public SerializedUpgrades(UpgradeType _type, bool _unlockStatus, int _curLevel, int _maxLevel)
        {
            Type = _type;
            UnlockStatus = _unlockStatus;
            CurrentUpgradeLevel = _curLevel;
            MaxUpgradeLevel = _maxLevel;
        }
    }

    [Serializable]
    public class SerializedUpgradeManager
    {
        public SerializedUpgrades[] upgrades;
    }
}
