 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [Header("EASE TYPES")]
    public LeanTweenType easeFall;
    public LeanTweenType easeComeBack;
    public LeanTweenType easeShake;

    [Header ("MOVEMENT")]
    public Vector2 MoveAmount;
    [Range (0.01f, 3f)] public float fallTime;
    [Range (0.01f, 3f)] public float shakeTime;
    [Range (0.01f, 1f)] public float timeBetweenShakes;
    [Range (0.01f, 3f)] public float waitToGoBackUp;
    [Range (0.01f, 3f)] public float timeToGoBackUp;
    [Range (0.01f, 3f)] public float rechargeTime;


    private Vector2 originalPosition;
    private Vector2 fallToPosition;
    private bool triggerFall;
    private GameObject model;

    private bool shake;

    private void Start()
    {
        model = transform.GetChild(0).gameObject;
        triggerFall = false;
        StartCoroutine(GetPositions());
    }

    private IEnumerator GetPositions()
    {
        yield return new WaitForSeconds(1f);
        originalPosition = gameObject.transform.position;
        fallToPosition = new Vector2(originalPosition.x + MoveAmount.x, originalPosition.y + MoveAmount.y);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (triggerFall)
            return;

        if(collision.tag == "Player") {
            triggerFall = true;
            shake = true;
            StartCoroutine(ShakeTime());
        }
    }

    private IEnumerator ShakeTime()
    {
        ShakeRight();
        yield return new WaitForSeconds(shakeTime);
        shake = false;
        FallTime();
    }

    private void ShakeRight()
    {
        if (!shake)
            return;

        LeanTween.rotateZ(model, -2, timeBetweenShakes).setOnComplete(ShakeLeft);
    }
    private void ShakeLeft()
    {
        if (!shake)
            return;

        LeanTween.rotateZ(model, 2, timeBetweenShakes).setOnComplete(ShakeRight);
    }

    private void FallTime()
    {
        LeanTween.move(model, fallToPosition, fallTime).setOnComplete(WaitTime).setEase(easeFall);
        LeanTween.rotateZ(model, 0, 0.1f);
    }

    private void WaitTime()
    {
        LeanTween.rotateZ(model, 0, 0.1f);
        shake = false;
        StartCoroutine(FallWait());
    }

    private IEnumerator FallWait()
    {
        yield return new WaitForSeconds(waitToGoBackUp);
        LeanTween.move(model, originalPosition, timeToGoBackUp).setOnComplete(BackToOriginal).setEase(easeComeBack);
    }

    private void BackToOriginal()
    {
        StartCoroutine(CanGoAgain());
    }

    private IEnumerator CanGoAgain()
    {
        yield return new WaitForSeconds(rechargeTime);
        triggerFall = false;
    }
}
