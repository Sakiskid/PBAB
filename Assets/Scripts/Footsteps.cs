using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour {

    [FMODUnity.EventRef] [SerializeField] string stepSound;
    [SerializeField] float stepDistance = 1f;
    [SerializeField] float stepWidth = 0.5f;
    [SerializeField] float timeBetweenSteps = 0.2f;
    [SerializeField] GameObject footPrintPrefab;

    Vector3 lastStepPos;
    bool stepLeft;

    private void Start()
    {
        lastStepPos = transform.position;
        StartCoroutine(CalculateSteps());
    }


    private IEnumerator CalculateSteps()
    {
        while (true)
        {
            float distanceSinceLastStep;
            distanceSinceLastStep = (transform.position - lastStepPos).magnitude;

            if (distanceSinceLastStep >= stepDistance)
            {
                Step();
                lastStepPos = transform.position;
                yield return new WaitForSeconds(timeBetweenSteps + UnityEngine.Random.Range(0f ,0.3f)); // Simulates running at high speed, with long strides
            }
            yield return null;
        }
    }

    private void Step()
    {
        FMODUnity.RuntimeManager.PlayOneShot(stepSound);
        float offset;

        if (stepLeft)
        {
            offset = -stepWidth;
            stepLeft = !stepLeft;
        }
        else
        {
            offset = stepWidth;
            stepLeft = !stepLeft;
        }

        Vector3 newPos = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);
        GameObject footprint = Instantiate(footPrintPrefab, newPos, transform.rotation) as GameObject;
    }
}
