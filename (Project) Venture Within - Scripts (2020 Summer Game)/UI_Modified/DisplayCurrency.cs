using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplayCurrency : MonoBehaviour, 
                                MMEventListener<CorgiEngineEvent>
{
    private GameObject currencyGO;
    private TextMeshProUGUI Text_Currency;
    private bool CurrencyAnimationPlaying;
    private Vector2 localScaleCurrency;
    private const float ScaleTime = 0.1f;
    public float scaleAmount;

    private void Start()
    {
        currencyGO = transform.GetChild(0).gameObject;
        Text_Currency = gameObject.GetComponent<TextMeshProUGUI>();

        localScaleCurrency = currencyGO.transform.localScale;
        
        PlayerInventory.Instance.eventCurrencyUp.AddListener(CurrencyUp);
        PlayerInventory.Instance.eventCurrencyDown.AddListener(CurrencyDown);
        SetCurrentCurrency();
    }

    private void SetCurrentCurrency()
    {
        Text_Currency.text = PlayerInventory.Instance.currencyAmount.ToString();
    }

    private void CurrencyUp()
    {
        Text_Currency.text = PlayerInventory.Instance.currencyAmount.ToString();
        PlayShakeCurrency();
    }

    private void CurrencyDown()
    {
        Text_Currency.text = PlayerInventory.Instance.currencyAmount.ToString();
        PlayShakeCurrency();
    }

    private void PlayShakeCurrency()
    {
        if (CurrencyAnimationPlaying) return;

        CurrencyAnimationPlaying = true;
        Vector2 scaleVector = new Vector2(currencyGO.transform.localScale.x + scaleAmount, currencyGO.transform.localScale.y + scaleAmount);
        LeanTween.scale(currencyGO.gameObject, scaleVector, ScaleTime).setEase(LeanTweenType.easeInSine).setOnComplete(ScaleBackCurrency);
    }
    private void ScaleBackCurrency()
    {
        LeanTween.scale(currencyGO.gameObject, localScaleCurrency, ScaleTime).setEase(LeanTweenType.easeOutSine).setOnComplete(EndShakeCurrency);
    }
    private void EndShakeCurrency()
    {
        CurrencyAnimationPlaying = false;
    }

    void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>();
    }
    void OnDisable()
    {
        this.MMEventStopListening<CorgiEngineEvent>();
    }

    public virtual void OnMMEvent(CorgiEngineEvent engineEvent)
    {
        switch (engineEvent.EventType) {
            case CorgiEngineEventTypes.LoadPoints:
                SetCurrentCurrency();
                break;
        }
    }
}
