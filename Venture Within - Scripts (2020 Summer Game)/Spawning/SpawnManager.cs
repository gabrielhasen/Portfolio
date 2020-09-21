using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.CorgiEngine;
using MoreMountains.Tools;
using UnityEngine.SceneManagement;

public class SpawnManager : MMPersistentSingleton<SpawnManager>,
                                MMEventListener<CorgiEngineEvent>
{
    public GameObject player;
    public float spawnTime;

    public int maxNumberOfEnemies;
    public int currentNumberOfEnemies;
    public bool pauseSpawning = false;
    public List<GameObject> allEnemies = new List<GameObject>();
    public List<Block> spawnPoints = new List<Block>();
    public List<Block> nearbySpawnPoints = new List<Block>();

    /// <summary>
    /// A timer that continues to call itself and spawns enemies after the call.
    /// It will stop calling itself the bool pauseSpawning gets called and will 
    /// destroy current enemies
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnTime()
    {
        yield return new WaitForSeconds(spawnTime);
        if (pauseSpawning) {
            ClearEnemies();
        }
        else {
            NearbySpawnPoints();
            SpawnEnemies();
            StartCoroutine(SpawnTime());
        }
    }

    private void NearbySpawnPoints()
    {
        nearbySpawnPoints.Clear();
        int mapHeight = WorldController.Instance.mapHeight;
        Vector2 playerLocation = player.transform.position;
        for (int i = 0; i < spawnPoints.Count; i++) {
            spawnPoints[i].Type = Block.BlockType.Spawn;
            Vector2 spawnLoc = new Vector2(spawnPoints[i].X , (spawnPoints[i].Y + (spawnPoints[i].MapArea * mapHeight) ));
            float distance = Vector2.Distance(playerLocation, spawnLoc);
            if (distance < 18 && distance > 13) {
                spawnPoints[i].Type = Block.BlockType.SpawnPoint;
                nearbySpawnPoints.Add(spawnPoints[i]);
            }
        }
    }

    private void SpawnEnemies()
    {
        if (nearbySpawnPoints.Count <= 0) return;

        Block temp = nearbySpawnPoints[Random.Range(0, nearbySpawnPoints.Count)];
        int mapHeight = WorldController.Instance.mapHeight;
        Vector2 location = new Vector2(temp.X, temp.Y + (temp.MapArea * mapHeight) );

        EnemyData.Instance.GrabEnemy(location, player, maxNumberOfEnemies);
    }

    private void ClearEnemies()
    {
        for (int i = 0; i < EnemyData.Instance.enemiesLoaded.Count; i++) {
            EnemyData.Instance.enemiesLoaded[i].SetActive(false);
        }
    }

    public void PauseSpawning()
    {
        pauseSpawning = true;
    }

    public void UnpauseSpawning()
    {
        pauseSpawning = false;
        StartCoroutine(SpawnTime());
    }

    public void DestroyCurrentEnemies()
    {
        for (int i = 0; i < allEnemies.Count; i++) {
            Destroy(allEnemies[i]);
        }
        allEnemies.Clear();
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
            case CorgiEngineEventTypes.GameOver:
                EndScene();
                break;
            case CorgiEngineEventTypes.LevelEnd:
                EndScene();
                break;
            case CorgiEngineEventTypes.LevelStart:
                StartScene();
                break;
        }
    }

    private void StartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        player = LevelManager.Instance.Players[0].gameObject;
        if (currentScene.name != "Base") {
            UnpauseSpawning();
        }
    }

    private void EndScene()
    {
        PauseSpawning();
        DestroyCurrentEnemies();
    }
}
