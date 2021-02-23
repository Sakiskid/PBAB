using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnvironmentGenerator/EnvTable")]
public class EnvironmentTable : ScriptableObject {

    public Environment[] environmentTypes = new Environment[1];
}
