using DataSystem;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTimeManager :  MMPersistentSingleton<DataTimeManager>,
                                MMEventListener<CorgiEngineEvent>
{
    [MMReadOnly]
    public float timeSinceStart = 0;

    /// <summary>
    /// Data Management
    /// </summary>
    protected const string _resourceItemPath = "Data/";
    protected const string _saveFolderName = "DataSystem/";
    protected const string _saveFileNameUpgrades = "time";
    protected const string _saveFileExtensionUpgrades = ".time";

    public float GetTime()
    {
        return timeSinceStart;
    }

    private void Update()
    {
        timeSinceStart += Time.deltaTime;
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
            case CorgiEngineEventTypes.LevelStart:
                StartCoroutine(Wait());
                break;
            case CorgiEngineEventTypes.LevelEnd:
                EndLevel();
                break;
            case CorgiEngineEventTypes.PlayerDeath:
                EndLevel();
                break;
            case CorgiEngineEventTypes.GameOver:
                EndLevel();
                break;
        }
    }
    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        StartLevel();
    }

    private void StartLevel()
    {
        //SaveTime();
        LoadSavedTime();
    }

    private void EndLevel()
    {
        SaveTime();
    }

    public virtual void SaveTime()
    {
        SerializedTime serializedTime = new SerializedTime();
        FillSerializedTime(serializedTime);
        MMSaveLoadManager.Save(serializedTime, _saveFileNameUpgrades + _saveFileExtensionUpgrades, _saveFolderName);
    }

    /// <summary>
    /// Tries to load the currency if a file is present
    /// </summary>
    public virtual void LoadSavedTime()
    {
        SerializedTime serializedTime = (SerializedTime)MMSaveLoadManager.Load(typeof(SerializedTime), _saveFileNameUpgrades + _saveFileExtensionUpgrades, _saveFolderName);
        ExtractSerializedTime(serializedTime);
    }

    private void FillSerializedTime(SerializedTime serializedTime)
    {
        serializedTime.Value += timeSinceStart;
    }

    private void ExtractSerializedTime(SerializedTime serializedTime)
    {
        if (serializedTime == null) {
            SaveTime();
            return;
        }
        timeSinceStart = serializedTime.Value;
    }
}
