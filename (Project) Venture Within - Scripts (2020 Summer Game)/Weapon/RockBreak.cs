using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBreak : MonoBehaviour
{
    public GameObject player;
    public GameObject displayLocation;
    private GameObject displayGO;
    private bool canThrow;
    private Vector3 ThrowLocation;
    private InputManager _inputManager;

    private void Start()
    {
        player = LevelManager.Instance.Players[0].gameObject;
        canThrow = true;
        displayGO = Instantiate(displayLocation, player.transform.position, Quaternion.identity);
        displayGO.SetActive(false);
        _inputManager = player.GetComponent<Character>().LinkedInputManager;
    }

    void Update()
    {
        if (canThrow && _inputManager.ThrowButton.State.CurrentState == MMInput.ButtonStates.ButtonPressed) {
            if (PlayerInventory.Instance.HasBombsToUse()) {
                ShowBreakLocation();
            }
        }
        if(canThrow && _inputManager.ThrowButton.State.CurrentState == MMInput.ButtonStates.ButtonUp) {
            Break();
        }
    }

    private void ShowBreakLocation()
    {
        Vector3 currentFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int xPos = 0;
        int yPos = 0;

        if (currentFramePosition.x < player.transform.position.x - 1f) {
            xPos = -2;
        }
        else if (currentFramePosition.x > player.transform.position.x + 1f) {
            xPos = +2;
        }

        if (currentFramePosition.y < player.transform.position.y - 1) {
            yPos = -2;
        }
        else if (currentFramePosition.y > player.transform.position.y + 1) {
            yPos = +2;
        }
        ThrowLocation = new Vector3(player.transform.position.x + xPos, player.transform.position.y + yPos, 0);
        displayGO.transform.position = ThrowLocation;

        if (!displayGO.activeSelf) {
            displayGO.SetActive(true);
        }


    }

    private void Break()
    {
        if (!PlayerInventory.Instance.UseBomb()) {
            return;
        }
        GameObject newBomb = ConsumableData.Instance.grabBomb();

        newBomb.transform.position = ThrowLocation;

        newBomb.SetActive(true);
        displayGO.SetActive(false);
        StartCoroutine(HitTime(newBomb));
        StartCoroutine(WaitTime());
    }

    private IEnumerator HitTime(GameObject newBomb)
    {
        yield return new WaitForSeconds(0.3f);
        newBomb.SetActive(false);

    }

    private IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(1f);
        canThrow = true;
    }
}
