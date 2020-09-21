using System.Collections;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class DoorManager : MonoBehaviour
{
    public Teleporter returnDoor;
    public Teleporter returnDoorMiniBoss;
    public Teleporter returnDoorBoss;

    public Teleporter currentDoor;
    
    public GameObject currentRoom;
    public GameObject room_GO;

    public GameObject currentRoomBoss;
    public GameObject roomBoss_GO;

    public GameObject currentRoomMiniBoss;
    public GameObject roomMiniBoss_GO;

    public Vector3 roomLocation;
    public Vector3 miniBossRoomLocation;
    public Vector3 bossRoomLocation;

    /// <summary>
    /// Will create the first room for when you find a door.
    /// Has to create a room to begin with in order to properly connect doors
    /// </summary>
    private void Start()
    {
        GetRoom();
        GetMiniBossRoom();
        GetBossRoom();
    }

    /// <summary>
    /// When going through a door from the level, it saves the door in order to teleport
    /// back to the door when leaving the room.
    /// </summary>
    /// <param name="temp"></param>
    public void SetCurrentDoor(Teleporter temp)
    {
        currentDoor = temp;
        returnDoor.Destination = currentDoor;
        ReturnRock();
    }

    public void SetCurrentDoorMiniBoss(Teleporter temp)
    {
        currentDoor = temp;
        returnDoorMiniBoss.Destination = currentDoor;
        ReturnRock();
    }

    public void SetCurrentDoorBoss(Teleporter temp)
    {
        currentDoor = temp;
        returnDoorBoss.Destination = currentDoor;
        ReturnRock();
    }

    private void ReturnRock()
    {
        StartCoroutine(WaitToReturnRock());
    }

    private IEnumerator WaitToReturnRock()
    {
        yield return new WaitForSeconds(0.4f);
        LevelManager.Instance.Players[0].gameObject.GetComponent<RockHolder>().ReturnRock();
    }

    /// <summary>
    /// The rooms are destroyed when leaving, and a new one is created.
    /// </summary>
    private void DestroyRoom()
    {
        Destroy(currentRoom);
    }

    private void DestroyMiniBossRoom()
    {
        Destroy(currentRoomMiniBoss);
    }

    private void DestroyBossRoom()
    {
        Destroy(currentRoomBoss);
    }

    /// <summary>
    /// Gets the RoomManager data of all rooms
    /// </summary>
    public void GetRoom()
    {
        room_GO = RoomManager.Instance.GetRoom();
        if(room_GO.GetComponent<Room>() == null) {
            Debug.LogError("Room Does not have 'Room' script attached");
        }
        StartCoroutine(WaitTimeRoom());
    }

    public void GetMiniBossRoom()
    {
        roomMiniBoss_GO = RoomManager.Instance.GetMiniBossRoom();
        if (roomMiniBoss_GO.GetComponent<Room>() == null) {
            Debug.LogError("Mini Boss room Does not have 'Room' script attached");
        }
        StartCoroutine(WaitTimeMiniBossRoom());
    }

    public void GetBossRoom()
    {
        roomBoss_GO = RoomManager.Instance.GetBossRoom();
        if (roomBoss_GO.GetComponent<Room>() == null) {
            Debug.LogError("Boss room Does not have 'Room' script attached");
        }
        StartCoroutine(WaitTimeBossRoom());
    }

    /// <summary>
    /// Used as the screen fades, so a new room does not get destroyed and created while the
    /// player is still in the previous room.
    /// </summary>
    /// <returns> A time to wait for </returns>
    private IEnumerator WaitTimeRoom()
    {
        yield return new WaitForSeconds(1f);
        CreateRoom();
    }
    private IEnumerator WaitTimeMiniBossRoom()
    {
        yield return new WaitForSeconds(1f);
        CreateMiniBossRoom();
    }

    private IEnumerator WaitTimeBossRoom()
    {
        yield return new WaitForSeconds(1f);
        CreateBossRoom();
    }

    /// <summary>
    /// The process of creating a room from the RoomManager
    /// </summary>
    private void CreateRoom()
    {
        DestroyRoom();
        currentRoom = Instantiate(room_GO, roomLocation, Quaternion.identity, gameObject.transform);
        GetReturnDoor();
    }

    private void CreateMiniBossRoom()
    {
        DestroyMiniBossRoom();
        currentRoomMiniBoss = Instantiate(roomMiniBoss_GO, miniBossRoomLocation, Quaternion.identity, gameObject.transform);
        GetReturnDoorMiniBoss();
    }

    private void CreateBossRoom()
    {
        DestroyBossRoom();
        currentRoomBoss = Instantiate(roomBoss_GO, bossRoomLocation, Quaternion.identity, gameObject.transform);
        GetReturnDoorBoss();
    }

    /// <summary>
    /// Gets the position of the room door.  So when finding a door in the level, will know where
    /// to teleport the player to as the door changes each room.
    /// </summary>
    private void GetReturnDoor()
    {
        returnDoor.gameObject.transform.position = currentRoom.GetComponent<Room>().door.transform.position;
    }

    private void GetReturnDoorMiniBoss()
    {
        returnDoorMiniBoss.gameObject.transform.position = currentRoomMiniBoss.GetComponent<Room>().door.transform.position;
    }

    private void GetReturnDoorBoss()
    {
        returnDoorBoss.gameObject.transform.position = currentRoomBoss.GetComponent<Room>().door.transform.position;
    }


    public void LoadInInfoOfRoom()
    {
        StartCoroutine(waitToLoad());
    }

    public void LoadInInfoOfRoomMiniBoss()
    {
        StartCoroutine(waitToLoadMiniBoss());
    }

    public void LoadInInfoOfRoomBoss()
    {
        StartCoroutine(waitToLoadBoss());
    }

    private IEnumerator waitToLoad()
    {
        yield return new WaitForSeconds(0.2f);
        currentRoom.GetComponent<Activator>().ActivateAgents();
    }

    private IEnumerator waitToLoadMiniBoss()
    {
        yield return new WaitForSeconds(0.2f);
        currentRoomMiniBoss.GetComponent<Activator>().ActivateAgents();
    }

    private IEnumerator waitToLoadBoss()
    {
        yield return new WaitForSeconds(0.2f);
        currentRoomBoss.GetComponent<Activator>().ActivateAgents();
    }

    public void UnloadInfoOfRoom()
    {
        currentRoom.GetComponent<Activator>().DeactivateAgents();
        ReturnRock();
    }

    public void UnloadInfoOfRoomMiniBoss()
    {
        currentRoomMiniBoss.GetComponent<Activator>().DeactivateAgents();
        ReturnRock();
    }

    public void UnloadInfoOfRoomBoss()
    {
        currentRoomBoss.GetComponent<Activator>().DeactivateAgents();
        ReturnRock();
    }
}
