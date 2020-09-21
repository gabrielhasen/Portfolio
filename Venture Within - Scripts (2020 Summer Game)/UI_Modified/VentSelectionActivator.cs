using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentSelectionActivator : MonoBehaviour
{
    public GameObject VentUI;
    public GameObject UpgradeUI;

    public void OpenUI()
    {
        VentUI.SetActive(true);
    }

    public void CloseUI()
    {
        VentUI.SetActive(false);
    }

    public void OpenUIUpgrade()
    {
        UpgradeUI.SetActive(true);
    }

    public void CloseUIUpgrade()
    {
        UpgradeUI.SetActive(false);
    }
}
