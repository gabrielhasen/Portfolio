using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeMenu : MMSingleton<UpgradeMenu>
{
    private Upgrade _upgrade;
    private TextMeshProUGUI Text_Title;
    private TextMeshProUGUI Text_Description;
    //only reason Text_CurrencyCost is public and has to be assigned through inspector is because its
    //a child of another object and needs to be scaled with button press. (Could always find this through a recusive function as well)
    public TextMeshProUGUI Text_CurrencyCost;
    private TextMeshProUGUI Text_Progress;
    private Image Image_DisplayIcon;
    private GameObject Button_Upgrade;
    private GameObject Panel_NotUnlocked;
    private int cost;

    private void Start()
    {
        //Getting text references
        Text_Title = transform.Find("Text_Title").GetComponent<TextMeshProUGUI>();
        Text_Description = transform.Find("Text_Description").GetComponent<TextMeshProUGUI>();
        //Text_CurrencyCost = transform.Find("Text_CurrencyCost").GetComponent<TextMeshProUGUI>();
        Text_Progress = transform.Find("Text_Progress").GetComponent<TextMeshProUGUI>();

        //Getting image references
        Image_DisplayIcon = transform.Find("Image_DisplayIcon").GetComponent<Image>();

        //Getting button references
        Button_Upgrade = transform.Find("Button_Upgrade").gameObject;

        //Getting pannel references
        Panel_NotUnlocked = transform.Find("Panel_NotUnlocked").gameObject;

        Panel_NotUnlocked.SetActive(true);
    }

    public void UnlockedDisplayInfo(Upgrade upgrade)
    {
        _upgrade = upgrade;
        Text_Title.text = upgrade.Title;
        Text_Description.text = upgrade.Description;
        Text_CurrencyCost.text = "- " + upgrade.CurrencyCost.ToString();
        Text_Progress.text = "Progress: " + upgrade.ProgressCurrent.ToString() + " / " + upgrade.ProgressMax.ToString();

        Image_DisplayIcon.sprite = upgrade.UnlockedImage;
        cost = upgrade.CurrencyCost;
        Panel_NotUnlocked.SetActive(false);
    }

    public void LockedDisplayInfo(Upgrade upgrade)
    {
        Panel_NotUnlocked.SetActive(true);
    }

    public void OnClick()
    {
        if(PlayerInventory.Instance.currencyAmount >= cost) {
            bool success = PlayerUpgrades.Instance.ApplyUpgrade(_upgrade.Type);
            if (success) {
                PlayerInventory.Instance.CurrencyDown(cost);
            }
        }
        Text_Progress.text = "Progress: " + _upgrade.ProgressCurrent.ToString() + " / " + _upgrade.ProgressMax.ToString();
        PlayerUpgrades.Instance.SaveUpgrades();
    }
}
