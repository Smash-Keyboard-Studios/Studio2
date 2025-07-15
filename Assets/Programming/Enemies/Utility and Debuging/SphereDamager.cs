using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|
// © 2025 Dominic McNeill dommcneill@outlook.com
// Licensed for use within the Wraiths of Retail by Smash Keyboard Studios (SKS) only.
// Redistribution or modification outside of this project is prohibited without explicit written permission.
// For full license terms, see DOMIBRON_CODE_LICENSE.md at the project root.



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
