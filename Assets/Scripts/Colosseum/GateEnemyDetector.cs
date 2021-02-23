using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateEnemyDetector : MonoBehaviour {

    int numberOfEnemiesInCollider;

    public bool GetIsEnemyInCollider()
    {
        if (numberOfEnemiesInCollider > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            numberOfEnemiesInCollider++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            numberOfEnemiesInCollider--;
        }
    }
}
