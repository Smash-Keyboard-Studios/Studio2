using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{

    private AttackSpace space; // the area of where you attack (the attack space)
    [Header("DamageNumbers")]
    public int LightDmg = 2;
    public int HeavyDmg = 5;
    public float LightAtkDelay;
    public float HeavyAtkDelay;
    public bool isAttacking = false;

    private void Awake()
    {
        space = GetComponentInChildren<AttackSpace>(); // the area of where you attack (the attack space)
    }
    //private Animator animator;

    //private void Awake()
    //{
    //    animator = GetComponent<Animator>();
    //}

    public void OnAttack(InputValue input)
    {
        if (isAttacking) return;
        StartCoroutine(LightAtk());
    }

    public void OnHeavyAttack(InputValue input)
    {
        if (isAttacking) return;
        StartCoroutine (HeavyAtk());
    }

    IEnumerator LightAtk()
    {
        isAttacking = true; 

        foreach (var target in space.TakeDamageInRange.ToList<IDamageable>())
        {
            target.TakeDamage(LightDmg);
        }
        //Debug.Log("Attack"); //change these to the animations that will be added later
        yield return new WaitForSeconds(LightAtkDelay);

        isAttacking = false;
    }

    IEnumerator HeavyAtk()
    {
        isAttacking = true;

        foreach (var target in space.TakeDamageInRange.ToList<IDamageable>())
        {
            target.TakeDamage(HeavyDmg);
        }
        //Debug.Log("Heavy Attack"); //change these to the animations that will be added later
        yield return new WaitForSeconds(HeavyAtkDelay);

        isAttacking = false;
    }
}

