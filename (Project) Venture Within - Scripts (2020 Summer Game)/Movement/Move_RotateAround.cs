using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_RotateAround : MonoBehaviour
{
    public float timeToMove;
    public float timeBetween;
    public Vector3 axisToRotate;
    public float rotateAmount;
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
            yield return new WaitForSeconds(Random.Range(0.1f, timeToMove));
            StartCoroutine(MoveTime());
        }
        else {
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(MoveTime());
        }
    }

    private IEnumerator MoveTime()
    {
        StartCoroutine(Tween.RotateGameObject(gameObject, axisToRotate, rotateAmount, timeToMove, inTween, outTween, timeBetween));
        yield return new WaitForSeconds(timeToMove + timeBetween + 0.2f);
        StartCoroutine(MoveTime());
    }
}
