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
public class BarrageAttack
{
    public Transform spawnPoint;

    public float forceForProjectile = 15f;

    public GameObject projectilePrefab;

    public float attackCoolDown = 5f;

    public int projectileCount = 6;

    public float verticalOffset = 10f;

    public float horizontalDisplacement = 10f;

    public float verticalDisplacement = 10f;

    public float delayPerShot = 1f;

    public float windUpTime = 2f;


    public Quaternion GenerateProjectileAngle()
    {
        return Quaternion.AngleAxis(verticalOffset, spawnPoint.right)
                * Quaternion.AngleAxis(UnityEngine.Random.Range(-horizontalDisplacement, horizontalDisplacement), spawnPoint.up)
                * Quaternion.AngleAxis(UnityEngine.Random.Range(-verticalDisplacement, verticalDisplacement), spawnPoint.right);
    }

    public float GetAngleForFireProjectile(Vector3 startPos, Vector3 targetPos, float force)
    {
        float gravity = Mathf.Abs(Physics.gravity.y);

        float distance = Vector2.Distance(new Vector2(startPos.x, startPos.z), new Vector2(targetPos.x, targetPos.z));
        float heightOffset = startPos.y - targetPos.y;
        //subtract to get direct curve and adding is high curve.
        //A = arctan((v^2 ± SQRT(v^4 - g(gx^2 + 2yv^2)))/gx)

        //float angle01 = Mathf.Atan((Mathf.Pow(force, 2) + Mathf.Sqrt(Mathf.Pow(force, 4) - (Physics.gravity.y * (Physics.gravity.y * Mathf.Pow(distance, 2) + (2 * heightOffset) * Mathf.Pow(force, 2))))) / (Physics.gravity.y * distance));

        //print("Angle 01 : " + angle01);

        float angle02 = Mathf.Atan((Mathf.Pow(force, 2) - Mathf.Sqrt(Mathf.Pow(force, 4) - (Physics.gravity.y * (Physics.gravity.y * Mathf.Pow(distance, 2) + (2 * heightOffset) * Mathf.Pow(force, 2))))) / (Physics.gravity.y * distance));

        //print("Angle 02 : " + angle02);

        //float angleResult = Mathf.Min(angle01, angle02);
        float angleResult = angle02;

        //print("Angle : " + angleResult * Mathf.Rad2Deg);
        return angleResult * Mathf.Rad2Deg;
    }
}
