using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnerController : MonoBehaviour {

    // Config Variables
    [SerializeField] float universalScoreMultiplier = 1f;
    [SerializeField] public bool isSpawning = true;
    [SerializeField] float spawnDelay;
    [SerializeField] int maxEnemyAmt;

    // Private Variables
    enum spawnScoreIndex { easy, medium, hard, boss, ultra};
    float[] intrinsicScoreMultipler = { 1, 0.25f, 0.08f, 0.02f, 0.01f };
    float spawnScoreThreshold = 1f;

    float[] currentSpawnScores = { 0, 0, 0, 0, 0 };
    int currentWave = 0;
    int currentwaveDifficulty = 0;      //Used to calculate difficulty of wave. todo: implement rewards, etc, based on wave difficulty
    int minimumEnemiesThisWave;
    int maximumEnemiesThisWave;

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

    public float[] GetCurrentSpawnScores()
    {
        return currentSpawnScores;
    }
    #endregion

    // --------------------------------------------------------------------------------------- \\
    #region Wave Control

    private void UpdateCurrentSpawnScores()
    {
        for(int i = 0; i < currentSpawnScores.Length; i++)
        {
            currentSpawnScores[i] += (intrinsicScoreMultipler[i] * universalScoreMultiplier * currentWave);
        }
    }

    private void NextWave()
    {
        currentWave++;
    }

#endregion

#region send spawns and find enemy amount

    private GameObject GetEnemyToSpawnBasedOnDifficulty()
    {
        // If there are enough points to spawn this difficulty enemy, then spawn and subtract spawnScoreThreshold

        // ULTRA
        if (currentSpawnScores[(int)spawnScoreIndex.ultra] >= spawnScoreThreshold)
        {
            currentSpawnScores[(int)spawnScoreIndex.ultra] -= spawnScoreThreshold;
            return gameManager.ultraEnemies.GetRandomEnemy();
        }
        // HARD
        else if (currentSpawnScores[(int)spawnScoreIndex.hard] >= spawnScoreThreshold)
        {
            currentSpawnScores[(int)spawnScoreIndex.hard] -= spawnScoreThreshold;
            return gameManager.hardEnemies.GetRandomEnemy();
        }
        // MEDIUM
        else if (currentSpawnScores[(int)spawnScoreIndex.medium] >= spawnScoreThreshold)
        {
            currentSpawnScores[(int)spawnScoreIndex.medium] -= spawnScoreThreshold;
            return gameManager.mediumEnemies.GetRandomEnemy();
        }
        // EASY
        else if (currentSpawnScores[(int)spawnScoreIndex.easy] >= spawnScoreThreshold)
        {
            currentSpawnScores[(int)spawnScoreIndex.easy] -= spawnScoreThreshold;
            return gameManager.easyEnemies.GetRandomEnemy();
        }

        return null;
    }

    void SpawnEnemyFromTable(EnemySpawner currentSpawner)
    {
        // Chooses a random index based on weights, then sets currentEnemy to the enemyIndex of the enemyContainerPrefabs list
        currentSpawner.Spawn(GetEnemyToSpawnBasedOnDifficulty());
    }

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
