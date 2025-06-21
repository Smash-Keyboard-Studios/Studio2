using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTickManager : MonoBehaviour
{

    public float defaultTickRate = 0.5f;

    private float timer = 0f;

    private float damage = 0f;

    private float tickRate = 0.5f;

    private float tickingTimer = 0f;

    private Health health;

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

        if (tickingTimer >= tickRate)
        {
            health.TakeDamage(damage);
            tickingTimer = 0;
        }

    }

    public void SetOnFire(float duration, float damage, float tickRate = -1)
    {
        if (duration > timer) timer = duration;

        if (damage > this.damage) this.damage = damage;

        if (tickRate > 0) this.tickRate = tickRate;
        else this.tickRate = defaultTickRate;
    }
}
