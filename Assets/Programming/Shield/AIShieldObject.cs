using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|



public class AIShieldObject : MonoBehaviour, IShieldObject
{
    [SerializeField]
    private GameObject shieldObject;

    [SerializeField]
    private float shieldMaxHealth = 10f;

    private float shieldCurrentHealth;

    [SerializeField]
    private float shieldCoolDown = 5f;

    private float shieldCurrentCoolDownTime;

    [SerializeField]
    private float shieldActivationDuration = 5f;

    private float shieldCurrentUseTimer = 0;

    private bool shieldActive = false;

    void Start()
    {
        shieldCurrentHealth = shieldMaxHealth;
    }

    void Update()
    {
        if (shieldCurrentHealth <= 0)
        {
            BreakShield();
        }

        if (shieldCurrentCoolDownTime > 0)
        {
            shieldCurrentCoolDownTime -= Time.deltaTime;
        }

        if (shieldActive && shieldCurrentUseTimer < shieldActivationDuration)
        {
            shieldCurrentUseTimer += Time.deltaTime;
        }
        else if (shieldActive)
        {
            BreakShield();
        }
        else if (!shieldActive)
        {
            shieldCurrentUseTimer = 0;
        }
    }


    public void ActivateShield()
    {
        if (shieldCurrentCoolDownTime > 0) return;

        shieldCurrentHealth = shieldMaxHealth;

        shieldCurrentUseTimer = 0;

        shieldActive = true;
    }

    public void BreakShield()
    {
        shieldCurrentCoolDownTime = shieldActivationDuration;

        shieldActive = false;

    }



    public bool CanUseShield()
    {
        if (shieldCurrentCoolDownTime > 0) return false;

        return true;
    }
}
