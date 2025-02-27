using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyBeam : BaseEnemyProjectile
{
	/// <summary> /// Player referance to compare distances and such. /// </summary>
	[SerializeField] protected Transform playerTarget;
	[SerializeField] private Vector3 unmovingTarget;
	[SerializeField] private float increment;
	[SerializeField] LineRenderer lineRenderer;
	protected override void Awake()
	{
		playerTarget = GameObject.FindWithTag("Player").transform;
	}
	protected override void Start()
	{
		unmovingTarget = playerTarget.position;
	}
	protected override void Update()
	{
		increment = increment + Time.deltaTime;
		if (increment <= projectileLifespan)
		{	
			Physics.Linecast(transform.position, unmovingTarget, out RaycastHit hit);
			if (hit.collider.gameObject.CompareTag("Player"))
			{
				lineRenderer.SetPosition(0,transform.position);
				lineRenderer.SetPosition(1, unmovingTarget);
				dealDamage(hit);
			}
		}
		else { Destroy(gameObject); }
	}
	private void dealDamage(RaycastHit hit)
	{
		//Debug.Log("Taken Damage from Beam");
		hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(projectileDamage);
	}
}

