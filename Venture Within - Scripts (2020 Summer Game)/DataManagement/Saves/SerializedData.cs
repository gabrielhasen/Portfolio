using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataSystem
{
    [Serializable]
    /// <summary>
    /// Serialized class to help store / load upgrades from files
    /// </summary>
    public class SerializedData
    {
        public DataType Type;
        public bool UnlockStatus;
        public int CurrentDataProgress;
        public int MaxDataProgress;
        public float timeAchieved;

        public SerializedData(DataType _type, bool _unlockStatus, int _curProgress, int _maxProgress, float _timeAchieved)
        {
            Type = _type;
            UnlockStatus = _unlockStatus;
            CurrentDataProgress = _curProgress;
            MaxDataProgress = _maxProgress;
            timeAchieved = _timeAchieved;
        }
    }

    [Serializable]
    public class SerializedDataItem
    {
        public string name;
        public bool hasRead;
        public float timeAchieved;

        public SerializedDataItem(string _name, bool _hasRead, float _timeAchieved)
        {
            name = _name;
            hasRead = _hasRead;
            timeAchieved = _timeAchieved;
        }
    }

    [Serializable]
    public class SerializedDataManager
    {
        public SerializedData[] DataArray;
        public SerializedDataItem[] ItemDataArray;
    }
}
