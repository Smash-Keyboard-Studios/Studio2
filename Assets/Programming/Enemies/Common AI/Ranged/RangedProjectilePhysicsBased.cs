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
// Licensed for use within the Wraiths of Retail by Smash Keyboard Studios (SKS) only.
// Redistribution or modification outside of this project is prohibited without explicit written permission.
// For full license terms, see DOMIBRON_CODE_LICENSE.md at the project root.


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

        if (collider.gameObject.CompareTag(Constants.PlayerTag))
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
