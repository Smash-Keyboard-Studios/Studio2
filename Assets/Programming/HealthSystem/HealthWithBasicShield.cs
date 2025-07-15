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

	public bool startShieldAsActive = true;

	[HideInInspector]
	public bool shieldActive = true;

	// protected ShieldHitIndicator shieldHitIndicator;

	// audio events
	public event Action onShieldHit;
	public event Action onShieldBreak;
	public event Action onShieldActivate;

	// public Color shieldTopColor = new Color(0.7f, 1f, 1f);
	// public Color shieldBottomColor = new Color(0f, 0.6f, 1f);

	bool IShieldObject.isShieldActive { get => shieldActive; }

	protected override void Start()
	{
		// hurtIndicator = GetComponent<HurtIndicatorAuto>();
		// floatingTextSystem = GetComponent<FloatingTextSystem>();

		// shieldHitIndicator = GetComponent<ShieldHitIndicator>();

		if (!startShieldAsActive)
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

	public override void Reset()
	{
		base.Reset();

		ActivateShield(false);
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

	public virtual void ActivateShield(bool playSFX = true)
	{
		shieldActive = true;
		shieldObject.SetActive(true);
		InvokeOnShieldActivate();
	}

	public virtual float GetShieldNormalized()
	{
		return (shieldActive ? 1 : 0);
	}


	// protected void SpawnShieldBlockedText()
	// {
	// 	if (floatingTextSystem != null)
	// 	{
	// 		floatingTextSystem.SpawnTwoToneText("Blocked",
	// 		shieldTopColor, shieldBottomColor,
	// 		textSize: (baseSize + (numberSizeMultiply * Mathf.Sqrt(5))) * UnityEngine.Random.Range(minRandomMultiplyAmount, maxRandomMultiplyAmount),
	// 		characterSpacing: 0);
	// 	}
	// }



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
