using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swinger : EnemyController {

    [Header("SWINGER PROPERTIES")]
    WeaponTable swingerItemTable;
    [Space]
    [Header("Swinger Config")]
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float spinSpeed = 1f;
    [SerializeField] Vector2 weaponOffset;
    [Header("AI CONFIGURATION")]
    [SerializeField] float attackRange = 5f;
    [SerializeField] State currentState = 0;
    [SerializeField] float minimumDistance = 1f;
    enum State {Idle, Follow, Attack, Flee, Daze};

    GameObject initialWeapon;
    DamageDealer initialWeaponDamageDealer;
    float dazedMoveSpeed = 0.2f;
    bool holdingWeapon;
    FixedJoint2D fj2D;

    void Start()
    {
        Init();
        fj2D = GetComponent<FixedJoint2D>();

        swingerItemTable = FindObjectOfType<GameManager>().swingerItemTable;
        SpawnWithWeapon();
        initialWeaponDamageDealer = initialWeapon.GetComponent<DamageDealer>();
    }

    void Update()
    {
        CalculatePlayerDistance();
        CheckIfHoldingWeapon();
    }

    private void FixedUpdate()
    {
        HandleAIState();
    }

    private void CheckIfHoldingWeapon()
    {
        if ((!fj2D || isDying) && holdingWeapon)
        {
            holdingWeapon = false;
            initialWeaponDamageDealer.SetWhoCanBeDamaged(true, false, false);
        }
    }

    void SpawnWithWeapon()
    {
        GameObject weapon = Instantiate(
            swingerItemTable.GetRandomItem(),
            transform.position,
            transform.rotation);
        Rigidbody2D weaponRB = weapon.GetComponent<Rigidbody2D>();
        initialWeapon = weapon;
        weapon.GetComponent<DamageDealer>().SetWhoCanBeDamaged(false, true, true);

        fj2D.connectedBody = weaponRB;
        fj2D.anchor = weaponOffset;

        holdingWeapon = true;
    }

    void Spin()
    {
        rb2D.angularVelocity = spinSpeed;
    }


    #region Player Detection & AI

    // EXECUTES AI METHODS
    private void HandleAIState()
    {

        // MAKE DECISION
        if (isDazed == true)
            currentState = State.Daze;
        else if (!holdingWeapon)
            currentState = State.Flee;
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
                Spin();
                if (distanceToPlayer > minimumDistance)
                {
                    Move(player.transform.position, MOVETYPE_STRAFE, moveSpeed);
                } 
                else if (distanceToPlayer < minimumDistance)
                {
                    Move(-player.transform.position, MOVETYPE_STRAFE, moveSpeed);
                }
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
        }

    }
    #endregion
}
