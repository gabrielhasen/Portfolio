using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Tools;

public class ItemDisplay : MMSingleton<ItemDisplay>
{
    public GameObject itemDisplay;
    private TextMeshProUGUI Text_Title;
    private TextMeshProUGUI Text_DescriptionScience;
    private TextMeshProUGUI Text_DescriptionGameplay;
    private TextMeshProUGUI Text_Stat1;
    private TextMeshProUGUI Text_Stat2;
    private TextMeshProUGUI Text_Stat3;
    private Image Image_Icon;

    protected override void Awake()
    {
        base.Awake();
        GetInformation();
    }

    /// <summary>
    /// Used when clicking on an item, and change the information being displayed
    /// </summary>
    /// <param name="item">Item info to be displayed</param>
    public void DisplayItemInfo(Item item)
    {
        Text_Stat1.text = "";
        Text_Stat2.text = "";
        Text_Stat3.text = "";

        Text_Title.text = item.itemName;
        Text_DescriptionScience.text = item.description;
        Text_DescriptionGameplay.text = item.descriptionItem;
        Image_Icon.sprite = item.icon;
        for (int i = 0; i < item.statList.Count; i++) {
            if(i == 0) {
                StatText(Text_Stat1, item.statList[0]);
            }
            if (i == 1) {
                StatText(Text_Stat2, item.statList[1]);
            }
            if (i == 2) {
                StatText(Text_Stat3, item.statList[2]);
            }
        }
    }

    /// <summary>
    /// Displays the proper text for each stat that is to be displayed
    /// </summary>
    /// <param name="Text_Stat">Text visually to change</param>
    /// <param name="stat">The stat information</param>
    private void StatText(TextMeshProUGUI Text_Stat, Stat stat)
    {
        Text_Stat.text = stat.Type.ToString();

        // If it is a int / float modifier
        if (stat.Modifier != 0) {
            if (stat.Modifier > 0) {
                Text_Stat.text += ": +";
            }
            else {
                Text_Stat.text += ": ";
            }
            Text_Stat.text += stat.Modifier;
        }
        // If it is a vector modifier
        else if(stat.VectorModifier != Vector3.zero) {
            if(stat.VectorModifier.x < 0 || stat.VectorModifier.y < 0 ||
                stat.VectorModifier.z < 0) {
                Text_Stat.text += ": -";
            }
            else {
                Text_Stat.text += ": +";
            }
        }
        // If it is a bool modifier
        else {
            if (stat.BoolModifier) {
                Text_Stat.text += ": Enable";
            }
            else {
                Text_Stat.text += ": Disable";
            }
        }
    }

    /// <summary>
    /// From children, finds the correct gameObjects to assign to variables
    /// </summary>
    private void GetInformation()
    {
        for (int i = 0; i < itemDisplay.transform.childCount; i++) {
            GameObject temp = itemDisplay.transform.GetChild(i).gameObject;
            if (temp.name == "Text_Title") {
                Text_Title = temp.GetComponent<TextMeshProUGUI>();
            }
            if (temp.name == "Text_DescriptionScience") {
                Text_DescriptionScience = temp.GetComponent<TextMeshProUGUI>();
            }
            if (temp.name == "Text_DescriptionGameplay") {
                Text_DescriptionGameplay = temp.GetComponent<TextMeshProUGUI>();
            }
            if (temp.name == "Text_Stat1") {
                Text_Stat1 = temp.GetComponent<TextMeshProUGUI>();
            }
            if (temp.name == "Text_Stat2") {
                Text_Stat2 = temp.GetComponent<TextMeshProUGUI>();
            }
            if (temp.name == "Text_Stat3") {
                Text_Stat3 = temp.GetComponent<TextMeshProUGUI>();
            }
            if (temp.name == "Image_Icon") {
                Image_Icon = temp.GetComponent<Image>();
            }
        }
    }
}
