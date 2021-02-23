using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaFighter : EnemyController {

    [Header("ALPHA FIGHTER PROPERTIES")]
    [Space]
    [SerializeField] BrawlerHand leftHand;
    [SerializeField] BrawlerHand rightHand;
    [Header("Alpha Fighter Config")]
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] public int punchDamage;
    [SerializeField] public int maxPunchDamage;
    [Header("AI CONFIGURATION")]
    [SerializeField] float attackRange = 5f;
    [SerializeField] int currentState = 0;

    // Misc Brawler Properties
    float dazedMoveSpeed = 0.2f;

    IEnumerator punchCoroutine;





    void Start()
    {
        Init();
    }

    void Update()
    {
        HandleAIState();
        CalculatePlayerDistance();
    }


    #region Player Detection & AI

    // EXECUTES AI METHODS
    private void HandleAIState()
    {

        // MAKE DECISION
        if (isDazed == true)
        {
            currentState = 5;
        }
        else if (leftHand.isBroken && rightHand.isBroken)
        {
            currentState = 4;
        }
        else if (distanceToPlayer > attackRange)
        {
            currentState = 2;
        }
        else if (distanceToPlayer < attackRange)
        {
            currentState = 3;
        }


        switch (currentState)
        {
            case 2:
                // FOLLOW
                Rotate(PLAYER);
                Move(player.transform.position, MOVETYPE_STRAFE, moveSpeed);
                StopPunching();
                break;
            case 3:
                // ATTACK
                Rotate(PLAYER);
                StartPunching();
                break;
            case 4:
                // FLEE
                Rotate(ROTATETYPE_FLEE);
                Move(-player.transform.position, MOVETYPE_DIRECTION, moveSpeed);
                StopPunching();
                break;
            case 5:
                // DAZE
                // While dazed, move slowly forward or backwards
                Move(transform.position + Vector3.up, MOVETYPE_DIRECTION, dazedMoveSpeed);
                break;
        }

    }
    #endregion


    void StartPunching()
    {
        // Picks a random attack and then attacks
        int punchIndex = Random.Range(0, 2);
        animator.SetInteger("PunchIndex", punchIndex);
        animator.SetBool("isEnemyPunching", true);

        // Set left or right punch on hands. This prevents damage while the hand isn't punching
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Punch_Left"))
        {
            rightHand.isPunching = false;
            leftHand.isPunching = true;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Punch_Right"))
        {
            rightHand.isPunching = true;
            leftHand.isPunching = false;
        }
    }

    void StopPunching()
    {
        animator.SetBool("isEnemyPunching", false);
        rightHand.isPunching = false;
        leftHand.isPunching = false;
    }
}
