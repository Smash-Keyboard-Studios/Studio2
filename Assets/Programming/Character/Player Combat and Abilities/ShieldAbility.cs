using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|

public class ShieldAbility : MonoBehaviour
{
    public bool unlockedShield;

    public float shieldCoolDownTime = 5f;

    public float shieldDurationTime = 5f;

    private float shieldCoolDownTimer = 0f;
    private float shieldDurationTimer = 0f;

    public GameObject HooverModel;

    private HealthWithBasicShield healthWithBasicShield;

    void Start()
    {
        healthWithBasicShield = GetComponent<HealthWithBasicShield>();
    }

    void Update()
    {
        HooverModel.SetActive(unlockedShield);

        if (shieldDurationTimer > 0)
        {
            shieldDurationTimer -= Time.deltaTime;
        }
        else if (healthWithBasicShield.shieldActive)
        {
            shieldCoolDownTimer = shieldCoolDownTime;
            healthWithBasicShield.BreakShield();
        }

        if (shieldCoolDownTimer > 0)
        {
            shieldCoolDownTimer -= Time.deltaTime;
        }
    }

    public void OnBlock()
    {
        if (unlockedShield)
        {
            if (shieldCoolDownTimer <= 0)
            {
                shieldDurationTimer = shieldDurationTime;
                healthWithBasicShield.ActivateShield();
            }
        }
    }


}
