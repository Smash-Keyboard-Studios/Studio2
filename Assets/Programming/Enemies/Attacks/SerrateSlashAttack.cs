using System;
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

[Serializable]
public class SerrateSlashAttack
{
    [Header("Serrated Slash Settings")]

    // cool down
    public float serratedSlashCoolDown = 10f;

    // activation requirements
    public float minimumDistanceForSerratedSlash = 4f;

    // wind up
    public float serratedSlashWindUpTime = 0.5f;

    // damage and radius
    public float serratedSlashRadius = 5f;

    public float serratedSlashDamage = 2f;

    public float serratedSlashTickLength = 0.25f;

    public float serratedSlashAttackDuration = 1f;



    public void DamageInRadius(Transform entityTransform, LayerMask layersToCheck)
    {
        Collider[] HitObjects = Physics.OverlapSphere(entityTransform.position, serratedSlashRadius,
                    layersToCheck, QueryTriggerInteraction.Ignore);

        if (HitObjects.Length > 0)
        {
            foreach (var hitObject in HitObjects)
            {
                if (hitObject.gameObject.CompareTag(Constants.PlayerTag))
                {
                    float distanceFromPlayer = Vector3.Distance(hitObject.transform.position, entityTransform.position);

                    hitObject.GetComponent<IDamageable>()?.TakeDamage(serratedSlashDamage);
                }
            }
        }
    }
}
