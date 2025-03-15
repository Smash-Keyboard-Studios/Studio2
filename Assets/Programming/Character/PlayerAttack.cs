using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.LightingExplorerTableColumn;

public class PlayerAttack : MonoBehaviour
{
    [Header("Damage Numbers")]
    public int LightDmg = 2;
    public int HeavyDmg = 5;
    public int ChargedHeavyDmg = 0;

    [Header("Cooldown Delays")]
    public float LightAttackDelay;
    public float HeavyAttackDelay;
    public float ChargedHeavyAttackDelay;

    [Header("Attacks being carried out")]
    public bool isAttacking = false;

    public bool lightAttacking = false;
    public bool heavyAttacking = false;
    public bool chargedHeavyAttacking = false;

    private Animator MyAnim;
    [Header("Player Model")] public GameObject MainCharacter;

    [SerializeField] private float heavyAttackRadius = 5f;
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
        StartCoroutine(ChargedHeavyAttack());
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
        MyAnim.SetBool("Attacking", isAttacking);

        DamageEnemy(Physics.OverlapSphere(transform.position, heavyAttackRadius), AttackType.Heavy);

        yield return new WaitForSeconds(HeavyAttackDelay);

        isAttacking = false;
        heavyAttacking = false;
        MyAnim.SetBool("Attacking", isAttacking);
    }

    IEnumerator ChargedHeavyAttack()
    {
        isAttacking = true;
        heavyAttacking = true;
        MyAnim.SetBool("Attacking", isAttacking);

        DamageEnemy(Physics.OverlapSphere(transform.position, chargedHeavyAttackRadius), AttackType.ChargedHeavy);

        yield return new WaitForSeconds(ChargedHeavyAttackDelay);

        isAttacking = false;
        heavyAttacking = false;
        MyAnim.SetBool("Attacking", isAttacking);
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

