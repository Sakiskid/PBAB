using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyTable : ScriptableObject {

    public Enemy[] enemies = new Enemy[1];

    public GameObject GetRandomEnemy()
    {
        List<float> enemyWeights = new List<float>();
        GameObject chosenEnemy = null;
        foreach(Enemy enemy in enemies)
        {
            if (enemy.canSpawn)
            {
                enemyWeights.Add(enemy.randomWeight);
            }
            else if (!enemy.canSpawn)
            {
                enemyWeights.Add(0);        // Adds an empty float to the list to keep the list/array in sync with eachother
            }
        }
        int index = RandomFromDistribution.RandomChoiceFollowingDistribution(enemyWeights);
        chosenEnemy = enemies[index].enemy;
        return chosenEnemy;
    }
}
