using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Tools;
using TMPro;
using UnityEngine.UI;
using MoreMountains.CorgiEngine;

[System.Serializable]
public class UnityEventInt : UnityEvent<int> { }
public class UnityEventItem : UnityEvent<Item> { }

public class PlayerUI : MMPersistentSingleton<PlayerUI>,
                                MMEventListener<CorgiEngineEvent>
{
    private PlayerInventory playerInventory;

    [Header("Tween Variables")]
    public float scaleAmount;
    private Vector2 localScaleBomb;
    private Vector2 localScaleCurrency;

    [Header("GameObjects")]
    public GameObject bombGO;
    private TextMeshProUGUI bombAmount;

    public GameObject currencyGO;
    private TextMeshProUGUI currencyAmount;

    public GameObject itemInventory;
    private List<GameObject> itemHolders;

    private bool BombAnimationPlaying;
    private bool CurrencyAnimationPlaying;

    protected override void Awake()
    {
        base.Awake();
        BombAnimationPlaying = false;
        CurrencyAnimationPlaying = false;

        localScaleCurrency = currencyGO.transform.localScale;

        //Get Gameobjects
        bombAmount = bombGO.GetComponentInChildren<TextMeshProUGUI>();
        currencyAmount = currencyGO.GetComponentInChildren<TextMeshProUGUI>();
        localScaleBomb = bombGO.transform.localScale;
        GetItemHolders();

        //Assign Listeners
        playerInventory = PlayerInventory.Instance;
        playerInventory.eventSetStartUI.AddListener(SetStartUI);
        playerInventory.eventBombUse.AddListener(BombUse);
        playerInventory.eventBombEmpty.AddListener(BombEmpty);
        playerInventory.eventBombPickup.AddListener(BombPickup);

        playerInventory.eventCurrencyUp.AddListener(CurrencyUp);
        playerInventory.eventCurrencyDown.AddListener(CurrencyDown);

        playerInventory.eventBuffAdded.AddListener(ItemAddedToInventory);
    }

    private void GetItemHolders()
    {
        itemHolders = new List<GameObject>();
        int length = itemInventory.transform.childCount;
        for (int i = 0; i < length; i++) {
            itemHolders.Add(itemInventory.transform.GetChild(i).gameObject);
            //itemInventory.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void SetStartUI()
    {
        bombAmount.text = playerInventory.BombAmount.ToString();
    }

    #region Bombs
    private void BombUse()
    {
        bombAmount.text = playerInventory.BombAmount.ToString();
        PlayShakeBomb();
    }

    private void BombEmpty()
    {

    }

    private void BombPickup()
    {
        bombAmount.text = playerInventory.BombAmount.ToString();
        PlayShakeBomb();
    }
    #endregion

    #region Currency
    private void CurrencyUp()
    {
        currencyAmount.text = playerInventory.currencyAmount.ToString();
        PlayShakeCurrency();
        //StartCoroutine(Tween.ShakeGameObject(currencyGO, 0.1f, .2f, LeanTweenType.easeInSine, LeanTweenType.easeOutSine, 0.1f));
    }

    private void SetCurrentCurrency()
    {
        currencyAmount.text = playerInventory.currencyAmount.ToString();
    }

    private void CurrencyDown()
    {
        currencyAmount.text = playerInventory.currencyAmount.ToString();
        StartCoroutine(Tween.ShakeGameObject(currencyGO, 0.1f, .2f, LeanTweenType.easeInSine, LeanTweenType.easeOutSine, 0.1f));
    }
    #endregion

    private void ItemAddedToInventory(Item item)
    {
        Debug.Log(item.itemName);
        for (int i = 0; i < itemHolders.Count; i++) {
            if (itemHolders[i].GetComponent<UI_ItemClick>().currentItem == null) {
                itemHolders[i].GetComponent<UI_ItemClick>().CurrentItem = item;
                break;
            }
        }
    }

    private const float ScaleTime = 0.1f;
    private void PlayShakeBomb()
    {
        if (BombAnimationPlaying) return;

        BombAnimationPlaying = true;
        Vector2 scaleVector = new Vector2(bombGO.transform.localScale.x + scaleAmount, bombGO.transform.localScale.y + scaleAmount);
        LeanTween.scale(bombGO, scaleVector, ScaleTime).setEase(LeanTweenType.easeInSine).setOnComplete(ScaleBackBomb);
    }
    private void ScaleBackBomb()
    {
        LeanTween.scale(bombGO, localScaleCurrency, ScaleTime).setEase(LeanTweenType.easeOutSine).setOnComplete(EndShakeBomb);
    }
    private void EndShakeBomb()
    {
        BombAnimationPlaying = false;
    }
    private void PlayShakeCurrency()
    {
        if (CurrencyAnimationPlaying) return;

        CurrencyAnimationPlaying = true;
        Vector2 scaleVector = new Vector2(currencyGO.transform.localScale.x + scaleAmount, currencyGO.transform.localScale.y + scaleAmount);
        LeanTween.scale(currencyGO, scaleVector, ScaleTime).setEase(LeanTweenType.easeInSine).setOnComplete(ScaleBackCurrency);
    }
    private void ScaleBackCurrency()
    {
        LeanTween.scale(currencyGO, localScaleCurrency, ScaleTime).setEase(LeanTweenType.easeOutSine).setOnComplete(EndShakeCurrency);
    }
    private void EndShakeCurrency()
    {
        CurrencyAnimationPlaying = false;
    }

    //Event Listeners
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
            case CorgiEngineEventTypes.LevelStart:
                //StartScene();
                break;
            case CorgiEngineEventTypes.LevelEnd:
                //EndScene();
                break;
            case CorgiEngineEventTypes.LoadPoints:
                SetCurrentCurrency();
                break;
        }
    }
}
