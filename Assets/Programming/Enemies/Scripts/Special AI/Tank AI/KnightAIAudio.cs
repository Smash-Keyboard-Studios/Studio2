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
/// Audio event system for the tank AI.
/// </summary>
public class KnightAIAudio : GruntAIAudio
{
	protected KnightAI knightAI;

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
		AudioManager.Instance.PlayAudio(false, false, aiAudioSource, "Knight.Enemy.Death");
	}

	protected override void OnWalkingSFXPlay(float speed)
	{
		if (walkSFXPlayTimer <= 0)
		{
			AudioManager.Instance.PlayAudio(true, true, aiAudioSource, "Knight.Enemy.Walk");
			walkSFXPlayTimer = walkSFXPlayDelay;
		}
	}

	protected override void OnWalkingSFXStop()
	{
		walkSFXPlayTimer = 0;
	}

	protected override void OnAttackSFXPlayOnce(bool obj)
	{
		AudioManager.Instance.PlayAudio(false, false, aiAudioSource, "Knight.Enemy.MeleeAttack");
	}

	protected virtual void OnSlamAttackStartSFXPlayOnce()
	{
		AudioManager.Instance.PlayAudio(false, false, aiAudioSource, "Knight.Enemy.SlamAttack");
	}

	protected virtual void OnSlamHitGroundSFXPlayOnce()
	{
		AudioManager.Instance.PlayAudio(false, false, aiAudioSource, "Knight.Enemy.SlamShockWave");

	}

	protected virtual void OnSlashAttackSFXPlay()
	{
		AudioManager.Instance.PlayAudio(true, false, aiAudioSource, "Knight.Enemy.SerratedSlashAttack");

	}

	protected virtual void OnSlashAttackSFXStop()
	{
		AudioManager.Instance.StopAudio(aiAudioSource);

	}
}

