using MoreMountains.CorgiEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossEntrance : MonoBehaviour
{
    public bool isFinalBoss;
    public GameObject endingGO;
    public UnityEvent OnDeathEvent;
    public GameObject Boss;
    public string BossName;
    public Transform BossSpawnLocation;
    public GameObject StopPosition;

    public GameObject EnableOnMonsterSpawn;
    public GameObject DisableOnMonsterSpawn;

    public GameObject bossGO;
    private Character bossChar;

    private bool hasBeenTriggered;
    private bool bossHasDied;
    private GameObject doorGO;
    private GameObject player;

    private CharacterHorizontalMovement playerMovement;
    private CharacterJump jumpMovement;
    private RockHolder rockHolder;
    private RockBreak rockBreak;
    private CharacterDash characterDash;
    private CurrencyDropper currencyDropper;

    private void Start()
    {
        if(endingGO != null)
            endingGO.SetActive(false);
        
        DisableOnMonsterSpawn.SetActive(true);
        EnableOnMonsterSpawn.SetActive(false);

        currencyDropper = GetComponent<CurrencyDropper>();
        doorGO = transform.GetChild(0).gameObject;
        hasBeenTriggered = false;
        bossHasDied = false;
    }

    private void Update()
    {
        if (bossHasDied) 
            return;

        if(bossGO != null) {
            if(bossChar != null && bossChar.ConditionState.CurrentState == CharacterStates.CharacterConditions.Dead) {
                bossHasDied = true;
                BossDeath();
                MagicBarrierManager.Instance.DisableBarrier();
            }
        }
    }

    private void BossDeath()
    {
        //Is mini or final boss
        CorgiEngineEvent.Trigger(CorgiEngineEventTypes.BossDefeated);
        OnDespawn();
        if(OnDeathEvent.GetPersistentEventCount() > 0) {
            OnDeathEvent.Invoke();
        }

        //Is final-boss
        if (isFinalBoss) {
            endingGO.SetActive(true);
        }
        //Is mini-boss
        else {
            OpenDoor();
        }

        StartCoroutine(BossKillWait());
    }

    private void OnDespawn()
    {
        DisableOnMonsterSpawn.SetActive(true);
    }

    private IEnumerator BossKillWait()
    {
        CutsceneManager.Instance.StopPlayerMovement();
        SetCameraOnBoss();
        BossDeathAnimation();
        yield return new WaitForSeconds(2f);
        
        if (isFinalBoss) {
            SetCameraOnEnd();
            yield return new WaitForSeconds(2f);
            SetCameraOnPlayer();
            //DIALOGUE GOES HERE FOR KILLING FINAL BOSS
            //yield return new WaitForSeconds();
            CutsceneManager.Instance.EnablePlayerMovement();
        }
        else {
            SetCameraOnPlayer();
            //DIALOGUE GOES HERE FOR KILLING MINI BOSS
            //IS FOR BREAKING DOWN MAGIC BARRIER IN THE VENT
            //yield return new WaitForSeconds();
            CutsceneManager.Instance.EnablePlayerMovement();
        }
    }

    private void BossDeathAnimation()
    {
        currencyDropper.DropCurrency(bossGO.transform.position);
        LeanTween.scale(bossGO, new Vector2(0,0), 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !hasBeenTriggered) {
            hasBeenTriggered = true;
            player = collision.gameObject;
            MoveCharacter();
            //CloseDoor();
        }
    }

    private void MoveCharacter()
    {
        playerMovement = player.GetComponent<CharacterHorizontalMovement>();
        jumpMovement = player.GetComponent<CharacterJump>();
        rockHolder = player.GetComponent<RockHolder>();
        rockBreak = player.GetComponent<RockBreak>();
        characterDash = player.GetComponent<CharacterDash>();
        CutsceneManager.Instance.StopPlayerMovement();

        playerMovement.SetHorizontalMove(1f);

        StartCoroutine( WaitToMoveAgain() );
    }

    public void StopPlayerMovement()
    {
        playerMovement.SetHorizontalMove(0f);
        jumpMovement.enabled = false;
        playerMovement.ReadInput = false;
        rockHolder.enabled = false;
        rockBreak.enabled = false;
        characterDash.enabled = false;
    }

    private void EnablePlayerMovement()
    {
        jumpMovement.enabled = true;
        jumpMovement.NumberOfJumpsLeft = 1;
        playerMovement.ReadInput = true;
        rockHolder.enabled = true;
        rockBreak.enabled = true;
        characterDash.enabled = true;
    }

    private IEnumerator WaitToMoveAgain()
    {
        yield return new WaitForSeconds(1f);
        StopCharacter();
    }

    private void StopCharacter()
    {
        playerMovement.SetHorizontalMove(0f);
        CloseDoor();
        StartCoroutine( WaitToClose() );
    }

    private void CloseDoor()
    {
        LeanTween.scale(doorGO, new Vector2(1, 1), 0.3f);
    }

    private void OpenDoor()
    {
        LeanTween.scale(doorGO, new Vector2(0, 0), 0.3f);
    }

    private IEnumerator WaitToClose()
    {
        yield return new WaitForSeconds(1f);
        SpawnEnemy();
        yield return new WaitForSeconds(2f);
        EnableMovement();
    }

    private void SpawnEnemy()
    {
        //Event here
        if (isFinalBoss) {
            CorgiEngineEvent.Trigger(CorgiEngineEventTypes.FinalBossRoom);
        }
        else {
            CorgiEngineEvent.Trigger(CorgiEngineEventTypes.MiniBossRoom);
        }

        if (bossGO == null) {
            bossGO = Instantiate(Boss, BossSpawnLocation.position, Quaternion.identity, gameObject.transform);
        }
        bossChar = bossGO.GetComponent<Character>();
        SetCameraOnBoss();
        EnemyNameEnable();
        OnSpawn();
    }

    private void OnSpawn()
    {
        DisableOnMonsterSpawn.SetActive(false);
        EnableOnMonsterSpawn.SetActive(true);
    }

    private void SetCameraOnBoss()
    {
        MMCameraEvent.Trigger(MMCameraEventTypes.SetTargetCharacter, bossGO.GetComponent<Character>());
        MMCameraEvent.Trigger(MMCameraEventTypes.StartFollowing);
    }
    private void SetCameraOnPlayer()
    {
        MMCameraEvent.Trigger(MMCameraEventTypes.SetTargetCharacter, LevelManager.Instance.Players[0]);
        MMCameraEvent.Trigger(MMCameraEventTypes.StartFollowing);
    }
    private void SetCameraOnEnd()
    {
        MMCameraEvent.Trigger(MMCameraEventTypes.SetTargetCharacter, endingGO.GetComponent<Character>());
        MMCameraEvent.Trigger(MMCameraEventTypes.StartFollowing);
    }

    private void EnemyNameEnable()
    {
        BossUI.Instance.EnableBossUI(BossName);
    }

    private void EnemyNameDisable()
    {
        BossUI.Instance.DisableBossUI();
    }

    private void EnableMovement()
    {
        EnemyNameDisable();
        SetCameraOnPlayer();
        CutsceneManager.Instance.EnablePlayerMovement();
        StopPosition.SetActive(false);
    }
}
