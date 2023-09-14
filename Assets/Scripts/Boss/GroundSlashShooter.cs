using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlashShooter : MonoBehaviour
{
    public GameObject projectile;

    public Transform firePoint;

    public float fireRate = 4;

    private Vector3 destinationl;

    private float timeToFire;

    private GroundSlash groundSlash;

    // Update is called once per frame
    void Update()
    {
        if(Time.time>=timeToFire)
        {
            timeToFire = Time.time +1/fireRate;
        }
    }

    void ShootProjecttile()
    {

    }
}
