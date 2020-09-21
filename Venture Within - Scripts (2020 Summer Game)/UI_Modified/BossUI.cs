using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using TMPro;

public class BossUI : MMPersistentSingleton<BossUI>
{
    private GameObject UI;
    public TextMeshProUGUI BossText;

    private void Start()
    {
        UI = transform.GetChild(0).gameObject;
    }

    public void EnableBossUI(string name)
    {
        BossText.text = name;
        UI.SetActive(true);
    }

    public void DisableBossUI()
    {
        UI.SetActive(false);
    }
}
