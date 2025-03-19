using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorAOE : BaseEnemyProjectile
{
	[SerializeField] protected float damageTick;
	[SerializeField] float damageClock;

	protected override void Update()
	{
		if(damageClock >= 0) {  damageClock -= Time.deltaTime; }
		Destroy(gameObject, rangedLifespan);

	}
	protected virtual void OnTriggerEnter(UnityEngine.Collider other)
	{
		Debug.Log(other);
		other.gameObject.GetComponent<IDamageable>()?.TakeDamage(rangedDamage);
	}
	protected virtual void OnTriggerStay(UnityEngine.Collider collision)
	{
		if (damageClock <= 0) 
		{
			Debug.Log(collision);
			collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(rangedDamage);
			damageClock = damageTick;
		}
	}
}
