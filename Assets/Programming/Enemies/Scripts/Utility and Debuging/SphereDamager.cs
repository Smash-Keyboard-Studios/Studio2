using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|



/// <summary>
/// Deals damage to anything inside the sphere.
/// </summary>
public class SphereDamager : MonoBehaviour
{
	[SerializeField]
	private float range = 5;

	[SerializeField]
	private float damage = 15f;

	[SerializeField]
	private bool debugRad = false;


	void Update()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, range);

		if (colliders.Length > 0)
		{
			foreach (Collider collider in colliders)
			{
				collider.GetComponent<IDamageable>()?.TakeDamage(damage);
			}
		}
	}

	void OnDrawGizmos()
	{
		if (debugRad)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position, range);
		}
	}
}
