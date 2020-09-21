using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDetector : MonoBehaviour
{
    public List<GameObject> spawnPoint = new List<GameObject>();
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "SpawnPoint") {
            spawnPoint.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "SpawnPoint") {
            spawnPoint.Remove(collision.gameObject);
        }
    }
}
