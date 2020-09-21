using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossFinish : MonoBehaviour
{
    private GameObject player;
    private bool hasBeenTriggered;
    private GameObject Crystal;
    private GameObject CrystalExplostion_Particles;
    

    private void Start()
    {
        hasBeenTriggered = false;
        Crystal = transform.GetChild(0).gameObject;
        CrystalExplostion_Particles = transform.GetChild(1).gameObject;
        CrystalExplostion_Particles.SetActive(false);
    }

    public void BossKilled()
    {

    }

    private void TriggeredEnding()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            if (!hasBeenTriggered) {
                hasBeenTriggered = true;
                player = collision.gameObject;
                Outro();
            }
        }
    }

    private void Outro()
    {
        CutsceneManager.Instance.StopPlayerMovement();
        StartCoroutine(BreakOut());
    }

    IEnumerator BreakOut()
    {
        Crystal.GetComponent<ShakeObject>().StartShake(2.5f, 0.05f);
        yield return new WaitForSeconds(2.6f);
        CrystalExplostion_Particles.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        Crystal.SetActive(false);
        PullRandomGeomancer();
    }

    private void PullRandomGeomancer()
    {
        int randomValue = Random.Range(0, 2);
        Debug.Log(randomValue);

        //If 0 then unlock geomancer
        if(randomValue == 0) {
            GameObject temp = GeomancerManager.Instance.GetLockedGeomancer();
            if (temp != null) {
                Instantiate(temp, Crystal.transform.position, Quaternion.identity);
            }
            //DIALOGUE HERE IF YOU FIND A GEOMANCER
        }
        //If 1 then pick up currency
        else {
            //DIALOGUE HERE IF YOU DON'T FIND A GEOMANCER
            PlayerInventory.Instance.CurrencyUp(Random.Range(4,7));
        }

        StartCoroutine(DialogueWait());
    }

    IEnumerator DialogueWait()
    {
        yield return new WaitForSeconds(3f);
        ReturnToSub();
    }

    private void ReturnToSub()
    {
        MMGameEvent.Trigger("Save");
        CorgiEngineEvent.Trigger(CorgiEngineEventTypes.LevelEnd);
        LoadingSceneManager.LoadScene("Base");
    }
}
