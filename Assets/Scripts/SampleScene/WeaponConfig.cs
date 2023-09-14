using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Unnamed Weapon" ,menuName="Weponconfig",order =0)]
public class WeaponConfig:ScriptableObject
{
    public float mxAmmo;
    public float damage;
    public bool areaDamage;
}
