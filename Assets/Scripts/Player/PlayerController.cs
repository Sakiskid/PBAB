using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    [Header("References")]
    Player player;

    DistanceJoint2D dj2D;
    Rigidbody2D rb2D;
    Collider2D playerCollider;
    Grabbable[] nearbyGrabbableObjects;
    GameObject closestGrabbable = null;
    Transform currentGrabbable;
    Transform currentWeaponSecondary;

    public float rotationSpeed;
    float mouseSensitivity = 0.7f;
    float waitTimeAfterDeath = 5f;
    float currentStamina;
    float timeSinceLastSprint;
    bool isHoldingGrabbable = false;
    bool isDead;
    bool isSprinting;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        dj2D = GetComponent<DistanceJoint2D>();
        playerCollider = GetComponent<Collider2D>();
        player = FindObjectOfType<GameManager>().player;
        rotationSpeed = player.rotationSpeed;

        currentStamina = player.maxStaminaInSeconds;
    }

    // Update is called once per frame
    void Update ()
    {
        HandleGrabbing();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    public void TakeDamage(int incomingDamage)
    {
        FMODUnity.RuntimeManager.PlayOneShot(player.hurtSound);
        player.health -= incomingDamage;
        if(player.health <= 0) { StartCoroutine(Die()); }
    }

    private IEnumerator Die()
    {
        FMODUnity.RuntimeManager.PlayOneShot(player.deathSound);
        Instantiate(player.deathParticles,transform);
        Cursor.lockState = CursorLockMode.None;
        isDead = true;                                          // Prevents movement after dewath
        playerCollider.enabled = false;                         // Prevents collision
        yield return new WaitForSeconds(waitTimeAfterDeath);
        FindObjectOfType<LevelLoader>().LoadScene(1);
    }

    #region Weapon Control
    private void HandleGrabbing()
    {
        DamageDealer damageDealer;

        // This line fixes the bug where if a brawler dies while holding him you can't pick up another weapon
        if(currentGrabbable == null) { isHoldingGrabbable = false; } 

        // Grab an object
        if (Input.GetKeyDown(KeyCode.F) || Input.GetButton("Fire1") && !isHoldingGrabbable)
        {
            GetClosestGrabbableObject();
            if (closestGrabbable != null)
            {
                currentGrabbable = GetClosestGrabbableObject().transform;
                damageDealer = currentGrabbable.GetComponent<DamageDealer>();
                if (damageDealer) { damageDealer.SetWhoCanBeDamaged(true, false, true); }

                currentGrabbable.GetComponent<Grabbable>().Grab(transform);
                isHoldingGrabbable = true;
                dj2D.connectedBody = currentGrabbable.GetComponent<Rigidbody2D>();
                dj2D.distance = player.weaponOffset.x;
                dj2D.enabled = true;
                closestGrabbable = null;
            }
        }

        // Drop an object
        if (Input.GetKeyDown(KeyCode.G) || Input.GetButton("Fire2"))
        {
            if (currentGrabbable != null)
            {
                damageDealer = currentGrabbable.GetComponent<DamageDealer>();
                if (damageDealer) { damageDealer.SetWhoCanBeDamaged(true, false, true); }
                currentGrabbable.GetComponent<Grabbable>().Drop();
            }
                isHoldingGrabbable = false;
                dj2D.connectedBody = null;
                dj2D.enabled = false;
                currentGrabbable = null;
        }
    }

    private GameObject GetClosestGrabbableObject()
    {
        // This finds and returns the closest weapon!
        float distance = Mathf.Infinity; // for creating the distance variable
                                         // For creating a closest variable. these 2 lines are done to prevent errors in the equations
        nearbyGrabbableObjects = FindObjectsOfType<Grabbable>();
        foreach (Grabbable grabbable in nearbyGrabbableObjects)
        {
            Vector3 difference = grabbable.transform.position - transform.position;
            float currentDistance = difference.sqrMagnitude;
            if (currentDistance < distance && currentDistance < player.maxGrabRange)
            {
                closestGrabbable = grabbable.gameObject;
                distance = currentDistance;
            }
        }
        return closestGrabbable;
    }
    #endregion

    #region MOVEMENT
    private void HandleMovement()
    {
        if (!isDead)
        {
            RotateTowardsCursor();
            Run();
        }
    }

    private void RotateTowardsCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorldPosition - new Vector2(transform.position.x, transform.position.y);
        //rb2D.angularVelocity = Vector2.SignedAngle(transform.up, direction) * 25;

        player.rotationObject.transform.up = direction;
        //rb2D.AddTorque(Quaternion.Slerp(transform.rotation, player.rotationObject.transform.rotation, player.rotationSpeed * Time.fixedDeltaTime).eulerAngles.z);
        rb2D.MoveRotation(Quaternion.Slerp(transform.rotation, player.rotationObject.transform.rotation, player.rotationSpeed * Time.fixedDeltaTime).eulerAngles.z);
    }

    private void RotateTowardsEnemy()
    {
        if (FindNearestEnemy())
        {
            player.rotationObject.transform.up = -(transform.position - FindNearestEnemy().transform.position);
        }
        rb2D.MoveRotation(Quaternion.Slerp
            (transform.rotation, player.rotationObject.transform.rotation, player.rotationSpeed * Time.fixedDeltaTime).eulerAngles.z);
    }

    private void Run()
    {
        // WASD Movement
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        float step = Time.fixedDeltaTime * player.speedMultiplier * 100;
        Vector2 direction = new Vector2(horiz, vert);
        
        // Am I sprinting or running?
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            isSprinting = true;
            Sprint(direction, step);
        }
        else
        {
            isSprinting = false;
            rb2D.AddForce(direction * step);
            timeSinceLastSprint += Time.deltaTime;
        }

        // Recharge Sprint if stamina is not max, and enough time has passed since last sprint
        if (currentStamina <= player.maxStaminaInSeconds && timeSinceLastSprint > player.staminaRegenDelay)
        {
            currentStamina += player.staminaRegenPerSecond * Time.deltaTime;
        }
    }

    private void MouseRun()
    {
        //float vertical = Input.GetAxis("Mouse Y") * player.speedMultiplier;
        //float horizontal = Input.GetAxis("Mouse X") * player.speedMultiplier;
        float vertical = Input.GetAxis("Mouse Y");
        float horizontal = Input.GetAxis("Mouse X");
        rb2D.AddForce(new Vector2(horizontal, vertical) * Time.fixedDeltaTime * player.speedMultiplier * 100);

        /* NEW MOVEMENT CODE IN PROGRESS
         * 
        float vertical = Input.GetAxis("Mouse Y");
        float horizontal = Input.GetAxis("Mouse X");
        //rb2D.AddForce(new Vector2(horizontal, vertical) * Time.fixedDeltaTime * player.speedMultiplier * 100);

        float absVertical = Mathf.Abs(Input.GetAxis("Mouse Y"));
        float absHorizontal = Mathf.Abs(Input.GetAxis("Mouse X"));
        float newVertical = 0;
        float newHorizontal = 0;

        newVertical =
            Mathf.Clamp(absVertical * player.speedMultiplier, 0, player.maxRunSpeed) * Mathf.Sign(Input.GetAxis("Mouse Y"));
        newHorizontal =
            Mathf.Clamp(absHorizontal * player.speedMultiplier, 0, player.maxRunSpeed) * Mathf.Sign(Input.GetAxis("Mouse X"));

        Vector2 runDirection = new Vector2(newHorizontal, newVertical);
        Debug.Log("Mouse X = " + Input.GetAxis("Mouse X") + " || horizontal = " + newHorizontal);
        rb2D.AddForce(runDirection * Time.fixedDeltaTime);
        */

    }

    private void Sprint(Vector2 direction, float step)
    {
        rb2D.AddForce(direction * step * player.sprintMultiplier);
        currentStamina -= 1 * Time.deltaTime;
        timeSinceLastSprint = 0;
    }
    #endregion

    private GameObject FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float currentDistance = Mathf.Infinity;
        float newDistance = 0;
        EnemyController[] enemiesOnLevel = FindObjectsOfType<EnemyController>();
        foreach(EnemyController enemy in enemiesOnLevel)
        {
            newDistance = (transform.position - enemy.transform.position).magnitude;
            if(currentDistance > newDistance)
            {
                currentDistance = newDistance;
                nearestEnemy = enemy.gameObject;
            }
        }

        return nearestEnemy;
    }
}
