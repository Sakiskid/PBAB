using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour {

    [Header("Configurations")]
    [SerializeField] public bool isSpawning = true;
    [SerializeField] float spawnDelay;
    [SerializeField] int maxEnemyAmt;

    // References
    [SerializeField] EnemySpawner[] spawners;
    GameManager gameManager;
    IEnumerator spawnCoroutine;
    GameObject currentEnemy;


    void Start() {
        gameManager = FindObjectOfType<GameManager>();
        spawnCoroutine = SendSpawnSignal();
        StartCoroutine(spawnCoroutine);
    }

    // ------------------------------------------------------------------------------------------\\
    #region Public Methods
    // These two 'set' functions were created for the sandbox hard mode
    public void SetSpawnDelay(float newDelay)
    {
        spawnDelay = newDelay;
    }

    public void SetMaxEnemies(int maxEnemies)
    {
        maxEnemyAmt = maxEnemies;
    }
    #endregion

    // --------------------------------------------------------------------------------------- \\
    #region Weighted array and enemy-choosing

    void SpawnEnemyFromTable(EnemySpawner currentSpawner)
    {
        // Chooses a random index based on weights, then sets currentEnemy to the enemyIndex of the enemyContainerPrefabs list
        currentSpawner.Spawn(gameManager.enemySpawnTable.GetRandomEnemy());
    }
    #endregion

    #region send spawns and find enemy amount
    IEnumerator SendSpawnSignal()
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            if (FindEnemyAmount() < maxEnemyAmt && isSpawning)
            {
                int index = Random.Range(0, spawners.Length);
                EnemySpawner currentSpawner = spawners[index];

                SpawnEnemyFromTable(currentSpawner);
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    int FindEnemyAmount()
    {
        int currentEnemies = 0;
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            currentEnemies++;
        }
        return currentEnemies;
    }
    #endregion
}
