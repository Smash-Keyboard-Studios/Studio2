using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{

    [Header("DamageNumbers")]
    public int LightDmg = 2;
    public int HeavyDmg = 5;
    public float LightAtkDelay;
    public float HeavyAtkDelay;
    public bool isAttacking = false;

    private Animator MyAnim;
    public GameObject MainCharacter;

    public BoxCollider LightAtkBoxCollider;

    public float heavyAttackRadius = 5f;

    [Header("Debug")]
    public bool showHeavyRadius = false;

    private enum AtkType
    {
        Light,
        Heavy,
        HeavyCharged
    }

    private void Start()
    {
        MyAnim = MainCharacter.GetComponent<Animator>();
    }

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
        MyAnim.SetBool("Attacking", isAttacking);

        DamageEnemy(Physics.OverlapBox(this.transform.position + LightAtkBoxCollider.center, LightAtkBoxCollider.size / 2, this.transform.rotation), AtkType.Light);
       
        yield return new WaitForSeconds(LightAtkDelay);

        isAttacking = false;
        MyAnim.SetBool("Attacking", isAttacking);
    }

    IEnumerator HeavyAtk()
    {
        isAttacking = true;
        MyAnim.SetBool("Attacking", isAttacking);

        DamageEnemy(Physics.OverlapSphere(transform.position, heavyAttackRadius), AtkType.Heavy);

        yield return new WaitForSeconds(HeavyAtkDelay);

        isAttacking = false;
        MyAnim.SetBool("Attacking", isAttacking);
    }

    private void DamageEnemy(Collider[] enemiesToAttack, AtkType atkType)
    {
        

        //Collider[] HitObjects = Physics.OverlapBox(this.transform.position + LightAtkBoxCollider.center, LightAtkBoxCollider.size / 2, this.transform.rotation);

        if (enemiesToAttack.Length > 1)
        {
            foreach (var hitObject in enemiesToAttack)
            {
                var DamageComp = hitObject.GetComponent<IDamageable>();

                if (DamageComp != null && DamageComp.GetType() != typeof(PlayerStats))
                {
                    switch (atkType)
                    {
                        case AtkType.Light:
                            DamageComp.TakeDamage(LightDmg);
                            break;
                        case AtkType.Heavy:
                            DamageComp.TakeDamage(HeavyDmg);
                            hitObject.transform.GetComponent<IShieldObject>()?.BreakShield();
                            break;
                        case AtkType.HeavyCharged:
                            break;
                    }
                }
            }

        }
    }

    private void OnDrawGizmos()
    {
        if (showHeavyRadius)
        {
            Gizmos.DrawSphere(transform.position, heavyAttackRadius);
        }
    }
}

