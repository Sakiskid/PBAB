using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy {

    public GameObject enemy;
    public bool canSpawn;
    [SerializeField] [Range(0, 500)] public int randomWeight;

}
