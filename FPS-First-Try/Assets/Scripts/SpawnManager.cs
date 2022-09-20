using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject healthPackPrefab;
    private int spawnIndex, count;
    private float healthPoint = 9.0f;
    private Transform[] spawnpoints;

    private EnemyController enemy;
    public int waveCount, enemysSpawned = 5, enemysLeft = 5;

    private void Start()
    {
        enemy = enemyPrefabs[0].GetComponent<EnemyController>();
        count = transform.childCount;
        spawnpoints = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            spawnpoints[i] = transform.GetChild(i);
        }
        SpawnEnemys();
    }
    private void Update()
    {
        if (enemysLeft < 1)
        {
            waveCount++;
            if (enemysSpawned > 20)
            {
                enemy.health += waveCount;
                enemy.damage += Mathf.RoundToInt(waveCount / 2);
                enemysSpawned = 5;
            }
            SpawnHealthPack();
            SpawnEnemys();
        }
    }

    public void SpawnEnemys()
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
}
