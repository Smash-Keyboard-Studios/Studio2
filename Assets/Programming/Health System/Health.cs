using System;
using System.Collections;
using UnityEngine;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|



/// <summary>
/// The health class to give objects health.
/// </summary>

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField]
    protected float maxHealth = 100;

    protected float currentHealth;




    protected bool calledOnDeathEvent = false;

    // protected HurtIndicatorAuto hurtIndicator;
    // protected FloatingTextSystem floatingTextSystem;

    public event Action onDeath;
    public event Action<float> onTakeDamage;
    public event Action<float> onAddToHealth;

    // public float baseSize = 6f;
    // public float numberSizeMultiply = 3f;
    // public float minRandomMultiplyAmount = 0.4f;
    // public float maxRandomMultiplyAmount = 0.4f;

    // public Color topColor = new Color(1, 0.8f, 0.1f);
    // public Color bottomColor = new Color(1, 0, 0);

    protected virtual void Start()
    {
        // hurtIndicator = GetComponent<HurtIndicatorAuto>();
        // floatingTextSystem = GetComponent<FloatingTextSystem>();


        Reset();
    }


    /// <summary>
    /// Resets the health and on death event. Used for re-spawning.
    /// </summary>
    public virtual void Reset()
    {
        currentHealth = maxHealth;
        calledOnDeathEvent = false;
    }

    /// <summary>
    /// Use this to add to or remove from the health.
    /// </summary>
    /// <param name="amount">The value to add to the health.</param>
    public virtual void AddToHealth(float amount)
    {
        currentHealth += amount;

        InvokeOnAddToHealth(amount);

        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (currentHealth <= 0 && !calledOnDeathEvent)
        {
            calledOnDeathEvent = true;
            InvokeOnDeath();
        }
    }

    public virtual bool TakeDamage(float amount)
    {
        AddToHealth(-amount);


        // SpawnDamageText(amount);



        // if (hurtIndicator != null)
        // {
        //     hurtIndicator.TakenDamage();
        // }


        InvokeOnTakeDamage(amount);


        return true;
    }

    // protected void SpawnDamageText(float amount)
    // {
    //     if (floatingTextSystem != null)
    //     {
    //         floatingTextSystem.SpawnTwoToneText(amount.ToString("F0"),
    //         topColor, bottomColor,
    //         textSize: (baseSize + (numberSizeMultiply * Mathf.Sqrt(amount))) * UnityEngine.Random.Range(minRandomMultiplyAmount, maxRandomMultiplyAmount));
    //     }
    // }

    /// <summary>
    /// Gets a normalized version of the health aka as a percentage from 0 to 1.
    /// </summary>
    /// <returns>The percentage from 0 to 1.</returns>
    public virtual float GetHealthNormalized()
    {
        return currentHealth / maxHealth;
    }

    public virtual float ReturnHealthValue()
    {
        return currentHealth;
    }

    protected void InvokeOnTakeDamage(float amount)
    {
        onTakeDamage?.Invoke(amount);
    }

    protected void InvokeOnAddToHealth(float amount)
    {
        onAddToHealth?.Invoke(amount);
    }

    protected void InvokeOnDeath()
    {
        onDeath?.Invoke();
    }
}
