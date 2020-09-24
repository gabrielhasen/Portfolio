using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyDropper : MonoBehaviour
{
    public void DropCurrency(Vector2 dropPos)
    {
        int CurrencyToDrop = Random.Range(2, 5);
        for (int i = 0; i < CurrencyToDrop; i++) {
            StartCoroutine(BetweenWait(dropPos));
        }
    }

    private IEnumerator BetweenWait(Vector2 dropPos)
    {
        yield return new WaitForSeconds(Random.Range(0.2f,0.5f));
        CreateCurrency(dropPos);
    }

    private void CreateCurrency(Vector2 dropPos)
    {
        GameObject temp = ItemData.Instance.GrabCurrencyItem();
        GameObject currency = Instantiate(temp, dropPos, Quaternion.identity, this.transform);
        ThrowCurrency(currency);
    }

    private void ThrowCurrency(GameObject currency)
    {
        currency.AddComponent<BoxCollider2D>();
        Rigidbody2D rb = currency.AddComponent<Rigidbody2D>();
        Vector2 ForceVector = new Vector2(Random.Range(-6f, 6f), Random.Range(9f, 12f));
        rb.AddForce(ForceVector, ForceMode2D.Impulse);
    }
    private IEnumerator waitforsec(GameObject currency)
    {
        yield return new WaitForSeconds(1f);

    }
}
