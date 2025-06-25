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


public class SpawnFireOnCollision : MonoBehaviour
{
    public GameObject fireProjectile;

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contactPoint = collision.GetContact(0);

        Instantiate(fireProjectile, contactPoint.point, Quaternion.FromToRotation(Vector3.up, contactPoint.normal));
        // contactPoint.normal

        Destroy(gameObject);
    }

}
