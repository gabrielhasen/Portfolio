using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Projectile : MonoBehaviour {

    Tilemap walkableTilemap;
    Tilemap wallTilemap;

    GameObject proj;
    public int damage;
    public int range;
    public int currentRange;
    int horizontal;
    int vertical;
    Vector2 currentCell = new Vector2(0,0);
    GameManager gameManager;

    public void setInfo(GameObject temp, int rangeSet, Vector2 curCell, int hor, int vert, int dmg)
    {
        gameManager = GameManager.instance;

        walkableTilemap = gameManager.walkableTilemap;
        wallTilemap = gameManager.wallTilemap;

        currentRange = 0;
        proj = temp;
        range = rangeSet;
        currentCell = curCell;
        horizontal = hor;
        vertical = vert;
        damage = dmg;

        gameManager.NextTurnCallBack += projectileMove;
    }

    public void projectileMove()
    {
        if(proj == null)
        {
            return;
        }
        if (currentRange >= range)
        {
            Destroy(proj);
        }
        else
        {
            currentCell = new Vector2(currentCell.x + horizontal, currentCell.y + vertical);
            proj.transform.position = currentCell;
            currentRange++;
            if (getCell(wallTilemap, currentCell))
            {
                Destroy(proj);
                return;
            }
        }
    }

    private TileBase getCell(Tilemap tilemap, Vector2 cellPos)
    {
        TileBase currentTile = tilemap.GetTile(tilemap.WorldToCell(cellPos));
        return currentTile;
    }
}
