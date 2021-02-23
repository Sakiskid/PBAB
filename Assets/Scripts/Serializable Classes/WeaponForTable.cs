using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponForTable {

    public GameObject weapon;
    [SerializeField] [Range(0,500)]public int randomWeight;

}
