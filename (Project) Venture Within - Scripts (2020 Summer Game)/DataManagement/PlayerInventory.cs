using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;
using UnityEngine.Events;
using DataSystem;

public class PlayerInventory : MMPersistentSingleton<PlayerInventory>, 
                                MMEventListener<CorgiEngineEvent>
{
    //All Events
    //Invoke events after you have finished all variable manipulation and logic
    public UnityEvent eventSetStartUI;
    public UnityEvent eventBombUse;
    public UnityEvent eventBombEmpty;
    public UnityEvent eventBombPickup;

    public UnityEvent eventHealthCurrentUp;
    public UnityEvent eventHealthCurrentDown;
    public UnityEvent eventHealthMaxUp;
    public UnityEvent eventHealthMaxDown;

    public UnityEvent eventCurrencyUp;
    public UnityEvent eventCurrencyDown;

    public UnityEventItem eventBuffAdded = new UnityEventItem();

    public List<Item> equippedItems;
    public int currencyAmount = 0;



    /// <summary>
    /// Data Management
    /// </summary>
    protected const string _resourceItemPath = "Data/";
    protected const string _saveFolderName = "DataSystem/";

    protected const string _saveFileNameCurrency = "currency";
    protected const string _saveFileExtensionCurrency = ".points";

    //Variables
    private GameObject player;
    private int bombAmount;
    public int BombAmount
    {
        get { return bombAmount; }
        set {
            bombAmount = value;
            if(bombAmount < 0) {
                bombAmount = 0;
            }
        }
    }

    public bool HasBombsToUse()
    {
        if(bombAmount > 0) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes eventBombUse or eventBombEmpty
    /// </summary>
    /// <returns>true if have bomb, and fals if does not</returns>
    public bool UseBomb()
    {
        if(bombAmount > 0) {
            BombAmount--;
            eventBombUse.Invoke();
            return true;
        }
        //Start UI display to show you have no bombs
        eventBombEmpty.Invoke();
        return false;
    }

    /// <summary>
    /// Is called from ItemPickup.cs and adds a bomb to your inventory
    /// Invokes eventBombPickup
    /// </summary>
    public void BombPickup()
    {
        BombAmount++;
        eventBombPickup.Invoke();
    }

    public void CurrencyUp(int value)
    {
        currencyAmount += value;
        eventCurrencyUp.Invoke();
    }

    public void CurrencyDown(int value)
    {
        currencyAmount -= value;
        eventCurrencyDown.Invoke();
    }

    /// <summary>
    /// Calls PlayerUpgrades
    /// Decreases players current health and invokes events listening
    /// </summary>
    /// <param name="healthValue"></param>
    /// <returns></returns>
    public bool HealthCurrentUp(int healthValue)
    {
        bool temp = PlayerUpgrades.Instance.CurrentHealthUp(healthValue);
        if (temp) {
            eventHealthCurrentUp.Invoke();
        }
        return temp;
    }

    /// <summary>
    /// Calls PlayerUpgrades
    /// Decreases players current health and invokes events listening
    /// </summary>
    /// <param name="healthValue">health change</param>
    public void HealthCurrentDown(int healthValue)
    {
        PlayerUpgrades.Instance.CurrentHealthDown(healthValue);
        eventHealthCurrentDown.Invoke();
    }

    /// <summary>
    /// Calls PlayerUpgrades
    /// Increases players max health and invokes events listening
    /// </summary>
    /// <param name="healthValue">health change</param>
    public void HealthMaxUp(int healthValue)
    {
        PlayerUpgrades.Instance.Upgrade_MaxHealthUp(healthValue);
        eventHealthMaxUp.Invoke();
    }

    /// <summary>
    /// Calls PlayerUpgrades
    /// Decreases players max health and invokes events listening
    /// </summary>
    /// <param name="healthValue">health change</param>
    public void HealthMaxDecrease(int healthValue)
    {
        PlayerUpgrades.Instance.Upgrade_MaxHealthDown(healthValue);
        eventHealthMaxDown.Invoke();
    }

    public void BuffAdded(Item item)
    {
        equippedItems.Add(item);
        eventBuffAdded.Invoke(item);
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
                StartScene();
                break;
            case CorgiEngineEventTypes.LevelEnd:
                EndScene();
                break;
            case CorgiEngineEventTypes.PlayerDeath:
                EndScene();
                break;
            case CorgiEngineEventTypes.GameOver:
                EndScene();
                break;
        }
    }

    /// <summary>
    /// Called by MMEventListener LevelStart
    /// </summary>
    private void StartScene()
    {
        LoadSavedCurrency();

        if (bombAmount <= 0)
            //return;

        BombAmount = 7;
        eventSetStartUI.Invoke();
        player = LevelManager.Instance.Players[0].gameObject;
    }

    private void EndScene()
    {
        //ItemsToCurrency();
        SaveCurrency();
    }

    //Save / Serialize

    /// <summary>
    /// Saves the currency to a file
    /// </summary>
    public virtual void SaveCurrency()
    {
        SerializedCurrency serializedCurrency = new SerializedCurrency();
        FillSerializedCurrency(serializedCurrency);
        MMSaveLoadManager.Save(serializedCurrency, _saveFileNameCurrency + _saveFileExtensionCurrency, _saveFolderName);
    }
    
    /// <summary>
    /// Tries to load the currency if a file is present
    /// </summary>
    public virtual void LoadSavedCurrency()
    {
        SerializedCurrency serializedCurrency = (SerializedCurrency)MMSaveLoadManager.Load(typeof(SerializedCurrency), _saveFileNameCurrency + _saveFileExtensionCurrency, _saveFolderName);
        ExtractSerializedCurrency(serializedCurrency);
        CorgiEngineEvent.Trigger(CorgiEngineEventTypes.LoadPoints);
    }

    private void FillSerializedCurrency(SerializedCurrency serializedCurrency)
    {
        serializedCurrency.Value = currencyAmount;
    }

    private void ExtractSerializedCurrency(SerializedCurrency serializedCurrency)
    {
        if (serializedCurrency == null) {
            return;
        }
        currencyAmount = serializedCurrency.Value;
    }
}
