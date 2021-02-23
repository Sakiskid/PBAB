using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponTable : ScriptableObject {

    public WeaponForTable[] weapons = new WeaponForTable[1];

    public GameObject GetRandomItem()
    {
        List<float> weaponWeights = new List<float>();
        GameObject chosenWeapon = null;
        foreach(WeaponForTable weapon in weapons)
        {
            weaponWeights.Add(weapon.randomWeight);
        }
        int index = RandomFromDistribution.RandomChoiceFollowingDistribution(weaponWeights);
        chosenWeapon = weapons[index].weapon;
        return chosenWeapon;
    }
}
