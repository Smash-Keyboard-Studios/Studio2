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

	// Start is called before the first frame update
	protected override void Start()
	{
		gruntAI = GetComponent<GruntAI>();

		base.Start();
	}

	protected override void SubscribeToAudioEvents()
	{
		gruntAI.onAttackSFXPlayOnce += OnAttackSFXPlayOnce;

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

	protected virtual void OnAttackSFXPlayOnce(bool obj)
	{
		// Audio code here
	}
}
