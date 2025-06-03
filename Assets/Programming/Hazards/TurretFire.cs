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
    public GameObject projectileObject;

    public float projectileDamage = 10f;

    public float windUpTime = 0.3f;

    public float fireEvery = 0.5f;

    public float projectileSpeed = 5f;

    protected float windUpTimer = 0f;

    protected float fireTimer = 0;

    protected bool isFiring = false;



    protected virtual void Update()
    {
        if (fireTimer > 0 && !isFiring)
        {
            fireTimer -= Time.deltaTime;
        }

        if (fireTimer <= 0 && !isFiring)
        {
            windUpTimer = windUpTime;
            isFiring = true;
        }

        if (isFiring && windUpTimer > 0)
        {
            windUpTimer -= Time.deltaTime;
        }

        if (isFiring && windUpTimer <= 0)
        {
            isFiring = false;
            fireTimer = fireEvery;

            Vector3 targetVel = transform.forward * projectileSpeed;

            GameObject projectile = Instantiate(projectileObject, transform.position + transform.forward, Quaternion.identity);

            projectile.GetComponent<RangedProjectilePhysicsBased>().SetUpProjectile(targetVel, projectileDamage);
        }
    }
}
