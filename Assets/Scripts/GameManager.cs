using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour {

    [Header("ENVIRONMENT")]
    public EnvironmentTable environmentTable;

    [Header("PLAYER")]
    public Player player;

    [Space]
    [Header("ENEMIES")]
    public WeaponTable swingerItemTable;

    [Space]
    [Header("SURVIVAL")]
    public EnemyTable enemySpawnTable;

    [Space]
    [Header("CHAMPIONSHIP")]
    public EnemyTable easyEnemies;
    public EnemyTable mediumEnemies;
    public EnemyTable hardEnemies;
    public EnemyTable bossEnemies;
    public EnemyTable ultraEnemies;


    private void Start()
    {
        player.health = player.initialHealth;
    }
}
