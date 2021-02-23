using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] GameObject enemyParentObject;


    private Vector2 GetRandomPositionInArea()
    {
        float xMin = -(transform.localScale.x / 2);
        float xMax = (transform.localScale.x / 2);
        float yMin = -(transform.localScale.y / 2);
        float yMax = (transform.localScale.y / 2);

        float newX = Random.Range(xMin, xMax);
        float newY = Random.Range(yMin, yMax);

        Vector2 position = new Vector2(newX + transform.position.x, newY + transform.position.y);
        return position;
    }

    public void Spawn(GameObject enemy)
    {
        Vector2 newPos = GetRandomPositionInArea();
        GameObject newEnemy = Instantiate(enemy, newPos, Quaternion.identity, enemyParentObject.transform);
    }
}
