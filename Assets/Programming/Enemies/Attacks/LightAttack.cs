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
public class LightAttack
{
    #region Attacking Variables
    /* Attacking */

    /// <summary>
    /// The damage the AI will inflict onto the player.
    /// </summary>
    [Header("Attacking")]
    public float lightAttackDamage = 20f;



    /// <summary>
    /// How long before next attack.
    /// </summary>
    [Tooltip("How long before next attack.")]
    public float lightAttackRate = 1f;



    /// <summary>
    /// The minimum distance possible between the AI and player before the AI will Attack.
    /// </summary>
    public float minDistanceForAttack = 2f;



    #endregion


    #region Box Check Variables
    /* Box check to detect and damage player */

    /// <summary>
    /// AI forward (local Z), how far this will stretch.
    /// </summary>
    [Header("Box check for light attack")]
    public float boxCastDepth = 2f;

    /// <summary>
    /// AI side (local X), how wide this will be.
    /// </summary>
    public float boxCastLength = 3;

    /// <summary>
    /// AI up (local Y), how tall this check box is.
    /// </summary>
    public float boxCastHeight = 1;

    /// <summary>
    /// The offset from the AI position the box check will be.
    /// </summary>
    public float boxCastForwardOffset = 1;



    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entityTransform"></param>
    /// <param name="layersToCheck"></param>
    public void CheckAndDamage(Transform entityTransform, LayerMask layersToCheck)
    {
        Collider[] HitObjects = Physics.OverlapBox(entityTransform.position + (entityTransform.forward * boxCastForwardOffset), new Vector3(boxCastLength,
            boxCastHeight, boxCastDepth) / 2f,
            entityTransform.rotation, layersToCheck);


        if (HitObjects.Length > 0)
        {
            foreach (var hitObject in HitObjects)
            {
                if (hitObject.gameObject.CompareTag(Constants.PlayerTag))
                {
                    hitObject.GetComponent<IDamageable>()?.TakeDamage(lightAttackDamage);
                }
            }
        }
    }
}
