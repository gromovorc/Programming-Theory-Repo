using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject specialEnemyPrefab;
    [SerializeField] private GameObject healthPackPrefab;
    private int spawnIndex, count;
    private float healthPoint = 9.0f;
    private Transform[] spawnpoints;

    public int waveCount = 1, enemysSpawned = 5, enemysLeft = 5;

    private void Start()
    {
        count = transform.childCount;
        spawnpoints = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            spawnpoints[i] = transform.GetChild(i);
        }
        SpawnEnemys();
    }

    private void SpawnEnemys()
    {
        for (int i = 0; i < enemysSpawned; i++)
        {
            spawnIndex = Random.Range(0, enemyPrefabs.Length);
            var randomIndex = Random.Range(0, count);
            Instantiate(enemyPrefabs[spawnIndex], spawnpoints[randomIndex].position, enemyPrefabs[spawnIndex].transform.rotation);
            enemysLeft++;
        }
        enemysSpawned++;
    }

    public void SpawnHealthPack()
    {
        Vector3 randomPosition = new Vector3 (Random.Range(-healthPoint, healthPoint), 1.0f, Random.Range(-healthPoint, healthPoint));
        Instantiate(healthPackPrefab, randomPosition, healthPackPrefab.transform.rotation);
    }

    public void SpawnSpecialEnemy()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-healthPoint, healthPoint), 5.0f, Random.Range(-healthPoint, healthPoint));
        Instantiate(specialEnemyPrefab, randomPosition, specialEnemyPrefab.transform.rotation);
    }

    public void EnemyDead()
    {
        enemysLeft--;
        if (enemysLeft < 1)
        {
            if (waveCount % 2 == 0) SpawnSpecialEnemy();
            waveCount++;
            SpawnHealthPack();
            SpawnEnemys();
        }
    }
}
