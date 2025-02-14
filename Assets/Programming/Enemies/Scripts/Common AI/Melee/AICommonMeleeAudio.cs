using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICommonMeleeAudio : AIAudioBase
{
	protected AICommonMeleeCombat aiCommonMeleeCombat;

	// Start is called before the first frame update
	protected override void Start()
	{
		aiCommonMeleeCombat = GetComponent<AICommonMeleeCombat>();

		base.Start();
	}

	protected override void SubscribeToAudioEvents()
	{
		aiCommonMeleeCombat.onAttackSFXPlayOnce += OnAttackSFXPlayOnce;

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
