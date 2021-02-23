using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    // THIS SCRIPT IS CURRENTLY OUTDATED -
    // Moved damagedealing and grabbing to Grabbable and DamageDealer
    [Header("Weapon Properties")]
    [SerializeField] WeaponProperties weaponProperties;

    public bool isHeld;
    
    bool hasWhooshed;
    Rigidbody2D rb2D;
    GameObject playerHand;


    float initialDrag;


    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        initialDrag = rb2D.drag;
    }

    public void PickUpWeapon(Transform player, GameObject rotationObject)
    {
        
        playerHand = player.GetChild(0).gameObject;
        isHeld = true;

    }

    public void DropWeapon()
    {
        //StopCoroutine(handleMovementCoroutine);
        isHeld = false;
        rb2D.drag = initialDrag;
    }


}
