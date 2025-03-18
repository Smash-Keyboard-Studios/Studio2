using System;
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
	protected GameObject shieldObject;

	[HideInInspector]
	public bool shieldActive = true;

	public event Action onShieldBreak;
	public event Action onShieldActivate;


	public override void Reset()
	{
		base.Reset();

		ActivateShield();
	}

	public virtual void BreakShield()
	{
		shieldActive = false;

		shieldObject.SetActive(false);

		InvokeShieldBreak();
	}

	public override bool TakeDamage(float amount)
	{
		if (shieldActive)
		{
			return false;
		}

		return base.TakeDamage(amount);
	}

	protected virtual void ActivateShield()
	{
		shieldActive = true;
		shieldObject.SetActive(true);
	}

	protected void InvokeShieldBreak()
	{
		onShieldBreak?.Invoke();
	}

	protected void InvokeShieldActivate()
	{
		onShieldActivate?.Invoke();
	}

	public virtual float GetShieldNormalized()
	{
		return (shieldActive ? 1 : 0);
	}


}
