using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorAOE : BaseEnemyProjectile
{
	[SerializeField] protected float damageTick;
	float damageClock;

	protected override void Update()
	{
		if(damageClock >= 0) {  damageClock -= Time.deltaTime; }
		Destroy(gameObject, rangedLifespan);

	}
	protected override void OnTriggerEnter(UnityEngine.Collider other)
	{
		if (other.CompareTag("Player"))
		other.gameObject.GetComponent<IDamageable>()?.TakeDamage(rangedDamage);
	}
	protected virtual void OnTriggerStay(UnityEngine.Collider collision)
	{
		if (damageClock <= 0) 
		{
			collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(rangedDamage);
			damageClock = damageTick;
		}
	}
}
