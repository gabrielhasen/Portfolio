using UnityEngine;
using System.Collections;
using System;

namespace DataSystem
{
    [Serializable]
    /// <summary>
    /// Serialized class to help store / load science points from files.
    /// </summary>
    public class SerializedCurrency
    {
        public int Value;
        public string[] ContentType;
    }
}
