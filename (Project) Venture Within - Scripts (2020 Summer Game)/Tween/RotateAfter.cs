using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAfter : MonoBehaviour
{
    public float WaitTime;
    public Vector3 RotateAmount;
    void Start()
    {
        StartCoroutine(WaitForSeconds());
    }

    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(WaitTime);
        transform.Rotate(RotateAmount);
    }
}
