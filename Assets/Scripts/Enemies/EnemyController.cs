using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    
    // CONFIGURATION VARS
    [Header("ENEMY CONTROLLER PROPERTIES")]
    [Space]
    [SerializeField] GameObject damageTextPrefab;
    [SerializeField] GameObject dazeParticlesPrefab;
    [SerializeField] GameObject goreSystem;
    [SerializeField] GameObject deformationMask;
    [SerializeField] protected Animator animator;
    [Space]
    [Header("Audio")]
    [FMODUnity.EventRef] [SerializeField] string hurtImpactSound;
    [Space]
    [Header("Config")]
    [SerializeField] float rotateSpeed = 1f;
    [SerializeField] int health = 100;
    [Space]
    [Header("Tweaks")]
    [SerializeField] float dazeResistanceMultiplier = 1;
    [Space]

    protected GameObject player;
    protected Vector2 targetMovePos;
    protected Rigidbody2D rb2D;
    Transform rotationObject;

    public bool isFleeing = false;
    public bool isDazed = false;
    protected bool isDying = false;

    float deathFadeTime = 1f;
    int dazeTimeReducer = 4;
    protected float distanceToPlayer;

    protected const string PLAYER = "Player";
    protected const string MOVETYPE_STRAFE = "Strafe";
    protected const string MOVETYPE_DIRECTION = "Direction";
    protected const string ROTATETYPE_FLEE = "Flee";
    protected const string ROTATETYPE_MOVEMENT = "Movement";



    // Use this for initialization
    protected void Init () {
        player = GameObject.Find(PLAYER);
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rotationObject = transform.Find("Rotation Object");
	}

    #region Public Methods

    public IEnumerator BecomeDazed(float weaponVelocity, float weaponMass)
    {
        if (!isDazed)
        {
            float dazedTime;
            dazedTime = (weaponVelocity * weaponMass) / dazeTimeReducer + dazeResistanceMultiplier;

            //print(gameObject.name + " is dazed for " + dazedTime);
            //print("STUN BREAKDOWN: dazetimeMultiplier:" + dazeResistanceMultiplier + " weaponVelocity:" + weaponVelocity + " weaponMass:" + weaponMass);
            isDazed = true;
            GameObject dazedParticles = Instantiate(dazeParticlesPrefab, transform) as GameObject;

            yield return new WaitForSeconds(dazedTime);
            // If statement to prevent accessing destroyed system bug
            if (health > 0 && dazeParticlesPrefab) { Destroy(dazedParticles); }
            isDazed = false;
        }
    }

    public void TakeDamage(float damage)
    {
        HurtAudio();
        int incomingDamage;
        incomingDamage = Mathf.RoundToInt(damage);
        health -= incomingDamage;

        // DAMAGE TEXT. Update damage in textbox
        if (damageTextPrefab)
        {
            GameObject damageTextBox = Instantiate(damageTextPrefab, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
            damageTextBox.GetComponentInChildren<DamageText>().DisplayDamage(incomingDamage);
        }

        if(health <= 0)
        {
            Die();
        }
    }

    #endregion

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        // DEFORM AND GORE
        if (collision.gameObject.CompareTag("Weapon")){
            Deform(collision);
            GoreVFX(collision);
        }
    }

    protected void GoreVFX(Collision2D collision)
    {
        if (goreSystem)
        {
            ContactPoint2D contact = collision.contacts[0];
            GameObject newGore = Instantiate(goreSystem, contact.point, Quaternion.identity, transform) as GameObject;
            Vector2 clampedPos = new Vector2(Mathf.Clamp(newGore.transform.localPosition.x, -0.65f, 0.65f), Mathf.Clamp(newGore.transform.localPosition.y, -0.55f, 0.55f));
            newGore.transform.localPosition = clampedPos;
        }
    }

    protected void Deform(Collision2D collision)
    {
        if (deformationMask)
        {
            // if greater than .65 x or .55 y then shrink it down
            ContactPoint2D contact = collision.contacts[0];
            GameObject newMask = Instantiate(deformationMask, contact.point, Quaternion.identity, transform) as GameObject;
            Vector2 clampedPos = new Vector2(Mathf.Clamp(newMask.transform.localPosition.x, -0.65f, 0.65f), Mathf.Clamp(newMask.transform.localPosition.y, -0.55f, 0.55f));
            newMask.transform.localPosition = clampedPos;
        }
    }
    ///
    protected void Die()
    {
        if (!isDying)
        {
            StopCoroutine("BecomeDazed");
            if (FindObjectOfType<ScoreManager>()) { FindObjectOfType<ScoreManager>().AddOneKill(); }

            // Fade and die
            isDying = true;
            StartCoroutine(FadeDeath());
        }
    }

    private IEnumerator FadeDeath()
    {
        float currentFadeOutTime = deathFadeTime;
        SpriteRenderer[] spriteRenderersInChildren = transform.parent.GetComponentsInChildren<SpriteRenderer>();

        while (isDying && currentFadeOutTime >= 0) // While dying, change fading. Then if done fading, destroy object
        {
            currentFadeOutTime -= Time.deltaTime;
            float fadePercentage = Mathf.InverseLerp(0, deathFadeTime, currentFadeOutTime);
            float newAlpha = Mathf.Lerp(0, 255, fadePercentage);

            foreach (SpriteRenderer spriteRenderer in spriteRenderersInChildren)
            {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b , fadePercentage);
            }
            yield return null;
        }
        if (isDying && currentFadeOutTime <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }


    /// <summary>
    /// Acceptable Movement Types: MOVETYPE_STRAFE , MOVETYPE_DIRECTION
    /// </summary>
    /// <param name="moveType"></param>
    protected void Move(Vector2 targetPos, string moveType, float moveSpeed)
    {
        float step = moveSpeed * Time.deltaTime;

        if (moveType == MOVETYPE_STRAFE)
        {
            // MOVE TOWARDS REGARDLESS OF ROTATION
            transform.position = Vector2.MoveTowards(transform.position, targetPos, step);
        }
        if (moveType == MOVETYPE_DIRECTION)
        {
            // MOVE IN DIRECTION OF ROTATION
            transform.Translate(Vector3.up * step, Space.Self);
        }
    }

    protected void Rotate(string direction)
    {
        if (direction == PLAYER) {
            rotationObject.transform.up = -(transform.position - player.transform.position);
            rb2D.MoveRotation(Quaternion.Slerp(transform.rotation, rotationObject.rotation, rotateSpeed * Time.deltaTime).eulerAngles.z);
        }
        else if(direction == ROTATETYPE_FLEE) {
            rotationObject.transform.up = (transform.position - player.transform.position);
            rb2D.MoveRotation(Quaternion.Slerp(transform.rotation, rotationObject.rotation, rotateSpeed * Time.deltaTime).eulerAngles.z);
        }
        else if(direction == ROTATETYPE_MOVEMENT) {

        }
    }

    protected void CalculatePlayerDistance()
    {
        distanceToPlayer = (player.transform.position - transform.position).magnitude;
    }

    void HurtAudio()
    {
        FMODUnity.RuntimeManager.PlayOneShot(hurtImpactSound, transform.position);
    }
}
