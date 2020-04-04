using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    int desiredEnemiesInGame = 50;
    int currentEnemiesInGame = 0;
    public GameObject enemy;
    public int xPos;
    public int zPos;
    public int enemyCount;
    void Start()
    {
        currentEnemiesInGame = 0;
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        while (enemyCount < 10)
        {
            xPos = Random.Range(1, 50);
            zPos = Random.Range(1, 31);
            Instantiate(enemy, RandPos(), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }

    Vector3 RandPos()
    {
        Vector3 pos = new Vector3(Random.Range(-10.0f, 10f), 1, Random.Range(-10.0f, 10f));
        return pos;
    }
}
