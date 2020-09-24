using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStop : MonoBehaviour
{
    private bool hasBeenTriggered;

    private void Start()
    {
        hasBeenTriggered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !hasBeenTriggered) {
            hasBeenTriggered = true;
            collision.gameObject.GetComponent<CharacterHorizontalMovement>().SetHorizontalMove(0f);
        }
    }
}
