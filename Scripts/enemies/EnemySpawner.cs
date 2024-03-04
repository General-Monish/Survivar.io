using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public int waveQouta;
        public float spawnInterval;
        public int spawnCount;
    }

    [Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        public int spawnCount;
        public GameObject enemyPrefab;
    }

    public List<Wave> waves;
    public int currentWaveCount;

    [Header("Spawner Attributes")]
    float spawnTimer;
    public float waveInterval;
    public int enemiesAlive;
    public int maxEnemiesAllowed;
    public bool maxEnemiesReached=false;

    [Header("Spawn Positions")]
    public List<Transform> relativeSpawnPoints; 

    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        CalculateWaveQouta();
        
    }

    // Update is called once per frame
    void Update()
    {
        // if current wave is ended then start next wave
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0)
        {
            StartCoroutine(BeginNextWave());
        }
        spawnTimer += Time.deltaTime;
        // checking if its time to spawn the nwxt enemy
        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0;
            SpawnEnemies();
        }
        
    }

    IEnumerator BeginNextWave()
    {
        // waiting for waveinterval time b4 next wave starts
        yield return new WaitForSeconds(waveInterval);

        if (currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            CalculateWaveQouta();
        }
    }

    void CalculateWaveQouta()
    {
        int currentWaaveQouta = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaaveQouta += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQouta = currentWaaveQouta;
        Debug.LogWarning(currentWaaveQouta);
    }

    void SpawnEnemies()
    {
        //if the min no of waves of enemies has been spawned
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQouta && !maxEnemiesReached)
        {
            // spawn each type of enemy until qouta is filled
            foreach(var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                // if min no of enemies of this type has been spawned
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    if (enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }
                    Vector2 spawnPos = new Vector2(player.transform.position.x +
                    UnityEngine.Random.Range(-10f, 10f), player.transform.position.y + UnityEngine.Random.Range(-10f, 10f));

                    Instantiate(enemyGroup.enemyPrefab, player.position + 
                    relativeSpawnPoints[UnityEngine.Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);
                    

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }
        // resetting the maxenemiesreached flag if no of enemies alive has dropped below maxcount
        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }
}
