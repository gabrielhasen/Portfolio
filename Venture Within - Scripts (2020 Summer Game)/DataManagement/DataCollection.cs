using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using DataSystem;
using System.IO;
using UnityEngine.SceneManagement;

public class DataCollection :   MMPersistentSingleton<DataCollection>, 
                                MMEventListener<CorgiEngineEvent>,
                                MMEventListener<MMGameEvent>
{
    public List<Data> _data;
    public List<Item> _itemData;
    public List<ItemDataInfo> itemInfo;
    public DataList dataList;
    public bool PrintPretty;

    public struct ItemDataInfo
    {
        public string name;
        public bool hasRead;
        public float timeAchieved;

        public ItemDataInfo(string _name, bool _hasRead, float _timeAchieved)
        {
            name = _name;
            hasRead = _hasRead;
            timeAchieved = _timeAchieved;
        }
    }

    /// <summary>
    /// Data Management Location
    /// </summary>
    protected const string _resourceItemPath = "Data/";
    protected const string _saveFolderName = "DataSystem/";
    protected const string _saveFileNameUpgrades = "DataGathering";
    protected const string _saveFileExtensionUpgrades = ".txt";

    public void ReadItem(Item _item)
    {
        //If already read just return
        if (_item.hasRead) {
            return;
        }

        //If hasent read find the correct item and record data
        foreach (Item item in _itemData) {
            Debug.Log(item + " " + _item);
            if (item == _item) {
                item.hasRead = true;
                item.timeRead = DataTimeManager.Instance.GetTime();
                SaveDataCollection();
                SaveIntoJson();
            }
        }
    }

    private void DetermineScene()
    {
        if(SceneManager.GetActiveScene().name == "Vent") {
            FindDataLocation(DataType.FirstTimeInVent, true);
            FindDataLocation(DataType.TimesInNewVent, 1);
        }
    }

    private void ItemPickup()
    {
        FindDataLocation(DataType.ItemPickup, 1);
    }

    private void FindDataLocation(DataType type, bool unlocked) { FindDataLocation(type, 0, unlocked); }
    private void FindDataLocation(DataType type, int add) { FindDataLocation(type, add, false); }
    private void FindDataLocation(DataType type, int add, bool unlocked)
    {
        for (int i = 0; i < _data.Count; i++) {
            if(_data[i].Type == type) {
                //If has a value to add besides 0, add to current data progress
                if (add != 0) {
                    _data[i].AddProgress(add, DataTimeManager.Instance.GetTime());
                }

                //Checks if unlocked and does another check to make sure it doesn't overwrite time if already achieved
                if (unlocked && !_data[i].FinishedStatus) {
                    _data[i].FinishedStatus = unlocked;
                }
            }
        }
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

            case CorgiEngineEventTypes.ItemPickup:
                ItemPickup();
                break;
        }
    }

    public virtual void OnMMEvent(MMGameEvent gameEvent)
    {
        switch (gameEvent.EventName) {

        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        StartLevel();
        DetermineScene();
    }

    private void StartLevel()
    {
        LoadInItems();
        LoadInDataSaves();

        // --------------------
        //
        //  Reset on Data Collection
        //
        // --------------------
        //
        //Save here if you want to reset to initial values of data
          SaveDataCollection();
        //
          ResetAllItems();
        //
        // --------------------
        LoadDataCollection();
        SaveIntoJson();
    }

    private void EndLevel()
    {
        SaveDataCollection();
        SaveIntoJson();
    }

    /// --------------------------
    ///
    /// SAVE AND LOAD SECTION
    ///
    /// --------------------------

    private void ResetAllItems()
    {
        for (int i = 0; i < _itemData.Count; i++) {
            _itemData[i].hasRead = false;
            _itemData[i].timeRead = 0;
        }
        SaveDataCollection();
    }

    private void LoadInItems()
    {
        _itemData = new List<Item>();
        Object[] tempClass = Resources.LoadAll<GameObject>("Items");
        foreach (GameObject item in tempClass) {
            Item temp = item.GetComponent<ItemPickup>().itemObject;
            _itemData.Add(temp);
        }
    }

    private void LoadInDataSaves()
    {
        itemInfo = new List<ItemDataInfo>();
        _data = new List<Data>();

        // the Upgrade List scriptable object must be in a Resources folder inside your project, like so : Resources/Upgrades/PermUpgrades/PUT_SCRIPTABLE_OBJECT_HERE
        dataList = (DataList)Resources.Load("Data/DataCollection");
        if (dataList == null) {
            Debug.LogError("Could not find DataList in 'Data/DataCollection'");
            return;
        }

        foreach (Data data in dataList.AllData) {
            _data.Add(data.Copy());
        }
    }

    /// <summary>
    /// Saves the achievements current status to a file on disk
    /// </summary>
    public void SaveDataCollection()
    {
        SerializedDataManager serializedDataManager = new SerializedDataManager();
        FillSerializedData(serializedDataManager);
        MMSaveLoadManager.Save(serializedDataManager, _saveFileNameUpgrades + _saveFileExtensionUpgrades, _saveFolderName);
    }

    private void FillSerializedData(SerializedDataManager serializedDataManager)
    {
        serializedDataManager.DataArray = new SerializedData[_data.Count];
        for (int i = 0; i < _data.Count; i++) {
            SerializedData newData = new SerializedData(_data[i].Type, _data[i].FinishedStatus, _data[i].CurrentProgress, _data[i].AchieveAt, _data[i].TimeAchieved);
            serializedDataManager.DataArray[i] = newData;
        }

        serializedDataManager.ItemDataArray = new SerializedDataItem[_itemData.Count];
        for (int i = 0; i < _itemData.Count; i++) {
            Item temp = _itemData[i];
            SerializedDataItem newItemData = new SerializedDataItem(temp.name, temp.hasRead, temp.timeRead);
            serializedDataManager.ItemDataArray[i] = newItemData;
        }
    }

    public void LoadDataCollection()
    {
        SerializedDataManager serializedDataManager = (SerializedDataManager)MMSaveLoadManager.Load(typeof(SerializedDataManager), _saveFileNameUpgrades + _saveFileExtensionUpgrades, _saveFolderName);
        ExtractSerializedData(serializedDataManager);
    }

    private void ExtractSerializedData(SerializedDataManager serializedDataManager)
    {
        if (serializedDataManager == null) {
            return;
        }

        for (int i = 0; i < _data.Count; i++) {
            _data[i].Type = serializedDataManager.DataArray[i].Type;
            _data[i].FinishedStatus = serializedDataManager.DataArray[i].UnlockStatus;
            _data[i].CurrentProgress = serializedDataManager.DataArray[i].CurrentDataProgress;
            _data[i].AchieveAt = serializedDataManager.DataArray[i].MaxDataProgress;
            _data[i].TimeAchieved = serializedDataManager.DataArray[i].timeAchieved;
        }

        for (int i = 0; i < ItemData.Instance.allItemData.Count; i++) {
            _itemData[i].name = serializedDataManager.ItemDataArray[i].name;
            _itemData[i].hasRead = serializedDataManager.ItemDataArray[i].hasRead;
            _itemData[i].timeRead = serializedDataManager.ItemDataArray[i].timeAchieved;
        }
    }

    public void SaveIntoJson()
    {
        Debug.Log(Application.persistentDataPath);
        string write = "";
        for (int i = 0; i < _data.Count; i++) {
            write += JsonUtility.ToJson(_data[i], PrintPretty);
            write += "\n";
        }
        write += "\nItem_Data\n";
        for (int i = 0; i < ItemData.Instance.allItemData.Count; i++) {
            Item temp = ItemData.Instance.allItemData[i];
            ItemDataInfo tempInfo = new ItemDataInfo(temp.name, temp.hasRead, temp.timeRead);
            write += JsonUtility.ToJson(tempInfo, PrintPretty);
            write += "\n";
        }
        File.WriteAllText(Application.persistentDataPath + "/data.json", write);
    }
    void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>();
        this.MMEventStartListening<MMGameEvent>();
    }

    void OnDisable()
    {
        this.MMEventStopListening<CorgiEngineEvent>();
        this.MMEventStopListening<MMGameEvent>();
    }
}
