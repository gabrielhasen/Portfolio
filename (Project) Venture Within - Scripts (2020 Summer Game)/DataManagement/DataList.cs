using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataList", menuName = "Data System/Data List")]
public class DataList : ScriptableObject
{
    public string DataListID;
    public List<Data> AllData;
    public List<Item> AllItems;
}
