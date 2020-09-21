using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Button_Click : MonoBehaviour, IPointerClickHandler 
{
    [Header("CLICK LOGIC")]
    public LeanTweenType clickType;
    public Vector3 ShakeAmountClick;
    public float timeClick;
    public UnityEvent onClickEvent;

    private bool mouseOver;
    private bool playAnimationClick;
    private Vector3 localScale;

    private void Start() {
        playAnimationClick = true;
        localScale = transform.localScale;
    }

    public virtual void OnPointerClick(PointerEventData eventData) {
        if (playAnimationClick) {
            playAnimationClick = false;
            LeanTween.scale(gameObject, ShakeAmountClick, timeClick / 2).setOnComplete(ScaleBackClick).setEase(clickType);

            if (onClickEvent != null)
                onClickEvent.Invoke();
        }
    }

    private void ScaleBackClick() {
        LeanTween.scale(gameObject, localScale, timeClick / 2).setOnComplete(AnimationFinishedClick).setEase(clickType);
    }
    private void AnimationFinishedClick() {
        playAnimationClick = true;
    }
}
