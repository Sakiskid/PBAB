﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaFighterHand : MonoBehaviour {

    [Header("Serializations")]
    [SerializeField] AlphaFighter handOwner;
    [SerializeField] GameObject handDesignator;
    [Header("Configuration")]
    [SerializeField] float handSpeed = 10f;
    [SerializeField] int maxHealth = 25;
    [SerializeField, Range(0f,100f)] int breakPercentage;
    [Header("Blood Squirt Config")]
    [SerializeField] GameObject handGorePrefab;
    [Header("Audio")]
    [SerializeField] AudioClip breakSound;

    AudioSource audioSource;
    Rigidbody2D handRB2D;

    public bool isPunching;
    public bool isBroken = false;
    [SerializeField] int currentHealth;
    int breakThreshold;

    public void Update()
    {
        if (!isBroken) // If not broken, then update position
        {
            //handRB2D.MovePosition(Vector2.MoveTowards(transform.position, handDesignator.transform.position, handSpeed * Time.fixedDeltaTime));
            handRB2D.AddForce((handDesignator.transform.position - transform.position) * handSpeed * Time.fixedDeltaTime);
        }
    }

	void Start () {
        handRB2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        // calculate threshold percentages
        breakThreshold = breakPercentage * maxHealth / 100;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // PLAYER COLLISION - damage on punch
        if(collision.gameObject.tag == "Player")
        {
            if (!isBroken && isPunching)
            {
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                // Calculate Punch Damage
                if (playerController)
                {
                    playerController.TakeDamage(
                        Mathf.Clamp(handOwner.punchDamage *
                        Mathf.RoundToInt(handRB2D.velocity.magnitude), 0, handOwner.maxPunchDamage));
                }
            }
        }

        // WEAPON COLLISION - chop off those hands!!
        if(collision.gameObject.tag == "Weapon")
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude >= 1)
            {
                int incomingDamage = Mathf.RoundToInt(collision.rigidbody.velocity.magnitude * collision.rigidbody.mass);
                currentHealth -= incomingDamage;

                if (currentHealth <= breakThreshold)
                {
                    Break();
                }
            }
        }
    }

    private void Break()
    {
        if (isBroken == false)
        {
            isBroken = true;
            GetComponent<DistanceJoint2D>().enabled = false;
            GetComponent<Rigidbody2D>().drag = 1f;

            audioSource.PlayOneShot(breakSound);

            // GORE
            Instantiate(handGorePrefab, transform.position, Quaternion.identity, transform);
        }
    }
}
