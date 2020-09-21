using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    private GameObject currentItem;

    private void Start()
    {
        GrabItem();
        if(currentItem == null) {
            Debug.LogError("Could not grab item from (ItemData: GrabBuffItem)");
            return;
        }
        SetItem();
    }

    public void GrabItem()
    {
        currentItem = ItemData.Instance.GrabBuffItem();
    }

    public void SetItem()
    {
        Instantiate(currentItem, transform.position, Quaternion.identity, transform);
    }
}
