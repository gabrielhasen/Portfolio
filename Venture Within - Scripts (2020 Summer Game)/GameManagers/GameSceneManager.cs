using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataSystem;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MMPersistentSingleton<GameSceneManager>,
                                    MMEventListener<CorgiEngineEvent>
{
    public bool hasBeenToIntro;
    private void Awake()
    {
        base.Awake();
        SaveIntroScene();
        LoadSavedIntroScene();
    }

    public bool HasSeenIntroScene()
    {
        return hasBeenToIntro;
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
        //SaveIntroScene();
        LoadSavedIntroScene();
    }

    private void EndScene()
    {
        if (SceneManager.GetActiveScene().name == "IntroScene") {
            hasBeenToIntro = true;
        }
        SaveIntroScene();
    }

    protected const string _resourceItemPath = "Data/";
    protected const string _saveFolderName = "DataSystem/";

    protected const string _saveFileNameCurrency = "scene";
    protected const string _saveFileExtensionCurrency = ".intro";

    public virtual void SaveIntroScene()
    {
        SerializedIntro serializedIntro = new SerializedIntro();
        FillSerializedIntroScene(serializedIntro);
        MMSaveLoadManager.Save(serializedIntro, _saveFileNameCurrency + _saveFileExtensionCurrency, _saveFolderName);
    }

    /// <summary>
    /// Tries to load the currency if a file is present
    /// </summary>
    public virtual void LoadSavedIntroScene()
    {
        SerializedIntro serializedIntro = (SerializedIntro)MMSaveLoadManager.Load(typeof(SerializedIntro), _saveFileNameCurrency + _saveFileExtensionCurrency, _saveFolderName);
        ExtractSerializedIntroScene(serializedIntro);
    }

    private void FillSerializedIntroScene(SerializedIntro serializedIntro)
    {
        serializedIntro.hasBeenToIntro = hasBeenToIntro;
        Debug.Log(hasBeenToIntro);
    }

    private void ExtractSerializedIntroScene(SerializedIntro serializedIntro)
    {
        if (serializedIntro == null) {
            Debug.Log("IsNull");
            return;
        }
        Debug.Log(serializedIntro.hasBeenToIntro);
        hasBeenToIntro = serializedIntro.hasBeenToIntro;
    }
}
