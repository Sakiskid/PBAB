using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnvironmentGenerator/EnvObjectTable")]
public class EnvironmentObjectTable : ScriptableObject {

    public EnvironmentalObjectType[] envObjects = new EnvironmentalObjectType[1];
    public EnvironmentalObjectType[] envDetails = new EnvironmentalObjectType[1];
    public EnvironmentalObjectType[] envHazards = new EnvironmentalObjectType[1];

}
