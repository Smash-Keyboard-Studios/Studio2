using System;
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
// © 2025 Dominic McNeill dommcneill@outlook.com


[Serializable]
public struct HeavyAttackSegment
{
    public float Damage;
    public float DamageWithShield;
    public float Range;
}

public class PlayerAttackHandler : MonoBehaviour
{
    public bool heavyAttackUnlocked = false;

    public GameObject hammerModel; // for attack. (animation????)

    // light attack.

    public float lightAttackDamage = 3;
    public float lightAttackDelay = 0.3f;

    public float lightAttackDelayForAnimations = 0.2f;

    private float currentLightAttackCoolDown = 0f;

    /// <summary>
	/// (local Z)
	/// </summary>
	[Header("Box check for light attack")]
    public float boxCastDepth = 1f;

    /// <summary>
    /// (local X)
    /// </summary>
    public float boxCastLength = 1;

    /// <summary>
    /// (local Y)
    /// </summary>
    public float boxCastHeight = 1;

    /// <summary>
    /// The offset from the player position the box check will be.
    /// </summary>
    public float boxCastForwardOffset = 1;

    // heavy attack

    public HeavyAttackSegment[] heavyAttackSegments;
    public float heavyAttackCoolDown = 3f;
    private float currentHeavyAttackCoolDown = 0f;


    public float timeToChargeHeavyAttackFully = 3f; //  charging heavy segment hit sfx.
    private float currentChargeTime = 0f;

    public float heavyAttackDelayForAnimations = 0.2f;

    private int currentCharge = 0;

    public Transform rotation;
    private Health health;
    private PlayerMovementHandler playerMovementHandler;
    private RingIndicator ringIndicator;
    private Animator playerAnimator;

    // input related.
    private bool isLightAttackKeyDown = false;
    private bool isHeavyAttackKeyDown = false;

    private bool heavyAttacking = false;
    private bool lightAttacking = false;

    [Header("Debug")]
    [SerializeField] private bool showLightRadius = false;
    [SerializeField] private bool showHeavyRadius = false;

    void Awake()
    {
        // health = GetComponent<Health>();
        // health.onDeathEvent += OnDeath;
        playerMovementHandler = GetComponent<PlayerMovementHandler>();
        ringIndicator = GetComponent<RingIndicator>();
        playerAnimator = GetComponentInChildren<Animator>();
    }

    // private void OnDeath()
    // {
    //     enabled = false;
    // }

    // Update is called once per frame
    void Update()
    {
        // if (health.GetHealthNormalized() <= 0) return;

        if (currentLightAttackCoolDown > 0) currentLightAttackCoolDown -= Time.deltaTime;

        if (currentHeavyAttackCoolDown > 0) currentHeavyAttackCoolDown -= Time.deltaTime;



        if (heavyAttacking || lightAttacking)
        {
            playerMovementHandler.canSprint = false;
        }
        else
        {
            playerMovementHandler.canSprint = true;
        }

        // naughty naught directly hooking like this, this can cause problems.
        if (currentCharge != GetChargedHeavyAmount() && GetChargedHeavyAmount() > 0)
        {
            // show ring
            if (GetChargedHeavyAmount() <= heavyAttackSegments.Length)
            {
                ringIndicator.ShowRing(0.1f, heavyAttackSegments[GetChargedHeavyAmount() - 1].Range, (currentCharge == 0 ? 0 : heavyAttackSegments[currentCharge - 1].Range));
            }

            currentCharge = GetChargedHeavyAmount();
        }
        else if (currentCharge != GetChargedHeavyAmount())
        {
            // hide ring
            ringIndicator.HideRing(16f);
            currentCharge = GetChargedHeavyAmount();
        }



        if (isHeavyAttackKeyDown && currentHeavyAttackCoolDown <= 0 && heavyAttackUnlocked)
        {
            heavyAttacking = true;
            playerAnimator.SetBool("ChargingHeavyAttack", true);

            if (currentChargeTime <= 0)
            {
                currentChargeTime = timeToChargeHeavyAttackFully * (1f / heavyAttackSegments.Length);
            }

            currentChargeTime += Time.deltaTime;

            currentChargeTime = Mathf.Clamp(currentChargeTime, 0, timeToChargeHeavyAttackFully);
        }
        else if (!isHeavyAttackKeyDown && currentChargeTime > 0)
        {
            // heavy attack
            // print("H Attack with " + GetChargedHeavyAmount());
            StartCoroutine(DealHeavyAttack(GetChargedHeavyAmount()));
            currentChargeTime = 0;
            currentHeavyAttackCoolDown = heavyAttackCoolDown;
        }
        else if (isLightAttackKeyDown && currentLightAttackCoolDown <= 0)
        {
            lightAttacking = true;
            StartCoroutine(DealLightAttack());
            currentLightAttackCoolDown = lightAttackDelay;
        }
    }

    public void OnAttack(InputValue value)
    {
        isLightAttackKeyDown = value.isPressed;
    }

    public void OnHeavyAttack(InputValue value)
    {
        isHeavyAttackKeyDown = value.isPressed;
    }

    public bool IsHeavyAttackOnCoolDown()
    {
        return currentHeavyAttackCoolDown > 0;
    }

    public float GetHeavyAttackCoolDownNormalized()
    {
        return currentHeavyAttackCoolDown / heavyAttackCoolDown;
    }

    private IEnumerator DealLightAttack()
    {
        playerAnimator.SetBool("Attacking", true);

        yield return new WaitForSeconds(lightAttackDelayForAnimations);


        Collider[] colliders = Physics.OverlapBox(transform.position + rotation.forward * boxCastForwardOffset, new Vector3(boxCastLength, boxCastHeight, boxCastDepth), rotation.rotation);


        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player")) continue;

            collider.GetComponent<IDamageable>()?.TakeDamage(lightAttackDamage);
        }

        playerAnimator.SetBool("Attacking", false);
        lightAttacking = false; // release after animation.
    }

    private IEnumerator DealHeavyAttack(int chargeAmount)
    {
        chargeAmount--; // we reduce it by one max charge is one to high for arrays.
        playerAnimator.SetBool("ChargingHeavyAttack", false);
        playerAnimator.SetBool("HeavyAttacking", true);

        yield return new WaitForSeconds(heavyAttackDelayForAnimations);
        // print(chargeAmount);

        Collider[] colliders = Physics.OverlapSphere(transform.position, heavyAttackSegments[chargeAmount].Range);


        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player")) continue;

            if (collider.GetComponent<IShieldObject>() != null)
            {
                if (collider.GetComponent<IShieldObject>().isShieldActive)
                {
                    collider.GetComponent<IShieldObject>().BreakShield();
                    collider.GetComponent<IDamageable>()?.TakeDamage(heavyAttackSegments[chargeAmount].DamageWithShield);
                }
                else collider.GetComponent<IDamageable>()?.TakeDamage(heavyAttackSegments[chargeAmount].Damage);
            }
            else
            {
                collider.GetComponent<IDamageable>()?.TakeDamage(heavyAttackSegments[chargeAmount].Damage);
            }
        }

        playerAnimator.SetBool("HeavyAttacking", false);
        heavyAttacking = false; // release after animations.
    }

    public int GetChargedHeavyAmount()
    {
        return Mathf.FloorToInt((currentChargeTime / timeToChargeHeavyAttackFully) / (1f / heavyAttackSegments.Length));
    }

    public float GetChargedHeavyAmountNormalized()
    {
        return Mathf.Floor((currentChargeTime / timeToChargeHeavyAttackFully) / (1f / heavyAttackSegments.Length)) / heavyAttackSegments.Length;
    }

    public float GetChargedHeavyNormalized()
    {
        return currentChargeTime / timeToChargeHeavyAttackFully;
    }

    private void OnDrawGizmos()
    {
        if (showLightRadius)
        {
            Gizmos.DrawWireCube(transform.position + rotation.forward * boxCastForwardOffset, new Vector3(boxCastLength, boxCastHeight, boxCastDepth));
        }

        if (showHeavyRadius)
        {
            var gradient = new Gradient();

            var colors = new GradientColorKey[2];
            colors[0] = new GradientColorKey(Color.red, 0f);
            colors[1] = new GradientColorKey(Color.blue, 1f);

            var alphas = new GradientAlphaKey[2];
            alphas[0] = new GradientAlphaKey(1.0f, 0.0f);
            alphas[1] = new GradientAlphaKey(1.0f, 1.0f);

            gradient.SetKeys(colors, alphas);

            for (int i = 0; i < heavyAttackSegments.Length; i++)
            {
                float g = (1f / (heavyAttackSegments.Length - 1f)) * i;
                print(g);
                Gizmos.color = gradient.Evaluate(g);
                Gizmos.DrawWireSphere(transform.position, heavyAttackSegments[i].Range);

            }
        }
    }
}
