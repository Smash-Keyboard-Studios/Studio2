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
		// Audio code here
	}

	protected override void OnTakeDamageSFXPlayOnce()
	{
		// Audio code here
	}

	protected override void OnWalkingSFXPlay(float speed)
	{
		// Audio code here
	}

	protected override void OnWalkingSFXStop()
	{
		// Audio code here
	}

	protected override void OnAttackSFXPlayOnce(bool obj)
	{
		// Audio code here
	}

	protected virtual void OnSlamAttackStartSFXPlayOnce()
	{
		// audio code here
	}

	protected virtual void OnSlamHitGroundSFXPlayOnce()
	{
		// audio code here
	}

	protected virtual void OnSlashAttackSFXPlay()
	{
		// audio code here
	}

	protected virtual void OnSlashAttackSFXStop()
	{
		// audio code here
	}
}

