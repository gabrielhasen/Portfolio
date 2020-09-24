using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Button_OpenClose : MonoBehaviour,  IPointerClickHandler
{
    // Public Variables
    public bool resetPosition;
    public bool waitTime;

    [Header("CLICK OPEN OBJECT")]
    public GameObject ObjectToOpen;
    public float timeToScaleObject;
    public LeanTweenType easeTypeFadeOut;
    public LeanTweenType easeTypeFadeIn;
    public UnityEvent onClickOpenEvent;

    [Header("CLICK BUTTON")]
    public Vector3 shakeAmount;
    public float timeShake;
    public LeanTweenType easeTypeButton;

    // Private Variables
    private bool playAnimationResize;
    private Vector3 OpenOriginalScale;
    private Vector3 OriginalScale;
    private Vector3 OriginalPosition;

    protected virtual void Start() {
        OriginalPosition = ObjectToOpen.transform.position;
        playAnimationResize = true;
        OpenOriginalScale = ObjectToOpen.transform.localScale;
        OriginalScale = gameObject.transform.localScale;
        if(waitTime)
            StartCoroutine(waitToTurnOff());
        else {
            ObjectToOpen.transform.localScale = new Vector3(0, 0, 0);
            ObjectToOpen.SetActive(false);
        }
    }

    private IEnumerator waitToTurnOff()
    {
        yield return new WaitForSeconds(0.11f);
        ObjectToOpen.transform.localScale = new Vector3(0, 0, 0);
        ObjectToOpen.SetActive(false);
    }

    public virtual void OnPointerClick(PointerEventData eventData) {
        if(OpenOriginalScale.x < 0.1) {
            OpenOriginalScale = new Vector3(1, 1, 1);
        }

        if(ObjectToOpen.activeInHierarchy && playAnimationResize) {
            playAnimationResize = false;
            LeanTween.scale(gameObject, shakeAmount, timeShake).setOnComplete(ScaleBack).setEase(easeTypeButton);

            LeanTween.scale(ObjectToOpen, new Vector3(0, 0, 0), timeToScaleObject).setOnComplete(DeactivateObject).setEase(easeTypeFadeOut);
        }
        else if(!ObjectToOpen.activeInHierarchy && playAnimationResize) {
            playAnimationResize = false;
            ObjectToOpen.SetActive(true);
            LeanTween.scale(gameObject, shakeAmount, timeShake).setOnComplete(ScaleBack).setEase(easeTypeButton);

            if(resetPosition)
                ObjectToOpen.transform.position = OriginalPosition;

            LeanTween.scale(ObjectToOpen, OpenOriginalScale, timeToScaleObject).setOnComplete(ActivateObject).setEase(easeTypeFadeIn);
            onClickOpenEvent.Invoke();
        }
    }

    protected virtual void DeactivateObject() {
        ObjectToOpen.SetActive(false);
        playAnimationResize = true;
    }
    private void ActivateObject() {
        playAnimationResize = true;
    }

    private void ScaleBack() {
        LeanTween.scale(gameObject, OriginalScale, timeShake).setEase(easeTypeButton);
    }

}
