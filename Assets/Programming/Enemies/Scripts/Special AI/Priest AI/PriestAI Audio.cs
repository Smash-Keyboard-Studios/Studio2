using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestAIAudio : AICommonRangedAudio
{
	// by
	// 	   _	_         	  _  __
	//	  / \  | | _____  __ | |/ /
	//   / _ \ | |/ _ \ \/ / | ' /
	//  / ___ \| | __ />  <  | . \
	// /_/   \_\_|\___/_/\_\ |_|\_\
	protected PriestAI aiSpecialRanged;
	protected override void Start()
	{
		aiSpecialRanged = GetComponent<PriestAI>();
		aiSpecialRanged.onSFXProjectileLaunch += SFXProjectileLaunch;
		base.Start();
	}

	protected override void SubscribeToAudioEvents()
	{
		aiSpecialRanged.onAttackSFXPlayOnce += OnAttackSFXPlayOnce;

		base.SubscribeToAudioEvents();
	}

	void SFXProjectileLaunch()
	{
		// Audio people work here, ty
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

	protected virtual void OnSpecialAttackStartSFXPlayOnce()
	{
		// audio code here
	}

	private void OnSpecialHitGroundSFXPlayOnce()
	{
		// audio code here
	}
}
