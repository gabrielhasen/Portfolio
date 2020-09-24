using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public TabButton selectedTab;
    public List<GameObject> objectsToSwap;
    public UnityEvent OnGroupStart;

    private void Start() {
        OnGroupStart.Invoke();
    }

    public void Subscribe(TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
        button.background.sprite = tabIdle;
    }

    public void OnTabEnter(TabButton button)
    {
        RestTabs();
        if(selectedTab == null || button != selectedTab)
            button.background.sprite = tabHover;
    }
    public void OnTabExit(TabButton button)
    {
        RestTabs();
    }
    public void OnTabSelected(TabButton button)
    {
        if(selectedTab != null)
        {
            selectedTab.Deselect();
        }
        //Set Selected Tab
        RestTabs();
        if(selectedTab != null)selectedTab.background.sprite = tabIdle;
        selectedTab = button;

        selectedTab.Select();

        button.background.sprite = tabActive;
        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i < objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void RestTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if(selectedTab != null && button == selectedTab) { continue; }
            button.background.sprite = tabIdle;
        }
    }
}
