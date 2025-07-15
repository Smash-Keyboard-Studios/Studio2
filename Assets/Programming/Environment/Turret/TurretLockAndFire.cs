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

public class TurretLockAndFire : TurretFire
{
    [Header("Turret detecting settings")]
    public float maxDetectionRange = 10f;

    public float maxRotationFromCentre = 90f;

    public float lookAtRotationSpeed = 5f;

    private Transform player; // remove
    private CharacterController playerCC; // remove

    public Transform SensorPoint;


    private Vector3 forwardVector; // because using a child instead is too hard :3.

    void Start()
    {
        player = PlayerReferenceFetcher.instance.GetPlayerReference().transform;
        playerCC = player.GetComponent<CharacterController>();

        forwardVector = transform.forward;
    }

    protected override void Update()
    {
        DealWithTimers();

        // Turret detection to player.
        Vector3 directionXZ = new Vector3(player.position.x, 0, player.position.z) - new Vector3(SensorPoint.position.x, 0, SensorPoint.position.z);
        float angle = Quaternion.Angle(Quaternion.LookRotation(forwardVector), Quaternion.LookRotation(directionXZ.normalized));

        Quaternion targetRotation = new Quaternion();

        if (angle < maxRotationFromCentre && Vector3.Distance(player.position, SensorPoint.position) < maxDetectionRange && !Physics.Linecast(SensorPoint.position, player.position, LayerMask.GetMask("Default")))
        {
            float projectileTravelTime = Vector3.Distance(player.position, projectileSpawnPoint.position) / projectileSpeed;

            Vector3 playerSameY = player.position;
            playerSameY.y = transform.position.y;


            targetRotation = Quaternion.LookRotation(((playerSameY + (playerCC.velocity.normalized * (projectileTravelTime + (playerCC.velocity.magnitude * Time.deltaTime)))) - transform.position).normalized);
            TryAndFire();
        }
        else
        {
            targetRotation = Quaternion.LookRotation(Quaternion.AngleAxis(maxRotationFromCentre * Mathf.Cos(Time.time), transform.up) * forwardVector);
        }


        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, lookAtRotationSpeed * Time.deltaTime);
        // detect player, look at player. maybe lead shots too.


        // otherwise we will just look left and right? but this isn't a turret, its a portal.
    }

}
