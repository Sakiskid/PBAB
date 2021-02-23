using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponProperties : ScriptableObject {

    [Header("Audio")]
    [FMODUnity.EventRef] [SerializeField] public string whooshSound;

    [Header("Damage")]
    [SerializeField] public float damageMultiplier = 1f;
    [SerializeField] public float minVeloForDaze = 5;
    [SerializeField] public float minVeloForDamage = 1.5f;

    [Header("Tweaks")]
    [SerializeField] public int snapSpeedTweak = 1;
    [SerializeField] public int rotationSpeedTweak = 1;
    [SerializeField] public float minVeloForWhooshAudio = 8f;
}
