using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] WeaponConfig config;
    // Start is called before the first frame update
    void Start()
    {
        Shoot();
    }

    public void Shoot()
    {
        Debug.Log(config.damage + "‚ð" + config.name + "‚Í‚¨‚Á‚½");
    }
}
