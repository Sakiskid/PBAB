using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : MonoBehaviour {

    Grabbable grabbable;
    [SerializeField] WeaponController weaponController;
    [SerializeField] GameObject boltPrefab;
    [SerializeField] AudioClip firingSound;

    [SerializeField] float knockback;
    [SerializeField] float horizontalKnockback;
    [SerializeField] float power;

	// Use this for initialization
	void Start () {
        grabbable = GetComponent<Grabbable>();
	}
	
	// Update is called once per frame
	void Update () {
        if (grabbable.GetIsGrabbed())
        {
            if (Input.GetMouseButtonDown(0)) { Fire(); }
            if (Input.GetKey(KeyCode.H)) { Fire(); }
        }
	}

    private void Fire()
    {
        Rigidbody2D rb2D;
        rb2D = GetComponent<Rigidbody2D>();

        // Knockback and recoil
        rb2D.AddRelativeForce(new Vector2(0, -knockback));
        rb2D.AddTorque(Random.Range(-horizontalKnockback, horizontalKnockback));

        // Shoot Projectile
        GameObject bolt = Instantiate(boltPrefab, transform.position, transform.rotation);
        bolt.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0, power));

        // Audio
        GetComponent<AudioSource>().PlayOneShot(firingSound);
    }
}
