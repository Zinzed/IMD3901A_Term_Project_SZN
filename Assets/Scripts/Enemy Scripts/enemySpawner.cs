using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class enemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public int numOfEnemies;
    public float spawnRadius;
    private int enemyIndex;
    private int enemyCount;

    private bool canSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        enemyIndex = EnemyType();

        if (canSpawn)
        {
            Spawn();
        }   
    }

    int EnemyType()
    {
        return Random.Range(0, enemies.Length);
    }

    void Spawn()
    {
        Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
        Instantiate(enemies[enemyIndex], spawnPos, Quaternion.identity);
        enemyCount++;

        if (enemyCount == numOfEnemies)
        {
            canSpawn = false;
            //Debug.Log("Max enemies spawned");
        }
    }
}
