using UnityEngine;
using MoreMountains.CorgiEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemClick : Button_OpenClose, IPointerEnterHandler, IPointerExitHandler
{
    [Header("ITEM LOGIC")]
    public TabGroup tabGroup;
    public TabButton thisButton;
    public Item currentItem;
    private GameObject player;
    private RockHolder rockHolder;

    /// <summary>
    /// Is how to add item into a UI slot
    /// </summary>
    public Item CurrentItem
    {
        get { return currentItem; }
        set {
            currentItem = value;
            SetNewItem();
        }
    }

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
    /// Calls base function, and changes sprite to idle
    /// </summary>
    protected override void DeactivateObject()
    {
        base.DeactivateObject();
        tabGroup.selectedTab.background.sprite = tabGroup.tabIdle;
    }

    /// <summary>
    /// Adds an item to the UI element.  Is only called when assigning value to
    /// CurrentItem.
    /// </summary>
    private void SetNewItem()
    {
        GameObject child = gameObject.transform.GetChild(0).gameObject;
        child.GetComponent<Image>().sprite = currentItem.icon;
        thisButton.CanBeClicked = true;

        ItemDisplay.Instance.DisplayItemInfo(currentItem);
    }

    /// <summary>
    /// Checks if ItemDisplay is open and if it is, change the data being displayed if are clicking on
    /// tab that is not already selected.  If clicking on same tab that is already selected, close ItemDisplay
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem == null)
            return;

        if (ObjectToOpen.activeInHierarchy && tabGroup.selectedTab != thisButton) {
            ItemDisplay.Instance.DisplayItemInfo(currentItem);
            return;
        }

        ItemDisplay.Instance.DisplayItemInfo(currentItem);
        base.OnPointerClick(eventData);
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
}
