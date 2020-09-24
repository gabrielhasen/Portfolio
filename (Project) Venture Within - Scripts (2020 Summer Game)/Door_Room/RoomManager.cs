using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

// From the correct folder, pull all rooms and place them in the appropriate list based on the type of room they are.
// So all double jumps in the double jump room, and all triple in the triple jump etc.
public class RoomManager : MMSingleton<RoomManager>
{
    public List<GameObject> RoomList;
    public List<GameObject> BossRoomList;
    public List<GameObject> MiniBossRoomList;

    /// <summary>
    /// Loads singleton from inherited class, and then loads rooms.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        LoadRooms();
    }

    /// <summary>
    /// On awake loads all rooms from Resources/Rooms folder
    /// </summary>
    private void LoadRooms()
    {
        RoomList = new List<GameObject>();
        Object[] tempRoom = Resources.LoadAll<GameObject>("Rooms/NormalRooms");
        foreach (GameObject item in tempRoom) {
            RoomList.Add(item);
        }

        BossRoomList = new List<GameObject>();
        Object[] tempBoss = Resources.LoadAll<GameObject>("Rooms/BossRooms");
        foreach (GameObject item in tempBoss) {
            BossRoomList.Add(item);
        }

        MiniBossRoomList = new List<GameObject>();
        Object[] tempMiniBoss = Resources.LoadAll<GameObject>("Rooms/MiniBossRooms");
        foreach (GameObject item in tempMiniBoss) {
            MiniBossRoomList.Add(item);
        }
    }

    /// <summary>
    /// Pulls a room (Need to add specified traits eventually)
    /// </summary>
    /// <returns></returns>
    public GameObject GetRoom()
    {
        return RoomList[Random.Range(0,RoomList.Count)];
    }

    public GameObject GetBossRoom()
    {
        return BossRoomList[Random.Range(0, BossRoomList.Count)];
    }

    public GameObject GetMiniBossRoom()
    {
        return MiniBossRoomList[Random.Range(0, MiniBossRoomList.Count)];
    }
}
