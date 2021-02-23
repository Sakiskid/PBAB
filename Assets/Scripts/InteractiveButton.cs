using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveButton : MonoBehaviour {

    [Header("Who can trigger this?")]
    [SerializeField] bool canPlayerTrigger;
    [SerializeField] bool canEnemyTrigger;
    [SerializeField] bool canWeaponTrigger;
    [SerializeField] UnityEvent OnButtonPress;
    [Space]

    [Header("Audio")]
    [FMODUnity.EventRef] [SerializeField] string buttonOnSound;
    [FMODUnity.EventRef] [SerializeField] string buttonOffSound;


    bool isPressed;
    int currentNumberOfThingsPressingButton = 0;

    private void CheckNumberOfButtonPressers()
    {
        if(currentNumberOfThingsPressingButton > 0) { isPressed = true; }
        else if(currentNumberOfThingsPressingButton == 0) { isPressed = false; }
    }

    private void ActivateButton()
    {
        FMODUnity.RuntimeManager.PlayOneShot(buttonOnSound);
        OnButtonPress.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        // Check if Player can press
        if (otherCollider.GetComponent<PlayerController>() && canPlayerTrigger)
        {
            if (!isPressed)
            {
                ActivateButton();
            }
            currentNumberOfThingsPressingButton++;
            CheckNumberOfButtonPressers();
        }
        // Check if Enemy can press
        if (otherCollider.GetComponent<EnemyController>() && canEnemyTrigger)
        {
            if (!isPressed)
            {
                ActivateButton();
            }
            currentNumberOfThingsPressingButton++;
            CheckNumberOfButtonPressers();
        }
        // Check if Weapon can press
        if (otherCollider.GetComponent<WeaponController>() && canWeaponTrigger && !isPressed)
        {
            if (!isPressed)
            {
                ActivateButton();
            }
            currentNumberOfThingsPressingButton++;
            CheckNumberOfButtonPressers();
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.GetComponent<PlayerController>() && canPlayerTrigger)
        {
            currentNumberOfThingsPressingButton--;
            CheckNumberOfButtonPressers();
        }
        if (otherCollider.GetComponent<EnemyController>() && canEnemyTrigger)
        {
            currentNumberOfThingsPressingButton--;
            CheckNumberOfButtonPressers();
        }
        if (otherCollider.GetComponent<WeaponController>() && canWeaponTrigger)
        {
            currentNumberOfThingsPressingButton--;
            CheckNumberOfButtonPressers();
        }

        if(currentNumberOfThingsPressingButton == 0) { FMODUnity.RuntimeManager.PlayOneShot(buttonOffSound); }
    }
}
