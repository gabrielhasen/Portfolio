using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    public GameObject explosionArea;

    private void Awake()
    {
        explosionArea.SetActive(false);
    }

    public void Explode()
    {
        StartCoroutine(PlayExplosion());
    }

    private IEnumerator PlayExplosion()
    {
        yield return new WaitForSeconds(1.4f);
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        explosionArea.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().enabled = true;
        explosionArea.SetActive(false);
    }
}
