using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestProjectile : BaseEnemyProjectile
{
    [SerializeField] protected GameObject aoeObject;
    [SerializeField] protected float aoeDamage;
    [SerializeField] protected float aoeLifespan;
	[SerializeField] protected float projectileVariety;
	protected override void Start()
	{
		projectileRigidBody.velocity = transform.forward * rangedSpeed * UnityEngine.Random.Range(1, projectileVariety);
	}
	protected override void OnTriggerEnter(UnityEngine.Collider collision)
	{
        RaycastHit floorHit;

		collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(rangedDamage);
        if (Physics.Raycast(transform.position, Vector3.down, out  floorHit)) 
        { 
        GameObject instance = Instantiate(aoeObject, floorHit.point,Quaternion.identity);
        instance.GetComponent<FloorAOE>().rangedDamage = aoeDamage;
        instance.GetComponent<FloorAOE>().rangedLifespan = aoeLifespan;
        Destroy(gameObject, 0.05f); //Nearly instantly removes projectile to avoid player clipping
		}

	}
}
