using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class ConsumableData : MMPersistentSingleton<ConsumableData>
{
    public List<GameObject> bombs = new List<GameObject>();
    public GameObject bomb;

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < 2; i++) {
            GameObject temp = Instantiate(bomb, new Vector2(-50, -50), Quaternion.identity, gameObject.transform);
            temp.SetActive(false);
            bombs.Add(temp);
        }
    }

    public GameObject grabBomb()
    {
        for (int i = 0; i < bombs.Count; i++) {
            GameObject temp = bombs[i];
            if (!temp.activeInHierarchy) {
                return temp;
            }
        }
        return null;
    }
}
