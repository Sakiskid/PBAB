using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : EnemyController {


    [Header("L4ZER PROPERTIES")]
    [SerializeField] GameObject laser;
    [Space]
    [Header("L4zer Config")]
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float turnSpeedWhileFiring = 0.5f;
    [SerializeField] float chargeTime = 2f;
    [SerializeField] float lazerFireTime = 2f;
    [Header("AI CONFIGURATION")]
    [SerializeField] float attackRange = 5f;
    [SerializeField] State currentState = 0;
    float dazedMoveSpeed = 0.2f;
    enum State { Idle, Follow, Attack, Flee, Daze, FiringLaser };

    bool firingLaser = false;

    void Start()
    {
        Init();
    }

    void Update()
    {
        HandleAIState();
        CalculatePlayerDistance();
    }

    private IEnumerator ChargeLaser()
    {
        firingLaser = true;
        StartCoroutine(ChargeLaserFX());


        StartCoroutine(FireLaser());

        yield return null;
    }

    private IEnumerator ChargeLaserFX()
    {
        yield return new WaitForSeconds(10f);
    }

    private IEnumerator FireLaser()
    {
        laser.SetActive(true);

        yield return new WaitForSeconds(lazerFireTime);
        laser.SetActive(false);
        firingLaser = false;
        yield return null;
    }

    #region Player Detection & AI

    // EXECUTES AI METHODS
    private void HandleAIState()
    {

        // MAKE DECISION
        if (isDazed == true)
            currentState = State.Daze;
        else if (firingLaser == true)
            currentState = State.FiringLaser;
        else if (distanceToPlayer > attackRange)
            currentState = State.Follow;
        else if (distanceToPlayer < attackRange)
            currentState = State.Attack;

        // EXECUTE
        switch (currentState)
        {
            case State.Follow:
                // FOLLOW
                Rotate(PLAYER);
                Move(player.transform.position, MOVETYPE_STRAFE, moveSpeed);
                break;

            case State.Attack:
                // ATTACK
                    StartCoroutine(ChargeLaser());
                break;

            case State.Flee:
                // FLEE
                Rotate(ROTATETYPE_FLEE);
                Move(-player.transform.position, MOVETYPE_DIRECTION, moveSpeed);
                break;

            case State.Daze:
                // DAZE
                // While dazed, move slowly forward or backwards
                Move(transform.position + Vector3.up, MOVETYPE_DIRECTION, dazedMoveSpeed);
                break;
            case State.FiringLaser:
                Rotate(PLAYER);
                break;
        }

    }
    #endregion
}
