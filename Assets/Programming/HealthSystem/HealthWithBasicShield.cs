using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|



public class HealthWithBasicShield : Health, IShieldObject
{
	[Header("Shield Settings")]
	[SerializeField]
	private GameObject shieldObject;

	private bool shieldActive = true;

	public override void Reset()
	{
		base.Reset();

		shieldActive = true;
		shieldObject.SetActive(shieldActive);
	}

	public void BreakShield()
	{
		shieldActive = false;

		shieldObject.SetActive(shieldActive);
	}

	public override bool TakeDamage(float amount)
	{
		if (shieldActive)
		{
			return false;
		}

		return base.TakeDamage(amount);
	}
}
