using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;

public class EnemyData :    MMPersistentSingleton<EnemyData>,
                            MMEventListener<CorgiEngineEvent>
{
    public List<GameObject> enemyList;
    public List<GameObject> enemiesLoaded;
    public int currentEnemiesSpawned;
    
    /// <summary>
    /// On awake loads all rooms from Resources/Rooms folder
    /// </summary>
    private void LoadEnemies()
    {
        enemyList = new List<GameObject>();
        Object[] tempClass = Resources.LoadAll<GameObject>("Enemies");
        foreach (GameObject item in tempClass) {
            enemyList.Add(item);
        }
    }

    public void GrabEnemy(Vector2 pos, GameObject player, int maxNumberEnemies)
    {
        //If enemy distance is greater than some distance from player, then move that enemy
        for (int i = 0; i < enemiesLoaded.Count; i++) {
            GameObject tempGO = enemiesLoaded[i];
            if (!tempGO.activeInHierarchy) {

            }
            else if (Vector2.Distance(tempGO.transform.position, player.transform.position) > 38f) {
                Health tempHP = tempGO.GetComponent<Health>();
                tempHP.Revive();
                tempGO.transform.position = pos;
                tempGO.SetActive(true);
                GetCurrentEnemiesSpawned();
                return;
            }

        }

        //If current enemies spawned are greater than max
        if (maxNumberEnemies <= GetCurrentEnemiesSpawned()) {
            return;
        }

        //Loads in gameobjects
        for (int i = 0; i < enemiesLoaded.Count; i++) {
            GameObject tempGO = enemiesLoaded[i];
            if (!tempGO.activeInHierarchy) { 
                Health tempHP = tempGO.GetComponent<Health>();
                tempHP.Revive();
                tempGO.transform.position = pos;
                tempGO.SetActive(true);
                return;
            }
        }
    }

    public int GetCurrentEnemiesSpawned()
    {
        int tempInt = 0;
        for (int i = 0; i < enemiesLoaded.Count; i++) {
            GameObject tempGO = enemiesLoaded[i];
            if (tempGO.activeInHierarchy) {
                tempInt++;
            }
        }
        currentEnemiesSpawned = tempInt;
        return currentEnemiesSpawned;
    }

    private void LoadInEnemies()
    {
        for (int i = 0; i < enemyList.Count; i++) {
            int amount = enemyList[i].GetComponent<SpawnAmount>().AmountToSpawn;
            for (int j = 0; j < amount; j++) {
                GameObject temp = Instantiate(enemyList[i], new Vector2(-100, -100), Quaternion.identity, gameObject.transform);
                temp.SetActive(false);
                enemiesLoaded.Add(temp);
            }
        }
        Shuffle(enemiesLoaded);
    }

    private void DestroyLoadedEnemies()
    {
        for (int i = 0; i < enemiesLoaded.Count; i++) {
            Destroy(enemiesLoaded[i]);
        }
        enemiesLoaded.Clear();
    }

    public static void Shuffle(List<GameObject> alpha)
    {
        for (int i = 0; i < alpha.Count; i++) {
            GameObject temp = alpha[i];
            int randomIndex = Random.Range(i, alpha.Count);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }
    }

    void OnEnable()
    {
        this.MMEventStartListening<CorgiEngineEvent>();
    }
    void OnDisable()
    {
        this.MMEventStopListening<CorgiEngineEvent>();
    }

    public virtual void OnMMEvent(CorgiEngineEvent engineEvent)
    {
        switch (engineEvent.EventType) {
            case CorgiEngineEventTypes.LevelStart:
                StartScene();
                break;
            case CorgiEngineEventTypes.GameOver:
                EndScene();
                break;
            case CorgiEngineEventTypes.LevelEnd:
                EndScene();
                break;
        }
    }

    private void StartScene()
    {
        LoadEnemies();
        LoadInEnemies();
    }

    private void EndScene()
    {
        DestroyLoadedEnemies();
    }
}
