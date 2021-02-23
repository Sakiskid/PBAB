using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnvironmentGenerator/EnvType")][System.Serializable]
public class Environment : ScriptableObject {

    [Range(0, 100)] public float weight = 50;

    [SerializeField] [Range(0, 50)] public int envObjectDensity = 6;
    [SerializeField] [Range(0, 50)] public int envDetailDensity = 6;
    [SerializeField] [Range(0, 50)] public int envHazardDensity = 6;

    [SerializeField] public GameObject backgroundPrefab;
    [SerializeField] public Color darknessColor;
    [SerializeField] EnvironmentObjectTable envObjectTable;

    public GameObject GetRandomEnvObject()
    {
        List<float> objWeights = new List<float>();
        GameObject chosenObject = null;
        foreach(EnvironmentalObjectType obj in envObjectTable.envObjects)
        {
            objWeights.Add(obj.weight);
        }

       // Debug.Log("random selection from objWeights " + RandomFromDistribution.RandomChoiceFollowingDistribution(objWeights));
       // Debug.Log("table: " + envObjectTable + " objectType selected: " + envObjectTable.envObjects[0] + " and finally, gameObject selected: " + envObjectTable.envObjects[0].envObject);

        int index = RandomFromDistribution.RandomChoiceFollowingDistribution(objWeights);
        chosenObject = envObjectTable.envObjects[index].envObject;
        return chosenObject;
    }

    public GameObject GetRandomEnvDetail()
    {
        List<float> objWeights = new List<float>();
        GameObject chosenObject = null;
        foreach (EnvironmentalObjectType obj in envObjectTable.envDetails)
        {
            objWeights.Add(obj.weight);
        }
        int index = RandomFromDistribution.RandomChoiceFollowingDistribution(objWeights);
        chosenObject = envObjectTable.envDetails[index].envObject;
        return chosenObject;
    }

    public GameObject GetRandomEnvHazard()
    {
        List<float> objWeights = new List<float>();
        GameObject chosenObject = null;
        foreach (EnvironmentalObjectType obj in envObjectTable.envHazards)
        {
            objWeights.Add(obj.weight);
        }
        int index = RandomFromDistribution.RandomChoiceFollowingDistribution(objWeights);
        chosenObject = envObjectTable.envHazards[index].envObject;
        return chosenObject;
    }
}
