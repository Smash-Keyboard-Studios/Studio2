using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLockAndFire : TurretFire
{
    [Header("Turret detecting settings")]
    public float maxDetectionRange = 10f;

    public float maxRotationFromCentre = 90f;

    public float lookAtRotationSpeed = 5f;

    private Transform player; // remove
    private CharacterController playerCC; // remove

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
        float angle = Quaternion.Angle(Quaternion.LookRotation(forwardVector), Quaternion.LookRotation((player.position - transform.position).normalized));

        Quaternion targetRotation = new Quaternion();

        if (angle < maxRotationFromCentre && Vector3.Distance(player.position, transform.position) < maxDetectionRange)
        {
            float projectileTravelTime = Vector3.Distance(player.position, transform.position) / projectileSpeed;
            targetRotation = Quaternion.LookRotation(((player.position + (playerCC.velocity.normalized * (projectileTravelTime + (playerCC.velocity.magnitude * Time.deltaTime)))) - transform.position).normalized);
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
