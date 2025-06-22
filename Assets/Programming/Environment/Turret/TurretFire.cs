using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|



public class TurretFire : MonoBehaviour
{
    [Header("Turret firing settings")]
    public GameObject projectileObject;

    public float projectileDamage = 10f;

    public float windUpTime = 0.3f;

    public float fireEvery = 0.5f;

    public float projectileSpeed = 5f;

    public Transform projectileSpawnPoint;

    protected float windUpTimer = 0f;

    protected float fireTimer = 0;

    protected bool isFiring = false;



    protected virtual void Update()
    {
        DealWithTimers();

        TryAndFire();
    }

    protected virtual void DealWithTimers()
    {
        if (fireTimer > 0 && !isFiring)
        {
            fireTimer -= Time.deltaTime;
        }

        if (isFiring && windUpTimer > 0)
        {
            windUpTimer -= Time.deltaTime;
        }
    }

    protected virtual void TryAndFire()
    {
        if (fireTimer <= 0 && !isFiring)
        {
            windUpTimer = windUpTime;
            isFiring = true;
        }

        if (isFiring && windUpTimer <= 0)
        {
            isFiring = false;
            fireTimer = fireEvery;

            SpawnProjectile();
        }
    }

    protected virtual void SpawnProjectile()
    {
        Vector3 targetVel = projectileSpawnPoint.forward * projectileSpeed;

        GameObject projectile = Instantiate(projectileObject, projectileSpawnPoint.position, Quaternion.identity);

        projectile.GetComponent<RangedProjectilePhysicsBased>().SetUpProjectile(targetVel, projectileDamage);
    }
}
