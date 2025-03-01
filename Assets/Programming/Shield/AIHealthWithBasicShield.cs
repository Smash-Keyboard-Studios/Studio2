using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|



public class AIHealthWithBasicShield : Health, IShieldObject
{
	[Header("Shield Settings")]
	[SerializeField]
	private GameObject shieldObject;

	[SerializeField]
	private float maxShieldHealth = 10f;

	private float currentShieldHealth;

	private bool shieldActive = true;

	protected override void Start()
	{
		base.Start();
		currentShieldHealth = maxShieldHealth;
	}

	public override void Reset()
	{
		base.Reset();

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

	public virtual bool CanUseShield()
	{
		if (shieldActive) return false;

		return true;
	}

	public virtual bool IsUsingShield()
	{
		return shieldActive;
	}
}
