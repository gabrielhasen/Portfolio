using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;
    public Image background;
    public UnityEvent OnTabSelected;
    public UnityEvent OnTabDeselected;
    public bool CanBeClicked;

    private void Awake()
    {
        background = GetComponent<Image>();
        tabGroup.Subscribe(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CanBeClicked)
            tabGroup.OnTabSelected(this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CanBeClicked)
            tabGroup.OnTabEnter(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (CanBeClicked)
            tabGroup.OnTabExit(this);
    }

    public void Select()
    {
        if(OnTabSelected != null)
        {
            OnTabSelected.Invoke();
        }
    }
    public void Deselect()
    {
        if(OnTabDeselected != null)
        {
            OnTabDeselected.Invoke();
        }
    }
}
