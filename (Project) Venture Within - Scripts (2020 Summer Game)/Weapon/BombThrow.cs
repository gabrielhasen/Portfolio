using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;

public class BombThrow : MonoBehaviour
{
    GameObject player;
    private bool canThrow;

    private void Start()
    {
        player = LevelManager.Instance.Players[0].gameObject;
        canThrow = true;
    }
    void Update()
    {
        if (canThrow && Input.GetButtonDown("Player1_Shoot")) {
            if(PlayerInventory.Instance.HasBombsToUse())
                Throw();
        }
    }

    private void Throw()
    {
        StartCoroutine(WaitTimeToThrow());
        int playerFacing = 1;
        if (player.GetComponent<Character>().IsFacingRight) {
            playerFacing = 1;
        }
        else {
            playerFacing = -1;
        }
        GameObject newBomb = ConsumableData.Instance.grabBomb();
        if (newBomb == null) return; //No usable bombs
        Rigidbody2D temp = newBomb.GetComponent<Rigidbody2D>();
        newBomb.GetComponent<Rigidbody2D>().isKinematic = false;
        newBomb.GetComponent<SpriteRenderer>().enabled = true;
        newBomb.SetActive(true);
        newBomb.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 0.5f);
        Vector2 ThrowForce = new Vector2(30 * playerFacing, 220);
        newBomb.GetComponent<Rigidbody2D>().AddForce(ThrowForce);
        newBomb.GetComponent<Rigidbody2D>().AddTorque(20 * playerFacing * -1, ForceMode2D.Force);
        newBomb.GetComponent<BombExplosion>().Explode();
    }

    private IEnumerator WaitTimeToThrow()
    {
        canThrow = false;
        yield return new WaitForSeconds(.7f);
        canThrow = true;
    }
}
