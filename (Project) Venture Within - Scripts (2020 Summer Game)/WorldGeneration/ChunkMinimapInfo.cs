using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChunkMinimapInfo : MonoBehaviour
{
    public ChunkType Type;
    public GameObject ItemGO;
    private SpriteRenderer rendererBackground;
    private SpriteRenderer rendererIcon;
    private bool hasBeenVisited;

    private void Start()
    {
        hasBeenVisited = false;
    }

    private void GetSpriteRenderers()
    {
        rendererBackground = GetComponent<SpriteRenderer>();
        if(rendererBackground == null) {
            Debug.LogError(gameObject.name + " Minimap Chunk does not contain a sprite renderer");
        }

        rendererIcon = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (rendererIcon == null) {
            Debug.LogError(gameObject.name + " Minimap Chunks first child does not contain a sprite renderer");
        }
    }

    private void Check()
    {
        if(rendererBackground == null || rendererIcon == null) {
            GetSpriteRenderers();
        }
    }

    public void SetUnseen()
    {
        Check();
        rendererBackground.sprite = MinimapManager.Instance.Background_Hidden;
        rendererIcon.sprite = MinimapManager.Instance.Icon_QuestionMark;
    }

    public void SetBomb()
    {
        Check();
        rendererIcon.sprite = MinimapManager.Instance.Icon_Bomb;
    }

    public void SetHealth()
    {
        Check();
        rendererIcon.sprite = MinimapManager.Instance.Icon_Health;
    }

    public void SetEmpty()
    {
        Check();
        rendererIcon.sprite = null;
    }

    public void SetBoss()
    {
        Check();
        rendererIcon.sprite = MinimapManager.Instance.Icon_Boss;
        rendererBackground.sprite = MinimapManager.Instance.Background_Hidden;
    }

    public void SetMiniBoss()
    {
        Check();
        rendererIcon.sprite = MinimapManager.Instance.Icon_MiniBoss;
        rendererBackground.sprite = MinimapManager.Instance.Background_Hidden;
    }

    public void SetDoor()
    {
        Check();
        rendererIcon.sprite = MinimapManager.Instance.Icon_Door;
        rendererBackground.sprite = MinimapManager.Instance.Background_Hidden;
    }

    public void SetVisited()
    {
        Check();
        rendererBackground.sprite = MinimapManager.Instance.Background_Visited;
    }

    public void SetCurrentlyIn()
    {
        Check();
        rendererBackground.sprite = MinimapManager.Instance.Background_CurrentlyIn;
    }

    private void SetIcon()
    {
        switch (Type) {
            case ChunkType.Empty:
                SetEmpty();
                break;
            case ChunkType.Door:
                break;
            case ChunkType.Boss:
                SetBoss();
                break;
            case ChunkType.Health:
                SetHealth();
                break;
            case ChunkType.Bomb:
                SetBomb();
                break;
            default:
                break;
        }
    }

    private void GetItemType(Item item)
    {
        switch (item.type) {
            case ItemType.currency:
                break;
            case ItemType.bomb:
                Type = ChunkType.Bomb;
                break;
            case ItemType.healthPickup:
                Type = ChunkType.Health;
                break;
            case ItemType.buff:
                break;
            default:
                break;
        }
    }

    public void PickedUpItem()
    {
        Type = ChunkType.Empty;
        rendererIcon.sprite = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") {
            if (!hasBeenVisited) {
                SetIcon();
                SetCurrentlyIn();
            }
        }

        if (collision.tag == "Item") {
            ItemPickup pickup = collision.gameObject.GetComponent<ItemPickup>();
            if (pickup != null) {
                ItemGO = collision.gameObject;
                pickup.minimapInfo = this;
                GetItemType(pickup.itemObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player") {
            SetVisited();
        }
    }
}
