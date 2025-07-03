using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|
// © 2025 Dominic McNeill dommcneill@outlook.com

[RequireComponent(typeof(Rigidbody))]
public class RangedProjectilePhysicsBased : MonoBehaviour
{
    private Rigidbody rb;

    private float projectileDamage;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnTriggerEnter(Collider collider)
    {
        // ignore all triggers, we are looking for colliders.
        if (collider.isTrigger) return;

        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<IDamageable>().TakeDamage(projectileDamage);
            Destroy(gameObject);
        }

        Destroy(gameObject);
    }

    public void SetUpProjectile(Vector3 force, float damage)
    {
        rb.AddForce(force, ForceMode.VelocityChange);
        projectileDamage = damage;
    }
}
