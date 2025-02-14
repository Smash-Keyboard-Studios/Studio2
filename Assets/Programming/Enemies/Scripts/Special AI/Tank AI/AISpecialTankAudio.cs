using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpecialTankAudio : AICommonMeleeAudio
{
	protected AISpecialTankCombat aiSpecialTank;

	// Start is called before the first frame update
	protected override void Start()
	{
		aiSpecialTank = GetComponent<AISpecialTankCombat>();

		base.Start();
	}

	protected override void SubscribeToAudioEvents()
	{
		aiSpecialTank.onSpecialAttackStartSFXPlayOnce += OnSpecialAttackStartSFXPlayOnce;

		aiSpecialTank.onSpecialHitGroundSFXPlayOnce += OnSpecialHitGroundSFXPlayOnce;

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

	protected virtual void OnSpecialAttackStartSFXPlayOnce()
	{
		// audio code here
	}

	private void OnSpecialHitGroundSFXPlayOnce()
	{
		// audio code here
	}
}
