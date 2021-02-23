using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentalObjectType {

    public GameObject envObject;
    [Range(0,100)]public float weight = 50f;
	
}
