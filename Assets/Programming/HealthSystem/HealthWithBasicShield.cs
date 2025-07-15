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
// © 2025 Dominic McNeill dommcneill@outlook.com
// Licensed for use within the Wraiths of Retail by Smash Keyboard Studios (SKS) only.
// Redistribution or modification outside of this project is prohibited without explicit written permission.
// For full license terms, see DOMIBRON_CODE_LICENSE.md at the project root.


public class HealthWithBasicShield : Health, IShieldObject
{
	[Header("Shield Settings")]
	[SerializeField]
	protected GameObject shieldObject;

	public bool shieldActiveOnStart = true;

	[HideInInspector]
	public bool shieldActive = true;

	// protected ShieldHitIndicator shieldHitIndicator;

	/// <summary>
	/// Called when the shield blocks the hit.
	/// </summary>
	public event Action onShieldHit;

	/// <summary>
	/// Called when the shield is broken.
	/// </summary>
	public event Action onShieldBreak;

	/// <summary>
	/// Called when the shield is activated.
	/// </summary>
	public event Action onShieldActivate;

	/// <summary>
	/// Get whether the shield is active or not.
	/// </summary>
	// TODO remove unnecessary interfaces.
	bool IShieldObject.isShieldActive { get => shieldActive; }

	protected override void Start()
	{

		if (!shieldActiveOnStart)
		{
			currentHealth = maxHealth;
			calledOnDeathEvent = false;

			shieldActive = false;
			shieldObject.SetActive(false);
		}
		else
		{
			Reset();
		}
	}

	/// <summary>
	/// Resets both the health and the shield as well as the on death event.
	/// </summary>
	public override void Reset()
	{
		base.Reset();

		ActivateShield();
	}

	public virtual void BreakShield()
	{
		shieldActive = false;

		shieldObject.SetActive(false);

		InvokeOnShieldBreak();
	}

	public override bool TakeDamage(float amount)
	{
		if (shieldActive)
		{
			InvokeOnShieldHit();

			// SpawnShieldBlockedText();


			// if (shieldHitIndicator != null) shieldHitIndicator.ShieldHit();
			return false;
		}

		return base.TakeDamage(amount);
	}


	public virtual void ActivateShield()
	{
		shieldActive = true;
		shieldObject.SetActive(true);
		InvokeOnShieldActivate();
	}

	/// <summary>
	/// Returns if the shield is active, originally was going to return shield health but that was scrapped.
	/// </summary>
	/// <returns>Returns 1 if the shield is active and 0 if not</returns>
	public virtual float GetShieldNormalized()
	{
		return (shieldActive ? 1 : 0);
	}



	protected void InvokeOnShieldHit()
	{
		onShieldHit?.Invoke();
	}

	protected void InvokeOnShieldBreak()
	{
		onShieldBreak?.Invoke();
	}

	protected void InvokeOnShieldActivate()
	{
		onShieldActivate?.Invoke();
	}

}
