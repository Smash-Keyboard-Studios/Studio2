using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
