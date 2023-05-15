using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    readonly List<Enemy> enemies = new();

    readonly List<WorldSpaceEnemyUI> worldSpaceIndicators = new();

    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private GameObject worldSpaceInfoIndicatorPrefab;

    [SerializeField] Vector2 SpawnBounds;

    private void Awake()
    {
        if (instance is null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Start with 3 enemies.
        for (int i = 0; i < 4; i++)
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

        GameObject indicator = Instantiate(worldSpaceInfoIndicatorPrefab, pos, Quaternion.identity);

        indicator.name = "ENEMY[" + enemies.Count + "]";

        WorldSpaceEnemyUI uiElement = indicator.GetComponent<WorldSpaceEnemyUI>();

        worldSpaceIndicators.Add(uiElement);

        uiElement.LinkEnemy(enemy);

        enemies.Add(enemy);
    }

    public void KillEnemy(Enemy e)
    {
        for (int i = 0; i < worldSpaceIndicators.Count; i++)
        {
            if (worldSpaceIndicators[i].LinkedEnemy == e)
            {
                Destroy(worldSpaceIndicators[i].gameObject);
                worldSpaceIndicators.Remove(worldSpaceIndicators[i]);
                HUDManager.instance.AddToKillCount();
                AddNewEnemy();
            }
        }

        Destroy(e.gameObject, 10);
    }
}