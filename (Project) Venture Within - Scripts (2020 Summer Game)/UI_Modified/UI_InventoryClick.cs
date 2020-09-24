using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MoreMountains.CorgiEngine;

public class UI_InventoryClick : Button_OpenClose, IPointerEnterHandler, IPointerExitHandler
{
    public TabGroup tabGroup;
    public GameObject ItemData;
    private GameObject player;
    private RockHolder rockHolder;

    /// <summary>
    /// Calls base start of Button_OpenClose
    /// Will also get player and rockHolder components
    /// </summary>
    protected override void Start()
    {
        base.Start();
        player = LevelManager.Instance.Players[0].gameObject;
        rockHolder = player.GetComponent<RockHolder>();
    }

    /// <summary>
    /// Makes it so you will not shoot your rock when you click on a ui element
    /// </summary>
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) {
            //rockHolder.PauseShooting();
        }
    }

    /// <summary>
    /// Closes all tab groups.  Resets UI elements.
    /// Closes Inventory as well as Item Info Display
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        tabGroup.selectedTab = null;
        tabGroup.RestTabs();
        if(ItemData.activeInHierarchy)
            LeanTween.scale(ItemData, new Vector3(0, 0, 0), timeToScaleObject).setOnComplete(SetInactive).setEase(easeTypeFadeOut);
    }

    /// <summary>
    /// When a mouse cursor enters a ui element deactivate shooting
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        rockHolder.PauseShooting();
    }

    /// <summary>
    /// When a mouse cursor exits a ui element re-enable shooting
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        rockHolder.UnpauseShooting();
    }

    /// <summary>
    /// Sets Item Info Data to inactive
    /// </summary>
    private void SetInactive()
    {
        ItemData.SetActive(false);
    }
}
