using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    currency,
    bomb,
    healthPickup,
    buff
}

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Item")]
[System.Serializable] public class Item : ScriptableObject
{
    public string itemName;
    public ItemType type;
    public Sprite icon;
    public int SciencePoints;
    [TextArea(3,10)]
    public string description;
    [TextArea(3, 10)]
    public string descriptionItem;
    public List<Stat> statList;

    //For data collection
    public bool hasRead;
    public float timeRead;
}
  
