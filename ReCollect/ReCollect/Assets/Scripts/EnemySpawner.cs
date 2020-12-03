using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] int enemyCount; //how many will be spawned
    [SerializeField] int alreadySpawned; //how many have spawned so far, will ++ after one dies and another spawns. max = enemyCount
    public GameObject enemy;
    GameObject currentEnemy;
    [SerializeField] bool active;
    [SerializeField] bool currentEnemyAlive;
    [SerializeField] bool waiting;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //enemyCount = 0;
        alreadySpawned = 0;
        //active = true;
        currentEnemyAlive = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //spawns enemy if spawner is active and no enemy has spawned yet or is dead
        if (active && !player.GetComponent<Player>().recollecting)
        {
            //if currentEnemy's health is <= 0, currentEnemyAlive = false
            if (gameObject.name.Contains("Enemy"))
            {
                if (currentEnemy != null && currentEnemy.GetComponent<EnemyController>().respawn && alreadySpawned != enemyCount)
                {
                    print("Spawn another enemy");
                    currentEnemyAlive = false;
                }

                if (!currentEnemyAlive && alreadySpawned < enemyCount)
                {
                    currentEnemy = Instantiate(enemy, transform);
                    currentEnemyAlive = true;
                    alreadySpawned++;
                    //print(currentEnemy.name + " alive status: " + currentEnemy.GetComponent<EnemyController>().alive);
                }
            }
            else
            {
                if (currentEnemy != null && currentEnemy.GetComponent<FriendlyController>().respawn && alreadySpawned != enemyCount)
                {
                    //print("Spawn another friendly");
                    currentEnemyAlive = false;
                }

                if (!currentEnemyAlive && alreadySpawned < enemyCount && !waiting)
                {
                    waiting = true;
                    StartCoroutine(Wait());
                }
            }
        }
    }

    IEnumerator Wait()
    {
        waiting = true;
        yield return new WaitForSeconds(5f);
        waiting = false;
        currentEnemy = Instantiate(enemy, transform);
        currentEnemyAlive = true;
        alreadySpawned++;
        print(currentEnemy.name + " alive status: " + currentEnemy.GetComponent<FriendlyController>().alive);
        waiting = false;
    }
}
