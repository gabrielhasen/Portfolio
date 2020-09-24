using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataSystem;
using UnityEngine.SceneManagement;

public class GeomancerManager : MMPersistentSingleton<GeomancerManager>,
                                MMEventListener<CorgiEngineEvent>
{
    public GeomancerList geomancerList;
    public List<Geomancer> _geomancers;

    private GameObject Sphalerite;
    private GameObject Chalcopyrite;
    private GameObject Pyrite;
    private GameObject Mother;

    private List<GameObject> GeoGO;

    protected const string _resourceItemPath = "Data/";
    protected const string _saveFolderName = "DataSystem/";
    protected const string _saveFileNameUpgrades = "Geomancer";
    protected const string _saveFileExtensionUpgrades = ".geomance";

    public GameObject GetLockedGeomancer()
    {
        for (int i = 0; i < _geomancers.Count; i++) {
            if (!_geomancers[i].UnlockedStatus) {
                switch (_geomancers[i].Type) {
                    case GeomancerIdentifier.Sphalerite:
                        UnlockGeomancer(GeomancerIdentifier.Sphalerite);
                        return Sphalerite;

                    case GeomancerIdentifier.Chalcopyrite:
                        UnlockGeomancer(GeomancerIdentifier.Chalcopyrite);
                        return Chalcopyrite;

                    case GeomancerIdentifier.Pyrite:
                        UnlockGeomancer(GeomancerIdentifier.Pyrite);
                        return Pyrite;

                    case GeomancerIdentifier.Mother:
                        UnlockGeomancer(GeomancerIdentifier.Mother);
                        return Mother;
                    default:
                        break;
                }
            }
        }
        return null;
    }

    private void LoadInBaseGeomancers()
    {
        GameObject grabber = GameObject.Find("GeomancerSubGrab");
        if(grabber == null) {
            Debug.LogError("Could not find GeomancerSubGrab to activate unlocked geomancers on submarine");
        }

        for (int i = 0; i < _geomancers.Count; i++) {
            if (_geomancers[i].UnlockedStatus) {
                switch (_geomancers[i].Type) {
                    case GeomancerIdentifier.Sphalerite:
                        grabber.GetComponent<GeomancerSubGrab>().Sphalerite.SetActive(true);
                        break;
                    case GeomancerIdentifier.Chalcopyrite:
                        grabber.GetComponent<GeomancerSubGrab>().Chalcopyrite.SetActive(true);
                        break;
                    case GeomancerIdentifier.Pyrite:
                        grabber.GetComponent<GeomancerSubGrab>().Pyrite.SetActive(true);
                        break;
                    case GeomancerIdentifier.Mother:
                        grabber.GetComponent<GeomancerSubGrab>().Mother.SetActive(true);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void UnlockGeomancer(GeomancerIdentifier type)
    {
        for (int i = 0; i < _geomancers.Count; i++) {
            if(_geomancers[i].Type == type) {
                _geomancers[i].UnlockedStatus = true;
            }
        }
    }

    public GameObject GetGeomancer(GeomancerIdentifier type)
    {
        switch (type) {
            case GeomancerIdentifier.Sphalerite:
                return Sphalerite;
            case GeomancerIdentifier.Chalcopyrite:
                return Chalcopyrite;
            case GeomancerIdentifier.Pyrite:
                return Pyrite;
            case GeomancerIdentifier.Mother:
                return Mother;
            default:
                break;
        }
        return null;
    }

    private void Start()
    {
        LoadInGeomancersGameObjects();
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
        if (geomancerList == null) {
            LoadInGeomancers();
        }
        //Save geomancers here if you want to reset all geomancers back to the inital setting
        SaveGeomancers();
        LoadGeomancers();
        AssignInfoToGeomancersGO();
        if (SceneManager.GetActiveScene().name == "Base") {
            LoadInBaseGeomancers();
        }
    }

    private void EndLevel()
    {
        SaveGeomancers();
    }

    private void LoadInGeomancers()
    {
        _geomancers = new List<Geomancer>();

        // the Upgrade List scriptable object must be in a Resources folder inside your project, like so : Resources/Upgrades/PermUpgrades/PUT_SCRIPTABLE_OBJECT_HERE
        geomancerList = (GeomancerList)Resources.Load("Geomancers/GeomancerList");
        if (geomancerList == null) {
            Debug.LogError("Could not find UpgradeList in 'Resources/Upgrades/PermUpgrades'");
            return;
        }

        foreach (Geomancer geo in geomancerList.Geomancers) {
            _geomancers.Add(geo.Copy());
        }
    }

    private void LoadInGeomancersGameObjects()
    {
        GeoGO = new List<GameObject>();

        Object[] tempClass = Resources.LoadAll<GameObject>("GeomancerGameObjects");
        foreach (GameObject item in tempClass) {
            GeoGO.Add(item);
        }

        foreach (GameObject item in GeoGO) {
            switch (item.name) {
                case "Sphalerite":
                    Sphalerite = item;
                    break;
                case "Chalcopyrite":
                    Chalcopyrite = item;
                    break;
                case "Pyrite":
                    Pyrite = item;
                    break;
                case "Mother":
                    Mother = item;
                    break;
                default:
                    break;
            }
        }
    }

    private void AssignInfoToGeomancersGO()
    {
        for (int i = 0; i < _geomancers.Count; i++) {
            GeomancerIdentifier type = _geomancers[i].Type;
            switch (type) {
                case GeomancerIdentifier.Sphalerite:
                    Sphalerite.GetComponent<GeomancerHolder>().info = _geomancers[i];
                    break;
                case GeomancerIdentifier.Chalcopyrite:
                    Chalcopyrite.GetComponent<GeomancerHolder>().info = _geomancers[i];
                    break;
                case GeomancerIdentifier.Pyrite:
                    Pyrite.GetComponent<GeomancerHolder>().info = _geomancers[i];
                    break;
                case GeomancerIdentifier.Mother:
                    Mother.GetComponent<GeomancerHolder>().info = _geomancers[i];
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Saves the achievements current status to a file on disk
    /// </summary>
    public void SaveGeomancers()
    {
        SerializedGeomancerManager serializedGeo = new SerializedGeomancerManager();
        FillSerializedGeomancers(serializedGeo);
        MMSaveLoadManager.Save(serializedGeo, _saveFileNameUpgrades + _saveFileExtensionUpgrades, _saveFolderName);
    }

    private void FillSerializedGeomancers(SerializedGeomancerManager serializedGeo)
    {
        serializedGeo.geomancers = new SerializedGeomancer[_geomancers.Count];
        for (int i = 0; i < _geomancers.Count; i++) {
            SerializedGeomancer newGeo = new SerializedGeomancer(_geomancers[i].Type, _geomancers[i].UnlockedStatus);
            serializedGeo.geomancers[i] = newGeo;
        }
    }

    public void LoadGeomancers()
    {
        SerializedGeomancerManager serializedGeo = (SerializedGeomancerManager)MMSaveLoadManager.Load(typeof(SerializedGeomancerManager), _saveFileNameUpgrades + _saveFileExtensionUpgrades, _saveFolderName);
        ExtractSerializedGeomancers(serializedGeo);
    }

    private void ExtractSerializedGeomancers(SerializedGeomancerManager serializedGeo)
    {
        if (serializedGeo == null) {
            return;
        }

        for (int i = 0; i < _geomancers.Count; i++) {
            _geomancers[i].Type = serializedGeo.geomancers[i].GeoType;
            _geomancers[i].UnlockedStatus = serializedGeo.geomancers[i].UnlockStatus;
        }
    }
}
