using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour {

    GameManager gameManager;
    EnvironmentTable environmentTable;
    [SerializeField] EnvironmentSingletonData envSingletonData;
    [SerializeField] GameObject generationArea;
    [SerializeField] bool autoLoadRandomEnvironment = true;
    public Environment currentEnvironment;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        environmentTable = gameManager.environmentTable;

        if (autoLoadRandomEnvironment)
        {
            currentEnvironment = ChooseRandomWeightedEnvironment();
        }
        else
        {
            currentEnvironment = envSingletonData.GetEnvironmentToLoad();
        }
        GenerateArenaEnvironment();
    }

    private Environment ChooseRandomWeightedEnvironment()
    {
        int index;
        List<float> envWeights = new List<float>();
        Environment chosenEnv = null;
        foreach (Environment env in environmentTable.environmentTypes)
        {
            envWeights.Add(env.weight);
        }
        // Prevents the same Environment from loading twice
        do
        {
            index = RandomFromDistribution.RandomChoiceFollowingDistribution(envWeights);
        } while (environmentTable.environmentTypes[index] == envSingletonData.GetEnvironmentToLoad());
        chosenEnv = environmentTable.environmentTypes[index];
        return chosenEnv;
    }

    private Vector3 GetRandomPositionInArea()
    {
        float xMin = -(generationArea.transform.localScale.x / 2);
        float xMax = (generationArea.transform.localScale.x / 2);
        float yMin = -(generationArea.transform.localScale.y / 2);
        float yMax = (generationArea.transform.localScale.y / 2);

        float newX = UnityEngine.Random.Range(xMin, xMax);
        float newY = UnityEngine.Random.Range(yMin, yMax);

        Vector3 position = new Vector3(newX + generationArea.transform.position.x, newY + generationArea.transform.position.y, 0);
        return position;
    }

    // // // // // // //
    #region GENERATION
    private void GenerateArenaEnvironment()
    {
        Debug.Log("GENERATING ARENA~");
        int envObjectDensity = currentEnvironment.envObjectDensity;
        int envDetailDensity = currentEnvironment.envDetailDensity;
        int envHazardDensity = currentEnvironment.envHazardDensity;
        int currentObjects = 0, currentDetails = 0, currentHazards = 0;

        // Spawn Background and Lighting Manager
        Instantiate(currentEnvironment.backgroundPrefab);
        FindObjectOfType<LightingManager2D>().darknessColor = currentEnvironment.darknessColor;

        // Spawn Env Objects until we reach density
        for(int i = 0; i < envObjectDensity; i++)
        {
            GameObject newObject = Instantiate(currentEnvironment.GetRandomEnvObject(), GetRandomPositionInArea(), Quaternion.identity) as GameObject;
            newObject.transform.SetParent(transform);
        }
        for (int i = 0; i < envDetailDensity; i++)
        {
            GameObject newObject = Instantiate(currentEnvironment.GetRandomEnvDetail(), GetRandomPositionInArea(), Quaternion.identity) as GameObject;
            newObject.transform.SetParent(transform);
        }
        for (int i = 0; i < envHazardDensity; i++)
        {
            GameObject newObject = Instantiate(currentEnvironment.GetRandomEnvHazard(), GetRandomPositionInArea(), Quaternion.identity) as GameObject;
            newObject.transform.SetParent(transform);
        }
    }
    #endregion
}
