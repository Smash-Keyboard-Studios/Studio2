using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


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

    public Transform rotation;
    private Health health;
    private PlayerMovementHandler playerMovementHandler;

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
        health = GetComponent<Health>();
        playerMovementHandler = GetComponent<PlayerMovementHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health.GetHealthNormalized() <= 0) return;

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


        if (isHeavyAttackKeyDown && currentHeavyAttackCoolDown <= 0 && heavyAttackUnlocked && !heavyAttacking)
        {
            heavyAttacking = true;

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
        else if (isLightAttackKeyDown && currentLightAttackCoolDown <= 0 && !lightAttacking)
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



    private IEnumerator DealLightAttack()
    {
        yield return new WaitForSeconds(lightAttackDelayForAnimations);


        Collider[] colliders = Physics.OverlapBox(transform.position + rotation.forward * boxCastForwardOffset, new Vector3(boxCastLength, boxCastHeight, boxCastDepth), rotation.rotation);


        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player")) continue;

            collider.GetComponent<IDamageable>()?.TakeDamage(lightAttackDamage);
        }

        lightAttacking = false; // release after animation.
    }

    private IEnumerator DealHeavyAttack(int chargeAmount)
    {
        chargeAmount--; // we reduce it by one max charge is one to high for arrays.

        yield return new WaitForSeconds(heavyAttackDelayForAnimations);
        print(chargeAmount);

        Collider[] colliders = Physics.OverlapSphere(transform.position, heavyAttackSegments[chargeAmount].Range);


        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player")) continue;

            if (collider.GetComponent<IShieldObject>() != null)
            {
                collider.GetComponent<IShieldObject>().BreakShield();
                collider.GetComponent<IDamageable>()?.TakeDamage(heavyAttackSegments[chargeAmount].DamageWithShield);
            }
            else
            {
                collider.GetComponent<IDamageable>()?.TakeDamage(heavyAttackSegments[chargeAmount].Damage);
            }
        }

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
