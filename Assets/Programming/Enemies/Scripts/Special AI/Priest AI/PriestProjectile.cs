using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestProjectile : BaseEnemyProjectile
{
    [SerializeField] protected GameObject aoeObject;
    [SerializeField] protected float aoeDamage;
    [SerializeField] protected float aoeLifespan;
    protected override void OnCollisionEnter(Collision collision)
    {

      
        collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(rangedDamage);
        GameObject instance = Instantiate(aoeObject,transform);
        instance.GetComponent<BaseEnemyProjectile>().rangedDamage = aoeDamage;
        instance.GetComponent<BaseEnemyProjectile>().rangedLifespan = aoeLifespan;
        Destroy(gameObject, 0.05f); //Nearly instantly removes projectile to avoid player clipping
    }
}
