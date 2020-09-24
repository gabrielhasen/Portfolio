using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class ItemData : MMPersistentSingleton<ItemData>
{
    private List<GameObject> allItems = new List<GameObject>();
    public List<Item> allItemData = new List<Item>();

    private List<GameObject> buffItems;
    private GameObject bombGO;
    private GameObject healthPickupGO;
    private GameObject currencyGO;

    protected override void Awake()
    {
        base.Awake();
        buffItems = new List<GameObject>();
        LoadInItems();
    }

    private void LoadInItems()
    {
        allItems = new List<GameObject>();
        allItemData = new List<Item>();
        Object[] tempClass = Resources.LoadAll<GameObject>("Items");
        foreach (GameObject item in tempClass) {
            allItems.Add(item);
            allItemData.Add(item.GetComponent<ItemPickup>().itemObject);
            ItemType temp = item.GetComponent<ItemPickup>().itemObject.type;
            switch (temp) {
                case ItemType.currency:
                    currencyGO = item;
                    break;
                case ItemType.bomb:
                    bombGO = item;
                    break;
                case ItemType.healthPickup:
                    healthPickupGO = item;
                    break;
                case ItemType.buff:
                    AddToBuffItems(item);
                    break;
                default:
                    break;
            }
        }
    }

    private void AddToBuffItems(GameObject item)
    {
        buffItems.Add(item);
    }

    public GameObject GrabCurrencyItem()
    {
        return currencyGO;
    }

    public GameObject GrabBuffItem()
    {
        return RandomlySelectBuffItem();
    }
    private GameObject RandomlySelectBuffItem()
    {
        int random = Random.Range(0, buffItems.Count);
        return buffItems[random];
    }

    public GameObject GrabBombItem()
    {
        return bombGO;
    }

    public GameObject GrabHealthPickup()
    {
        return healthPickupGO;
    }

    public void GUILoad()
    {
        Awake();
    }
}
