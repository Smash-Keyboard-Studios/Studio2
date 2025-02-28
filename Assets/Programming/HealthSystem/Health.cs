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
    public float maxHealth = 100;

    public float currentHealth;



    public event Action onNoHealthLeft;

    private bool calledOnDeathEvent = false;



    void Start()
    {
        Reset();
    }

    void Update()
    {
        if (currentHealth <= 0 && !calledOnDeathEvent)
        {
            onNoHealthLeft?.Invoke();
            calledOnDeathEvent = true;
        }
    }

    /// <summary>
    /// Resets the health and on death event. Used for re-spawning.
    /// </summary>
    public void Reset()
    {
        currentHealth = maxHealth;
        calledOnDeathEvent = false;
    }

    /// <summary>
    /// Use this to add to or remove from the health.
    /// </summary>
    /// <param name="amount">The value to add to the health.</param>
    public void AddToHealth(float amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public bool TakeDamage(float amount)
    {
        AddToHealth(-amount);
        return true;
    }
}
