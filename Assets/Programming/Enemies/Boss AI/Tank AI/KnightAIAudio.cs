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


/// <summary>
/// Audio event system for the tank AI.
/// </summary>
public class KnightAIAudio : GruntAIAudio
{
	protected KnightAI knightAI;

	private const string collectionKey = "EnemyKnightBoss.";

	// Start is called before the first frame update
	protected override void Start()
	{
		knightAI = GetComponent<KnightAI>();

		base.Start();
	}

	protected override void SubscribeToAudioEvents()
	{
		knightAI.onSlamAttackStartSFXPlayOnce += OnSlamAttackStartSFXPlayOnce;

		knightAI.onSlamHitGroundSFXPlayOnce += OnSlamHitGroundSFXPlayOnce;

		knightAI.onSlashAttackSFXPlay += OnSlashAttackSFXPlay;
		knightAI.onSlashAttackSFXStop += OnSlashAttackSFXStop;

		base.SubscribeToAudioEvents();
	}



	protected override void OnDeathSFXPlayOnce()
	{
		//aiAudioSource.PlayOneShot(AudioClipFetcher.instance.GetClipFromKey(collectionKey + ))
	}

	protected override void OnWalkingSFXPlay(float speed)
	{
		if (walkSFXPlayTimer <= 0)
		{
			aiAudioSource.PlayOneShot(AudioClipFetcher.instance.GetClipFromKey(collectionKey + "Walk"));
			walkSFXPlayTimer = walkSFXPlayDelay;
		}
	}

	protected override void OnWalkingSFXStop()
	{
		walkSFXPlayTimer = 0;
	}

	protected override void OnAttackSFXPlayOnce(bool obj)
	{
		aiAudioSource.PlayOneShot(AudioClipFetcher.instance.GetClipFromKey(collectionKey + "MeleeAttack"));
	}

	protected virtual void OnSlamAttackStartSFXPlayOnce()
	{
		aiAudioSource.PlayOneShot(AudioClipFetcher.instance.GetClipFromKey(collectionKey + "SlamAttack"));
	}

	protected virtual void OnSlamHitGroundSFXPlayOnce()
	{
		aiAudioSource.PlayOneShot(AudioClipFetcher.instance.GetClipFromKey(collectionKey + "SlamShockWave"));

	}

	protected virtual void OnSlashAttackSFXPlay()
	{
		aiAudioSource.clip = AudioClipFetcher.instance.GetClipFromKey(collectionKey + "SerratedSlashAttack");
		aiAudioSource.Play();
	}

	protected virtual void OnSlashAttackSFXStop()
	{
		aiAudioSource.Stop();

	}
}

