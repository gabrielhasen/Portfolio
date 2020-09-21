using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Button_EnterClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler , IPointerClickHandler 
{
    [Header("ENTER LOGIC")]
    public LeanTweenType hoverType;
    public Vector3 ShakeAmountHover;
    public float timeHover;
    public UnityEvent onHoverEvent;
    public UnityEvent onHoverExitEvent;
    [Header("CLICK LOGIC")]
    public LeanTweenType clickType;
    public Vector3 ShakeAmountClick;
    public float timeClick;
    public UnityEvent onClickEvent;

    private bool mouseOver;
    private bool playAnimationEnter;
    private bool playAnimationClick;
    private Vector3 localScale;

    protected virtual void Start() {
        playAnimationEnter = true;
        playAnimationClick = true;
        localScale = transform.localScale;
    }

    // Mouse Pointer Functions
    public virtual void OnPointerEnter(PointerEventData eventData) {
        mouseOver = true;
        if (playAnimationEnter) {
            playAnimationEnter = false;
            LeanTween.scale(gameObject, ShakeAmountHover, timeHover / 2).setOnComplete(ScaleBackEnter).setEase(hoverType);

            if (onHoverEvent != null)
                onHoverEvent.Invoke();
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData) {
        mouseOver = false;

        if (onHoverExitEvent != null)
            onHoverExitEvent.Invoke();
    }

    public virtual void OnPointerClick(PointerEventData eventData) {
        if (playAnimationClick) {
            playAnimationClick = false;
            LeanTween.scale(gameObject, ShakeAmountClick, timeClick / 2).setOnComplete(ScaleBackClick).setEase(clickType);

            if(onClickEvent != null)
                onClickEvent.Invoke();
        }
    }

    // OnComplete Functions
    private void ScaleBackEnter() {
        LeanTween.scale(gameObject, localScale, timeHover / 2).setOnComplete(AnimationFinishedHover).setEase(hoverType);
    }
    private void AnimationFinishedHover() {
        playAnimationEnter = true;
    }
    private void ScaleBackClick() {
        LeanTween.scale(gameObject, localScale, timeClick / 2).setOnComplete(AnimationFinishedClick).setEase(clickType);
    }
    private void AnimationFinishedClick() {
        playAnimationClick = true;
    }
}
