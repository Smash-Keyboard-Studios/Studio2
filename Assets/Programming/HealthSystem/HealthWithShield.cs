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
            return false;
        }

        AddToHealth(-amount);
        return true;
    }

    protected override void ActivateShield()
    {
        //currentShieldHealth = maxShieldHealth;
        resetShield = false;
        base.ActivateShield();
        InvokeShieldActivate();
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
