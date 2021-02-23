using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodControl : MonoBehaviour {

    [SerializeField] bool destroyWhenParticleSystemIsDone;
    float timeBetweenDestroyChecks = 3f;
    ParticleSystem parentParticleSystem;

    private void Start()
    {
        parentParticleSystem = GetComponent<ParticleSystem>();
        StartCoroutine(CheckIfParticleSystemsAlive());
    }

    public void startFollow(Transform transformToFollow)
    {
        StartCoroutine(Follow(transformToFollow));
    }

	IEnumerator Follow(Transform transformToFollow)
    {
        while (true)
        {
            if (transformToFollow)
            {
                transform.position = transformToFollow.position;
                transform.rotation = transformToFollow.rotation;
            }
            else if (!transformToFollow)
            {
                parentParticleSystem.Stop(true);
            }
            yield return null;
        }
    }

    IEnumerator CheckIfParticleSystemsAlive()
    {
        if (parentParticleSystem.IsAlive(true))
        {
            yield return new WaitForSeconds(timeBetweenDestroyChecks);
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
