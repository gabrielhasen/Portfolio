using UnityEngine;

/// <summary>
/// Specifies the ability needed to complete the room
/// </summary>
public enum RoomType
{
    Easy,
    Medium,
    Hard,
    VeryHard,
    Treasure
}

/// <summary>
/// Data storage for the room
/// </summary>
public class Room : MonoBehaviour
{
    public RoomType type;
    public GameObject door;
}
