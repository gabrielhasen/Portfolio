using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MMPersistentSingleton<CutsceneManager>,
                                        MMEventListener<CorgiEngineEvent>
{
    private GameObject player;
    private CharacterHorizontalMovement playerMovement;
    private CharacterJump jumpMovement;
    private RockBreak rockBreak;
    private CharacterDash characterDash;
    private CharacterHandleWeapon characterWeapon;

    public void StopPlayerMovement()
    {
        playerMovement.SetHorizontalMove(0f);
        jumpMovement.enabled = false;
        playerMovement.ReadInput = false;
        rockBreak.enabled = false;
        characterDash.enabled = false;
        characterWeapon.enabled = false;
    }

    public void EnablePlayerMovement()
    {
        jumpMovement.enabled = true;
        jumpMovement.NumberOfJumpsLeft = 1;
        playerMovement.ReadInput = true;
        rockBreak.enabled = true;
        characterDash.enabled = true;
        characterWeapon.enabled = true;
    }

    private void SetCameraOnCharacter(Character character)
    {
        MMCameraEvent.Trigger(MMCameraEventTypes.SetTargetCharacter, character);
        MMCameraEvent.Trigger(MMCameraEventTypes.StartFollowing);
    }

    void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>();
    }

    void OnDisable()
    {
        this.MMEventStopListening<CorgiEngineEvent>();
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
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        StartLevel();
    }

    private void StartLevel()
    {
        player = LevelManager.Instance.Players[0].gameObject;
        GetComponents();
        CameraController.Instance.SetTarget(player.transform);
        //SetCameraOnCharacter(player.GetComponent<Character>());
    }

    private void GetComponents()
    {
        characterWeapon = player.GetComponent<CharacterHandleWeapon>();
        playerMovement = player.GetComponent<CharacterHorizontalMovement>();
        jumpMovement = player.GetComponent<CharacterJump>();
        rockBreak = player.GetComponent<RockBreak>();
        characterDash = player.GetComponent<CharacterDash>();
    }

    private void EndLevel()
    {
        
    }
}
