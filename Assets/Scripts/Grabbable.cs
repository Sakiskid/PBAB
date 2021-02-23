using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour {

    [SerializeField] bool rotateAwayOnGrab = true;
    bool isGrabbed;
    IEnumerator rotateCoroutine;
    IEnumerator handleMovementCoroutine;

    public void Grab(Transform grabber)
    {
        isGrabbed = true;
        handleMovementCoroutine = HandleMovement(grabber);
        StartCoroutine(handleMovementCoroutine);
        if (rotateAwayOnGrab)
        {
            rotateCoroutine = RotateAwayFromGrabber(grabber);
            StartCoroutine(rotateCoroutine);
        }
    }

    public void Drop()
    {
        isGrabbed = false;
        if (rotateAwayOnGrab)
        {
            StopCoroutine(rotateCoroutine);
            StopCoroutine(handleMovementCoroutine);
        }
    }

    public bool GetIsGrabbed()
    {
        return isGrabbed;
    }

    IEnumerator RotateAwayFromGrabber(Transform grabber)
    {
        while (true)
        {
            transform.up = (transform.position - grabber.position);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator HandleMovement(Transform grabber)
    {
        PlayerController playerController = grabber.GetComponent<PlayerController>();
        GameObject playerHand = grabber.GetChild(0).gameObject;
        Rigidbody2D rb2D = GetComponent<Rigidbody2D>();

        while (true)
        {
            float distanceFromPlayerHand;
            distanceFromPlayerHand = new Vector2(playerHand.transform.position.x - transform.position.x, playerHand.transform.position.y - transform.position.y).magnitude;
            // To keep the weapon from flying around once it gets in front of the player
            rb2D.drag = Mathf.InverseLerp(playerHand.transform.localPosition.y, 0f, distanceFromPlayerHand);

            if(distanceFromPlayerHand <= 0.15f)
            {
                rb2D.velocity = rb2D.velocity / 3;
            } else
            {
                rb2D.AddForce((playerHand.transform.position - transform.position).normalized * Time.fixedDeltaTime * playerController.rotationSpeed * 100);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
