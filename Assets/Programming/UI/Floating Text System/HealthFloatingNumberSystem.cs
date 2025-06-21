using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class HealthFloatingNumberSystem : MonoBehaviour
{
    [Header("Health Text Settings")]

    public bool healthUseGradient = false;

    public TMP_ColorGradient healthTextGradient;

    public Color healthTextColor;

    protected Health health;

    protected FloatingTextSystem floatingTextSystem;

    public float baseSize = 6f;
    public float numberSizeMultiply = 3f;
    public float minRandomMultiplyAmount = 0.4f;
    public float maxRandomMultiplyAmount = 0.4f;

    public float healthCharacterSpacing = -30f;

    protected virtual void Start()
    {
        health = GetComponent<Health>();

        if (health == null) Debug.LogError($"You need a {typeof(Health)} component!", this);

        floatingTextSystem = GetComponent<FloatingTextSystem>();

        if (floatingTextSystem == null) Debug.LogError($"Need a {nameof(floatingTextSystem)} component to also be added!", this);

        health.onTakeDamage += TakeDamage;
    }

    protected virtual void TakeDamage(float amount)
    {
        if (healthUseGradient)
        {
            floatingTextSystem.SpawnText(amount.ToString("F0"),
                healthTextGradient,
                textSize: (baseSize + (numberSizeMultiply * Mathf.Sqrt(5))) * UnityEngine.Random.Range(minRandomMultiplyAmount, maxRandomMultiplyAmount),
                characterSpacing: healthCharacterSpacing);
        }
        else
        {
            floatingTextSystem.SpawnText(amount.ToString("F0"),
                healthTextColor,
                textSize: (baseSize + (numberSizeMultiply * Mathf.Sqrt(5))) * UnityEngine.Random.Range(minRandomMultiplyAmount, maxRandomMultiplyAmount),
                characterSpacing: healthCharacterSpacing);
        }

    }
}
