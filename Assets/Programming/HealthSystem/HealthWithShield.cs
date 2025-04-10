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
    private float coolDownTime = 10f;
    private float currentCoolDownTime = 0;

    private bool resetShield = false;

    protected override void Start()
    {
        base.Start();
    }


    public override void Reset()
    {
        //currentShieldHealth = maxShieldHealth;


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
            InvokeOnShieldHitSFXPlayOnce();
            if (shieldHitIndicator != null) shieldHitIndicator.ShieldHit();
            return false;
        }

        AddToHealth(-amount);
        hurtIndicator.TakenDamage();
        return true;
    }

    protected override void ActivateShield(bool playSFX = true)
    {
        //currentShieldHealth = maxShieldHealth;
        resetShield = false;
        base.ActivateShield(playSFX);
    }

    public override void BreakShield()
    {
        ShieldDeactivate();
        base.BreakShield();
    }


    protected void ShieldDeactivate()
    {
        currentCoolDownTime = coolDownTime;
        resetShield = true;
    }



}
