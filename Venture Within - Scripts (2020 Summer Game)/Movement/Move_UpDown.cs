using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_UpDown : MonoBehaviour
{
    public float timeToMove;
    public float timeBetween;
    public Vector2 moveAmount;
    public LeanTweenType inTween;
    public LeanTweenType outTween;
    public bool offset;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartWaitToMove());
    }

    private IEnumerator StartWaitToMove()
    {
        if (offset) {
            yield return new WaitForSeconds( Random.Range(0.1f, timeToMove) );
            StartCoroutine(MoveTime());
        }
        else {
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(MoveTime());
        }
    }

    private IEnumerator MoveTime()
    {
        Vector2 movement = new Vector2(gameObject.transform.position.x + moveAmount.x, gameObject.transform.position.y + moveAmount.y);
        StartCoroutine( Tween.MoveGameObjectToandBack(gameObject, movement, timeToMove, inTween, outTween, timeBetween) );
        yield return new WaitForSeconds(timeToMove + timeBetween + 0.02f);
        StartCoroutine(MoveTime());
    }
}
