using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTime : MonoBehaviour
{
    [Header("Time Between")]
    public float maxRandomTime;
    public float minRandomTime;
    [Header("Shake")]
    public float maxShakeAmount;
    public float minShakeAmount;

    private Vector2 ScaleAmount;
    // Start is called before the first frame update
    void Start()
    {
        ScaleDown();
    }

    private void ScaleUp()
    {
        float Scale = Random.Range(1, maxShakeAmount);
        LeanTween.scale(gameObject, new Vector2(Scale, Scale), Random.Range(minRandomTime, maxRandomTime)).setOnComplete(ScaleDown);
    }

    private void ScaleDown()
    {
        float Scale = Random.Range(minShakeAmount, 1);
        LeanTween.scale( gameObject, new Vector2(Scale, Scale), Random.Range(minRandomTime, maxRandomTime) ).setOnComplete(ScaleUp);
    }
}
