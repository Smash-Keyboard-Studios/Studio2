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




/// <summary>
/// Audio event system for the common melee AI.
/// </summary>
public class GruntAIAudio : AIAudioBase
{
	protected GruntAI gruntAI;

	protected Health aiHealth;

	protected HealthWithBasicShield aiHealthWithBasicShield;

	// Start is called before the first frame update
	protected override void Start()
	{
		gruntAI = GetComponent<GruntAI>();

		aiHealth = GetComponent<Health>();

		aiHealthWithBasicShield = GetComponent<HealthWithBasicShield>();

		base.Start();
	}

	protected override void SubscribeToAudioEvents()
	{
		gruntAI.onAttackSFXPlayOnce += OnAttackSFXPlayOnce;

		aiHealth.onTakenDamageSFXPlayOnce += OnTakenDamageSFXPlayOnce;

		if (aiHealthWithBasicShield != null)
		{
			aiHealthWithBasicShield.onShieldActiveSFXPlayOnce += OnShieldActiveSFXPlayOnce;

			aiHealthWithBasicShield.onShieldBreakSFXPlayOnce += OnShieldBreakSFXPlayOnce;

			aiHealthWithBasicShield.onShieldHitSFXPlayOnce += OnShieldHitSFXPlayOnce;
		}

		base.SubscribeToAudioEvents();
	}

	protected virtual void OnAttackSFXPlayOnce(bool isVariant)
	{
		// Audio code here
		if (!isVariant)
			aiAudioSource.PlayOneShot(AudioClipFetcher.instance.GetClipFromKey("EnemyMelee.Attack"));
		else
			aiAudioSource.PlayOneShot(AudioClipFetcher.instance.GetClipFromKey("EnemyMelee.AttackVariant"));
	}

	protected virtual void OnTakenDamageSFXPlayOnce()
	{
		aiAudioSource.PlayOneShot(AudioClipFetcher.instance.GetClipFromKey("EnemyMelee.TakeDamage"));
	}

	protected virtual void OnShieldActiveSFXPlayOnce()
	{
		aiAudioSource.PlayOneShot(AudioClipFetcher.instance.GetClipFromKey("EnemyMelee.ActivateShield"));
	}

	protected virtual void OnShieldBreakSFXPlayOnce()
	{
		aiAudioSource.PlayOneShot(AudioClipFetcher.instance.GetClipFromKey("EnemyMelee.ShieldBreak"));
	}

	protected virtual void OnShieldHitSFXPlayOnce()
	{
		aiAudioSource.PlayOneShot(AudioClipFetcher.instance.GetClipFromKey("EnemyMelee.ShieldHit"));
	}

	protected override void OnDeathSFXPlayOnce()
	{
		// Handle death noise using death vfx to play.
	}

}
