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



public class ShakeCameraOnHit : MonoBehaviour
{
    public float shakeDuration = 0.3f;
    public float shakeIntensity = 0.03f;

    public float increaseIntensityFromDamage = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<HealthWithBasicShield>().onTakeDamage += OnTakeDamage;
        GetComponent<HealthWithBasicShield>().onShieldHit += OnShieldHit;
    }

    private void OnShieldHit()
    {
        CameraShakeSystem.instance.StartShake(shakeDuration, shakeIntensity);
    }

    private void OnTakeDamage(float obj)
    {
        CameraShakeSystem.instance.StartShake(shakeDuration, shakeIntensity * Mathf.Sqrt(obj) * increaseIntensityFromDamage);
    }
}
