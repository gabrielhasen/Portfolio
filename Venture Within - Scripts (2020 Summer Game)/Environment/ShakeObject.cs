using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    public float shakeTime;
    public float timeBetweenShakes;
    private bool shake;

    private void Start()
    {
        shake = true;
    }

    public void StartShake() { StartShake(shakeTime, timeBetweenShakes); }
    public void StartShake(float _shakeTime, float _timeBetweenShakes)
    {
        shakeTime = _shakeTime;
        timeBetweenShakes = _timeBetweenShakes;
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        ShakeRight();
        yield return new WaitForSeconds(shakeTime);
        shake = false;
        EndShake();
    }

    private void ShakeRight()
    {
        if (!shake)
            return;

        LeanTween.rotateZ(gameObject, -2, timeBetweenShakes).setOnComplete(ShakeLeft);
    }
    private void ShakeLeft()
    {
        if (!shake)
            return;

        LeanTween.rotateZ(gameObject, 2, timeBetweenShakes).setOnComplete(ShakeRight);
    }

    private void EndShake()
    {

    }
}
