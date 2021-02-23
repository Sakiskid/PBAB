using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Player : ScriptableObject,
    ISerializationCallbackReceiver {

    [Header("AUDIO")]
    [FMODUnity.EventRef] public string hurtSound;
    [FMODUnity.EventRef] public string deathSound;

    [Header("References")]
    public GameObject deathParticles;
    public GameObject rotationObject;

    [Header("Player Config")]
    public float initialHealth;
    public float health;

    [Header("Movement")]
    public float rotationSpeed = 1f;
    public float speedMultiplier = 1f;
    public float sprintMultiplier = 2f;
    public float maxStaminaInSeconds = 3f;
    public float staminaRegenPerSecond = 0.1f;
    public float staminaRegenDelay = 1f;
    public float minRunSpeed;
    public float maxRunSpeed;
    public float minSprintSpeed;
    public float maxSprintSpeed;

    [Header("Weapons and Grabbing")]
    public Vector2 weaponOffset;
    public float maxGrabRange = 5f;


    public void OnAfterDeserialize()
    {
        health = initialHealth;
    }

    public void OnBeforeSerialize() { }
}
