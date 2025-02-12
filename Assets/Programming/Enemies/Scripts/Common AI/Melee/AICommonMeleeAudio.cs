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
		aiCommonMeleeCombat.onAttackSFXPlayOnce += AttackSFXPlayOnce;

		base.SubscribeToAudioEvents();
	}

	protected override void DeathSFXPlayOnce()
	{
		// Audio code here
	}

	protected override void TakeDamageSFXPlayOnce()
	{
		// Audio code here
	}

	protected override void WalkingSFXPlay(float speed)
	{
		// Audio code here
	}

	protected override void WalkingSFXStop()
	{
		// Audio code here
	}

	private void AttackSFXPlayOnce(bool obj)
	{
		// Audio code here
	}
}
