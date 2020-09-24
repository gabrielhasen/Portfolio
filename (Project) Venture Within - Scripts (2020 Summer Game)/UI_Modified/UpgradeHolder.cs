using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeHolder : MonoBehaviour
{
    public UpgradeType type;
    public Upgrade upgrade;
    public Image icon;

    private void Start()
    {
        icon = transform.GetChild(0).GetComponent<Image>();
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        FindUpgrade();
    }

    private void FindUpgrade()
    {
        upgrade = PlayerUpgrades.Instance.FindUpgrade(type);
        if (upgrade.UnlockedStatus) {
            icon.sprite = upgrade.UnlockedImage;
        }
        else {
            icon.sprite = upgrade.LockedImage;
        }
    }

    public void OnClick()
    {
        if (upgrade.UnlockedStatus) {
            UpgradeMenu.Instance.UnlockedDisplayInfo(upgrade);
        }
        else {
            UpgradeMenu.Instance.LockedDisplayInfo(upgrade);
        }
    }
}
