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
public class SlamAttack
{
    #region Slam cool down settings
    [Header("Tank Slam Attack")]

    public float slamAttackRateCoolDown = 5f;

    #endregion



    #region Wind up for slam
    [Header("Slam wind up settings")]
    public float slamWindUpTime = 1f;

    #endregion



    #region Slam size and damage variables
    [Header("Slam size and damage")]

    public float slamMaxRadius = 8f;

    public float slamAttackDamageAtMaxRange = 15f;

    [Space]

    public float slamMinRadius = 5;

    public float slamAttackDamageAtMinRange = 55f;

    #endregion



    #region Slam Requirements for activating variables
    [Header("Slam Requirements for activating")]

    public float minimumDistanceForForceSlam = 5f;

    public float timeWithinRadiusBeforeSlam = 3f;
    #endregion
}
