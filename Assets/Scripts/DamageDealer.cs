using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour {

    [SerializeField] WeaponProperties weaponProperties;
    Rigidbody2D rb2D;

    int maxDamageToPlayer = 15;

    [SerializeField] bool canHurtEnemy;
    [SerializeField] bool canHurtPlayer;
    [SerializeField] bool canHurtOther;
    bool hasWhooshed;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleAudio();
    }

    public void SetWhoCanBeDamaged(bool newCanHurtEnemy, bool newCanHurtPlayer, bool newCanHurtOther)
    {
        canHurtEnemy = newCanHurtEnemy;
        canHurtPlayer = newCanHurtPlayer;
        canHurtOther = newCanHurtOther;
    }

    void OnCollisionEnter2D(Collision2D otherCollider)
    {
        //  ENEMY COLLISION
        if (canHurtEnemy && otherCollider.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemyController = otherCollider.gameObject.GetComponent<EnemyController>();

            // DAZE ENEMY
            if (rb2D.velocity.magnitude > weaponProperties.minVeloForDaze)
            {
                StartCoroutine(enemyController.BecomeDazed(rb2D.velocity.magnitude, rb2D.mass));
            }

            // DEAL DAMAGE
            if (rb2D.velocity.magnitude > weaponProperties.minVeloForDamage)
            {
                enemyController.TakeDamage(rb2D.velocity.magnitude * rb2D.mass * weaponProperties.damageMultiplier);
            }
        }


        // PLAYER COLLISION
        if (canHurtPlayer && otherCollider.gameObject.CompareTag("Player"))
        {
            int damage = Mathf.RoundToInt(Mathf.Clamp(rb2D.velocity.magnitude * weaponProperties.damageMultiplier, 0, maxDamageToPlayer));
            otherCollider.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }

        // OTHER COLLISION
    }

    private void HandleAudio()
    {
        if (weaponProperties.whooshSound != null)
        {
            if (rb2D.velocity.magnitude > weaponProperties.minVeloForWhooshAudio && hasWhooshed == false)
            {
                hasWhooshed = true;
                FMODUnity.RuntimeManager.PlayOneShot(weaponProperties.whooshSound, transform.position);
            }
            else if (rb2D.velocity.magnitude < weaponProperties.minVeloForWhooshAudio) { hasWhooshed = false; }
        }
    }
}
