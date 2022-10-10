using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private GameObject _specialEnemyPrefab, _healthPackPrefab;
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
            spawnIndex = Random.Range(0, _enemyPrefabs.Length);
            var randomIndex = Random.Range(0, count);
            Instantiate(_enemyPrefabs[spawnIndex], spawnpoints[randomIndex].position, _enemyPrefabs[spawnIndex].transform.rotation);
            enemysLeft++;
        }
        enemysSpawned++;
    }

    public void SpawnHealthPack()
    {
        Vector3 randomPosition = new Vector3 (Random.Range(-healthPoint, healthPoint), 1.0f, Random.Range(-healthPoint, healthPoint));
        Instantiate(_healthPackPrefab, randomPosition, _healthPackPrefab.transform.rotation);
    }

    public void SpawnSpecialEnemy()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-healthPoint, healthPoint), 5.0f, Random.Range(-healthPoint, healthPoint));
        Instantiate(_specialEnemyPrefab, randomPosition, _specialEnemyPrefab.transform.rotation);
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
