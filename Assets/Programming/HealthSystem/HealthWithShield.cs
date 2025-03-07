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




public class HealthWithShield : HealthWithBasicShield
{
    [SerializeField]
    private float maxShieldHealth = 30f;
    private float currentShieldHealth;

    [SerializeField]
    private float coolDownTime = 10f;
    private float currentCoolDownTime = 0;

    private bool resetShield = false;

    protected override void Start()
    {
        base.Start();
    }


    public override void Reset()
    {
        currentShieldHealth = maxShieldHealth;


        base.Reset();
    }

    void Update()
    {
        if (currentCoolDownTime > 0) currentCoolDownTime -= Time.deltaTime;
        else if (currentCoolDownTime < 0 && resetShield) ActivateShield();

    }

    public override bool TakeDamage(float amount)
    {
        if (shieldActive)
        {
            DamageShield(amount);
            return false;
        }

        AddToHealth(-amount);
        return true;
    }

    protected virtual void DamageShield(float amount)
    {
        currentShieldHealth -= amount;

        if (currentShieldHealth <= 0)
        {
            BreakShield();
        }
    }

    protected override void ActivateShield()
    {
        currentShieldHealth = maxShieldHealth;
        resetShield = false;
        base.ActivateShield();
    }

    public override void BreakShield()
    {

        base.BreakShield();
    }

    protected void ShieldDeactivate()
    {
        currentCoolDownTime = coolDownTime;
        resetShield = true;
    }

}
