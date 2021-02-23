using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour {

    [SerializeField] TextMeshProUGUI textBox;
    Animator animator;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        animator.Play("Popup");
	}
	
	// Update is called once per frame
	void Update () {
        CheckDeath();
    }

    public void DisplayDamage(int damage)
    {
        textBox.text = damage.ToString();
    }

    private void CheckDeath()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            GameObject.Destroy(gameObject);
        }
    }
}
