using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class enemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public int numOfEnemies;
    public float spawnRadius = 5.0f;
    private int enemyIndex;
    private int enemyCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        enemyIndex = EnemyType();

        if (enemyCount < numOfEnemies)
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

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Debug.Log("Space key pressed");
            
        //}
    }
}
