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

	protected ShieldHitIndicator shieldHitIndicator;

	// audio events
	public event Action onShieldHitSFXPlayOnce;
	public event Action onShieldBreakSFXPlayOnce;
	public event Action onShieldActiveSFXPlayOnce;

	protected override void Start()
	{
		shieldHitIndicator = GetComponent<ShieldHitIndicator>();

		base.Start();
	}

	public override void Reset()
	{
		base.Reset();

		ActivateShield(false);
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
			InvokeOnShieldHitSFXPlayOnce();

			if (shieldHitIndicator != null) shieldHitIndicator.ShieldHit();
			return false;
		}

		return base.TakeDamage(amount);
	}

	protected virtual void ActivateShield(bool playSFX = true)
	{
		shieldActive = true;
		shieldObject.SetActive(true);
		InvokeShieldActivate();
		if (playSFX) InvokeOnShieldActivateSFXPlayOnce();
	}

	protected void InvokeShieldBreak()
	{
		onShieldBreak?.Invoke();
		InvokeOnShieldBreakSFXPlayOnce();
	}

	protected void InvokeShieldActivate()
	{
		onShieldActivate?.Invoke();
	}

	public virtual float GetShieldNormalized()
	{
		return (shieldActive ? 1 : 0);
	}

	protected virtual void InvokeOnShieldHitSFXPlayOnce()
	{
		onShieldHitSFXPlayOnce?.Invoke();
	}

	protected virtual void InvokeOnShieldBreakSFXPlayOnce()
	{
		onShieldBreakSFXPlayOnce?.Invoke();
	}

	protected virtual void InvokeOnShieldActivateSFXPlayOnce()
	{
		onShieldActiveSFXPlayOnce?.Invoke();
	}

}
