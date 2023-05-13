using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    List<Enemy> enemies = new();

    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] Vector2 SpawnBounds;

    private void Awake()
    {
        //Start with 3 enemies.
        for (int i = 0; i < 3; i++)
        {
            AddNewEnemy();
        }
    }

    private void AddNewEnemy()
    {
        Vector3 pos = new()
        {
            x = Random.Range(-SpawnBounds.x, SpawnBounds.x),
            y = 0,
            z = Random.Range(-SpawnBounds.y, SpawnBounds.y)
        };

        GameObject spawnedEnemy = Instantiate(enemyPrefab, pos, Quaternion.identity);

        Enemy enemy = spawnedEnemy.GetComponent<Enemy>();

        enemies.Add(enemy);
    }
}
