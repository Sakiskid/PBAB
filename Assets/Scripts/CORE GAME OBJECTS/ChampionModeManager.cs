using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionModeManager : MonoBehaviour {

    PlayerController player;
    SpawnerController spawnerController;

    private void Start()
    {
        StartCoroutine(PlayerEnterArena());
    }

    public IEnumerator PlayerEnterArena()
    {
        player = FindObjectOfType<PlayerController>();
        spawnerController = FindObjectOfType<SpawnerController>();
        Gate gate = GameObject.Find("Gate Left").GetComponent<Gate>();

        StartCoroutine(gate.ForceOpenGate(1f));
        spawnerController.isSpawning = false;

        yield return new WaitForSeconds(1);
    }
}
