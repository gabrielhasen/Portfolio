using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;

public class JumpAgain : MonoBehaviour
{
    public GameObject model;
    public float timeToActive;
    private GameObject player;
    private CharacterJump jumpAbility;
    private bool canActivate;

    void Start()
    {
        canActivate = true;
        player = LevelManager.Instance.Players[0].gameObject;
        jumpAbility = player.GetComponent<CharacterJump>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!canActivate) return;

        if(collision.tag == "Player") {
            canActivate = false;
            CurrentAmountOfJumpsUp();
        }
    }

    private void CurrentAmountOfJumpsUp()
    {
        jumpAbility.NumberOfJumpsLeft++;
        jumpAbility.SetJumpFlags();
        model.SetActive(false);
        StartCoroutine(waitTime());
    }

    private IEnumerator waitTime()
    {
        yield return new WaitForSeconds(timeToActive);
        model.SetActive(true);
        canActivate = true;
    }

}
