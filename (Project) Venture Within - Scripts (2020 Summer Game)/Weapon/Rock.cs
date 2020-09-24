using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using MoreMountains.CorgiEngine;

public class Rock : MonoBehaviour
{
    public Vector2 newPosition;
    public float maxRange;
    public float distanceToReachedTarget;
    public GameObject platform;
    public GameObject lightGO;
    public ParticleSystem particles;
    private bool canShootAgain;
    private bool hasReachedEnd;
    private bool returningToPlayer;
    private bool moveLerp;
    private GameObject player;
    private SpriteRenderer model;
    public BoxCollider2D hitBox;
    private bool CoroutinePlaying;
    private Vector2 previousPosition;
    public bool hasBounced;
    private float Timer;
    private float TimerChange;
    private bool isReturning;

    //Movement
    public float MoveToTimeLerp = 7f;
    public float MoveToTime = 25f;

    public bool ReturningToPlayer
    {
        get { return returningToPlayer; }
        set {
            returningToPlayer = value;
        }
    }

    public bool HasReachedEnd
    {
        get { return hasReachedEnd; }
        set {
            hasReachedEnd = value;
        }
    }

    public bool CanShootAgain
    {
        get { return canShootAgain; }
        set {
            canShootAgain = value;
            if (value) {
                ReachedLocation();
            }
        }
    }

    private void Start()
    {
        Timer = 0;
        TimerChange = 0.13f;
        hasBounced = false;
        CoroutinePlaying = false;
        model = GetComponent<SpriteRenderer>();
        model.enabled = false;
        hitBox.enabled = false;
        player = LevelManager.Instance.Players[0].gameObject;
        returningToPlayer = false;
        moveLerp = true;
        CanShootAgain = true;
        HasReachedEnd = true;
        newPosition = transform.position;
        platform.SetActive(false);
        lightGO.SetActive(false);
        particles.Stop();
        isReturning = false;
    }

    private void Update()
    {
        if (!HasReachedEnd) {
            CalculateDistance();
            if (moveLerp) {
                Timer += Time.deltaTime;
                MoveToLerp();
            }
            else
                MoveTo();
        }
        if (!isReturning && model.enabled && hasReachedEnd) {
            if(Vector2.Distance(player.transform.position, transform.position) > 9) {
                isReturning = true;
                ReturnToPlayer();
                player.GetComponent<RockHolder>().rockOnPlayer = true;
            }
        }
    }

    public void ReachedLocation()
    {
        if (model.enabled && !CoroutinePlaying) {
            CoroutinePlaying = true;
            StartCoroutine(CheckForPlayers());
            //platform.SetActive(true);
        }
    }
    private IEnumerator CheckForPlayers()
    {
        yield return new WaitForSeconds(0.01f);
        if ( (Vector2.Distance(transform.position, player.transform.position) > 0.5f) ) {
            platform.SetActive(true);
            CoroutinePlaying = false;
        }
        else {
            StartCoroutine(CheckForPlayers());
        }
    }

    public void ReturnToPlayer()
    {
        hitBox.enabled = true;
        moveLerp = false;
        StartCoroutine(MovingBack());
    }

    private IEnumerator MovingBack()
    {
        SetNewLocation(player.gameObject.transform.position);
        yield return new WaitForSeconds(0.01f);
        if(!CalculateComingBack())
            StartCoroutine(MovingBack());
        else {
            RockReturned();
        }
    }

    public void RockReturned()
    {
        hitBox.enabled = false;
        moveLerp = true;
        model.enabled = false;
        platform.SetActive(false);
        lightGO.SetActive(false);
        Timer = 0;
        particles.Stop();
        isReturning = false;
    }

    private bool CalculateComingBack()
    {
        if ( (new Vector2(transform.position.x, transform.position.y) - newPosition).magnitude < 0.6f) {
            return true;
        }
        return false;
    }

    public void SetNewLocation(Vector2 pos)
    {
        if(player.GetComponent<CorgiController>())
        if (!model.enabled) {
            gameObject.transform.position = player.transform.position;
            model.enabled = true;
            hitBox.enabled = true;
        }
        platform.SetActive(false);
        lightGO.SetActive(true);
        newPosition = pos;
        HasReachedEnd = false;
        particles.Play();
    }

    private void MoveToLerp()
    {
        if(Timer > TimerChange)
            transform.position = Vector2.Lerp(gameObject.transform.position, newPosition, Time.deltaTime * MoveToTimeLerp);
        else
            transform.position = Vector2.MoveTowards(gameObject.transform.position, newPosition, Time.deltaTime * 3);
    }

    private void MoveTo()
    {
        transform.position = Vector2.MoveTowards(gameObject.transform.position, newPosition, Time.deltaTime * MoveToTime);
    }

    public void CalculateDistance()
    {
        if ((new Vector2(transform.position.x, transform.position.y) - newPosition).magnitude < 0.5f) {
            CanShootAgain = true;
            ReachedLocation();
        }
        else {
            CanShootAgain = false;
        }

        if ((new Vector2(transform.position.x, transform.position.y) - newPosition).magnitude < 0.1f) {
            HasReachedEnd = true;
                hitBox.enabled = false;
        }
        else {
            HasReachedEnd = false;
            previousPosition = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!moveLerp) return;
        if(collision.tag == "Environment") {
            int x, y;
            x = 1;
            y = 1;
            if(transform.position.x > previousPosition.x) {
                x = -1;
            }
            else if (transform.position.y > previousPosition.y) {
                y = -1;
            }
            newPosition = new Vector2(transform.position.x + (x * 0.4f), transform.position.y + (y * 0.4f));
            if(!hasBounced)
                StartCoroutine(BounceTime());
        }
    }

    IEnumerator BounceTime()
    {
        hasBounced = true;
        yield return new WaitForSeconds(0.5f);
        canShootAgain = true;
        hasReachedEnd = true;
        hasBounced = false;
    }

    public void TeleportPlayerToRock()
    {
        platform.SetActive(false);
        player.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.05f);
        RockReturned();
        CanShootAgain = true;
        HasReachedEnd = true;
    }
}
