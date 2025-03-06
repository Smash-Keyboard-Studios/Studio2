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



/// <summary>
/// The health class to give objects health.
/// </summary>

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField]
    private float maxHealth = 100;

    private float currentHealth;



    public event Action onNoHealthLeft;

    private bool calledOnDeathEvent = false;



    protected virtual void Start()
    {
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

        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (currentHealth <= 0 && !calledOnDeathEvent)
        {
            calledOnDeathEvent = true;
            onNoHealthLeft?.Invoke();
        }
    }

    public virtual bool TakeDamage(float amount)
    {
        AddToHealth(-amount);
        return true;
    }

    /// <summary>
    /// Gets a normalized version of the health aka as a percentage from 0 to 1.
    /// </summary>
    /// <returns>The percentage from 0 to 1.</returns>
    public virtual float GetHealthNormalized()
    {
        return currentHealth / maxHealth;
    }
}
