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
// © 2025 Dominic McNeill dommcneill@outlook.com
// Licensed for use within the Wraiths of Retail by Smash Keyboard Studios (SKS) only.
// Redistribution or modification outside of this project is prohibited without explicit written permission.
// For full license terms, see DOMIBRON_CODE_LICENSE.md at the project root.


public class FireTickManager : MonoBehaviour
{

    public float defaultTickRate = 0.5f;

    private float timer = 0f;

    private float damage = 0f;

    private float tickRate = 0.5f;

    private float tickingTimer = 0f;

    private Health health;

    private bool wasOnFire = false;

    public event Action onFireTick;
    public event Action onFireStart;
    public event Action onFireEnd;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();

        if (health == null)
        {
            Debug.LogError("You need a health script for this to work!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            tickingTimer += Time.deltaTime;
        }
        else
        {
            tickingTimer = 0f;
        }

        if (timer <= 0 && wasOnFire)
        {
            onFireEnd?.Invoke();
            wasOnFire = false;
        }

        if (tickingTimer >= tickRate)
        {
            onFireTick?.Invoke();
            health.TakeDamage(damage);
            tickingTimer = 0;
        }

    }

    public void SetOnFire(float duration, float damage, float tickRate = -1)
    {
        onFireStart?.Invoke();


        if (duration > timer) timer = duration;

        if (damage > this.damage) this.damage = damage;

        if (tickRate > 0) this.tickRate = tickRate;
        else this.tickRate = defaultTickRate;


        wasOnFire = true;
    }
}
