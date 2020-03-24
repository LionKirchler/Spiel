using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private Player player = null;
    private float nextSpawn = 0;
    private float spawnDelay = 3f;
    private float zombieSpawnInterval = 3;
    private List<GameObject> enemys = new List<GameObject>(100);
    private List<GameObject> zombiePool = new List<GameObject>(100);

    public void Start()
    {
        nextSpawn = Time.time + 60;
        player = GetComponent<Player>();
        fillPool(1000);
    }

    public void Update()
    {
        if (Time.time >= nextSpawn)
        {
            spawnEnemys();
            nextSpawn = Time.time + spawnDelay;
            if (spawnDelay >= 0.7)
            {
                spawnDelay -= 0.03f;
            }
        }
    }


    /// <summary>
    /// adds a enemy to the enemy list
    /// </summary>
    /// <param name="go">enemy to add</param>
    public void addEnemy(GameObject go)
    {
        enemys.Add(go);
    }

    /// <summary>
    /// remove a enemy from the enemy list
    /// </summary>
    /// <param name="go">enemy to remove</param>
    public void removeEnemy(GameObject go)
    {
        try
        {
            enemys.Remove(go);
        }
        catch { }
    }
    /// <summary>
    /// remove a enemy from the enemy list by index
    /// </summary>
    /// <param name="i">index to remove at</param>
    public void removeEnemy(int i)
    {
        enemys.RemoveAt(i);
    }

    /// <summary>
    /// spawns random enemys
    /// </summary>
    public void spawnEnemys()
    {

        //for (int i = 0; i < Mathf.Floor(zombieSpawnInterval); i++)
        //{
            int r = Random.Range(0, player.getMapGrid().getEmptyGrids().Count);
            getEnemyFromPool(player.getMapGrid().getEmptyGrids()[r].getPos());
            //getEnemyFromPool(player.getMapGrid().getGrid()[0, 0].getPos());
        //}
    }

    /// <summary>
    /// puts a enemy back in the pool
    /// </summary>
    /// <param name="enemy">enemy</param>
    public void poolEnemy(GameObject enemy)
    {
        Enemy enemycomp = enemy.GetComponent<Enemy>();
        removeEnemy(gameObject);
        enemycomp.getGridOn().removeEnemyOn(gameObject);
        enemycomp.resetStats();
        enemy.SetActive(false);
        zombiePool.Add(enemy);
    }

    /// <summary>
    /// get enemy from pool
    /// </summary>
    /// <param name="pos">pos</param>
    /// <returns>Enemy</returns>
    public GameObject getEnemyFromPool(Vector2 pos)
    {
        GameObject enemy = zombiePool[0];
        zombiePool.RemoveAt(0);
        enemy.SetActive(true);
        enemy.transform.position = pos;
        enemy.GetComponent<Enemy>().setGridOn();
        addEnemy(enemy);
        return enemy;
    }

    /// <summary>
    /// fills the enemy pool with enemys
    /// </summary>
    /// <param name="amount">amount of enemys</param>
    public void fillPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject enemy = player.getSpawner().spawnZombie1(Vector2.zero);
            enemy.SetActive(false);
            zombiePool.Add(enemy);
        }
    }

}
