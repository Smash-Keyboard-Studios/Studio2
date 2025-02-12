using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{

    private AttackSpace space;

    private void Awake()
    {
        space = GetComponentInChildren<AttackSpace>();
    }
    //private Animator animator;

    //private void Awake()
    //{
    //    animator = GetComponent<Animator>();
    //}

    public void OnAttack(InputValue input)
    {
        foreach (var target in space.TakeDamageInRange.ToList<IDamageable>())
        {
            target.TakeDamage(2);
        }
        Debug.Log("Attack"); //change these to the animations that will be added later
    }

    public void OnHeavyAttack(InputValue input)
    {
        foreach (var target in space.TakeDamageInRange.ToList<IDamageable>())
        {
            target.TakeDamage(5);
        }
        Debug.Log("Heavy Attack"); //change these to the animations that will be added later
    }
}
