using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

    // Variables
    float maxHeightToRaise = 1.5f;
    float secondsToWait = 1.5f;
    float initialYPos;
    enum GateState {raise , lower};
    GateState gateState;
    bool isEnemyInRange;
    // References
    [SerializeField] GameObject gateBody;
    GateEnemyDetector gateEnemyDetector;
    new Collider2D collider;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        gateEnemyDetector = GetComponentInChildren<GateEnemyDetector>();

        initialYPos = gateBody.transform.localPosition.y;
        //maxHeightToRaise = collider.bounds.size.y * 0.75f; // Raise 1.5x height of collider
        StartCoroutine(CheckGateStatus());
    }

    private void Update()
    {
        switch (gateState)
        {
            case GateState.raise:
                RaiseGate();
                break;
            case GateState.lower:
                LowerGate();
                break;
        }
    }

    // Every few seconds, check to see if enemy inside collider
    private IEnumerator CheckGateStatus()
    {
        while (true)
        {
            if (gateEnemyDetector.GetIsEnemyInCollider())
            {
                gateState = GateState.raise;
            }
            else
            {
                gateState = GateState.lower;
            }
            yield return new WaitForSeconds(secondsToWait);
        }
    }

    private void RaiseGate()
    {
        if((gateBody.transform.localPosition.y - initialYPos) <= maxHeightToRaise)
        {
            gateBody.transform.localPosition += Vector3.up * Time.deltaTime;
        }
        else
        {
            collider.enabled = false;
        }
    }

    private void LowerGate()
    {
        collider.enabled = true;
        if ((maxHeightToRaise - gateBody.transform.localPosition.y) <= maxHeightToRaise)
        {
            gateBody.transform.localPosition += Vector3.down * Time.deltaTime;
        } 
    }

    public IEnumerator ForceOpenGate(float seconds)
    {
        // Open
        while ((gateBody.transform.localPosition.y - initialYPos) <= maxHeightToRaise)
        {
            gateBody.transform.localPosition += Vector3.up * Time.deltaTime;
        }
        collider.enabled = false;

        yield return new WaitForSeconds(seconds);

        // Close
        collider.enabled = true;
        if ((maxHeightToRaise - gateBody.transform.localPosition.y) <= maxHeightToRaise)
        {
            gateBody.transform.localPosition += Vector3.down * Time.deltaTime;
        }
    }
}
