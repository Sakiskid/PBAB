using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnvironmentSingletonData : ScriptableObject {

    Environment lastEnvironment;
    Environment chosenEnvironmentToLoad;

    public void SetEnvironmentToLoad(Environment environment)
    {
        chosenEnvironmentToLoad = environment;
    }

    public Environment GetEnvironmentToLoad()
    {
        return chosenEnvironmentToLoad;
    }
}
