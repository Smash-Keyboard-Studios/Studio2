using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerAttack : MonoBehaviour
{
    [Header("Damage Numbers")]
    [SerializeField] private int LightDmg = 1;
    [SerializeField] private int HeavyDmg = 2;
    //Adds this amount of damage to the charged heavy attack for every 0.5 secs it is held down
    [SerializeField] private int ChargedHeavyDmgAddition = 1;
    //max damage for charged heavy attack
    [SerializeField] private int MaxChargedHeavyDmg = 5;

    //for the charged heavy
    private int ChargedHeavyDmg = 0;
    private bool isChargingChargedHeavyAttack = false;

    [Header("Cooldown Delays")]
    public float LightAttackDelay = 0.2f;
    public float HeavyAttackDelay = 2f;

    [Header("Attacks being carried out")]
    public bool isAttacking = false;

    public bool lightAttacking = false;
    public bool heavyAttacking = false;

    private Animator MyAnim;
    [Header("Player Model")] public GameObject MainCharacter;

    [SerializeField] private float heavyAttackRadius = 1.5f;
    [SerializeField] private float chargedHeavyAttackRadius = 2f;

    [Header("Debug")]
    [SerializeField] private bool showLightRadius = false;
    [SerializeField] private bool showHeavyRadius = false;

    private enum AttackType
    {
        Light,
        Heavy,
        ChargedHeavy
    }


    private void Awake()
    {
        MyAnim = MainCharacter.GetComponent<Animator>();
    }


    public void OnAttack(InputValue input)
    {
        if (isAttacking) return;
        StartCoroutine(LightAttack());
    }

    public void OnHeavyAttack(InputValue input)
    {
        if (isAttacking) return;
        StartCoroutine (HeavyAttack());
    }

    public void OnChargedHeavyAttack(InputValue input)
    {
        if (isAttacking) return;

        if (input.isPressed) { StartCoroutine("ChargeChargedHeavyAttack"); } //if pressed then start charging
        else { StartCoroutine(ChargedHeavyAttack()); } //if released then stop charging and do attack
    }


    IEnumerator LightAttack()
    {
        isAttacking = true; 
        lightAttacking = true;
        MyAnim.SetBool("Attacking", isAttacking);

        DamageEnemy(Physics.OverlapBox(transform.position + MainCharacter.transform.forward, Vector3.one, MainCharacter.transform.rotation), AttackType.Light);
       
        yield return new WaitForSeconds(LightAttackDelay);

        isAttacking = false;
        lightAttacking= false;
        MyAnim.SetBool("Attacking", isAttacking);
    }

    IEnumerator HeavyAttack()
    {
        isAttacking = true;
        heavyAttacking = true;
        MyAnim.SetBool("HeavyAttacking", isAttacking);

        DamageEnemy(Physics.OverlapSphere(transform.position, heavyAttackRadius), AttackType.Heavy);

        yield return new WaitForSeconds(HeavyAttackDelay);

        //charged heavy attack press gets triggered when heavy attack happens
        //so this is resetting the charging which starts on RMB press
        ChargedHeavyDmg = 0; //reset charged heavy damage

        isAttacking = false;
        heavyAttacking = false;
        MyAnim.SetBool("HeavyAttacking", isAttacking);
    }

    IEnumerator ChargedHeavyAttack()
    {
        isChargingChargedHeavyAttack = false; //stop charging the attack
        MyAnim.SetBool("ChargingHeavyAttack", isChargingChargedHeavyAttack);

        //set charged heavy dmg to heavy dmg if its too low (equivalent to normal heavy attack)
        if (ChargedHeavyDmg < HeavyDmg) { ChargedHeavyDmg = HeavyDmg; }

        isAttacking = true;
        heavyAttacking = true;
        MyAnim.SetBool("Attacking", isAttacking);


        DamageEnemy(Physics.OverlapSphere(transform.position, chargedHeavyAttackRadius), AttackType.ChargedHeavy);

        yield return new WaitForSeconds(HeavyAttackDelay);


        ChargedHeavyDmg = 0; //reset charged heavy damage

        isAttacking = false;
        heavyAttacking = false;
        MyAnim.SetBool("Attacking", isAttacking);
    }


    IEnumerator ChargeChargedHeavyAttack()
    {
        isChargingChargedHeavyAttack = true; //start charging the attack
        MyAnim.SetBool("ChargingHeavyAttack", isChargingChargedHeavyAttack);

        while (isChargingChargedHeavyAttack && ChargedHeavyDmg < MaxChargedHeavyDmg)
        {
            yield return new WaitForSeconds(0.5f);
            ChargedHeavyDmg += ChargedHeavyDmgAddition;
        }
    }


    private void DamageEnemy(Collider[] enemiesToAttack, AttackType atkType)
    {
        if (enemiesToAttack.Length > 1)
        {
            foreach (var hitObject in enemiesToAttack)
            {
                var DamageComp = hitObject.GetComponent<IDamageable>();

                if (DamageComp != null && DamageComp.GetType() != typeof(PlayerStats))
                {
                    switch (atkType)
                    {
                        case AttackType.Light:
                            DamageComp.TakeDamage(LightDmg);
                            break;
                        case AttackType.Heavy:
                            DamageComp.TakeDamage(HeavyDmg);
                            hitObject.transform.GetComponent<IShieldObject>()?.BreakShield();
                            break;
                        case AttackType.ChargedHeavy:
                            DamageComp.TakeDamage(ChargedHeavyDmg);
                            hitObject.transform.GetComponent<IShieldObject>()?.BreakShield();
                            break;
                    }
                }
            }

        }
    }


    private void OnDrawGizmos()
    {
        if (showLightRadius)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position + MainCharacter.transform.forward, MainCharacter.transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }

        if (showHeavyRadius)
        {
            Gizmos.DrawSphere(transform.position, heavyAttackRadius);
        }
    }
}

