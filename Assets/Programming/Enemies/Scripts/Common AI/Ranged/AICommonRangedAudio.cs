using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICommonRangedAudio : AIAudioBase
{
	//by	_             	  _ _                	 
	// 	   | |           	 (_) |               	 
	//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
	//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \
	// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
	//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|
	// And a few adjustments by
	//     _    _             _  __
	//    / \  | | _____  __ | |/ /
	//   / _ \ | |/ _ \ \/ / | ' / 
	//  / ___ \| | __ />  <  | . \ 
	// /_/   \_\_|\___/_/\_\ |_|\_\
	protected AICommonRangedCombat aiCommonRangedCombat;
	protected override void Start()
    {
		aiCommonRangedCombat = GetComponent<AICommonRangedCombat>();
		aiCommonRangedCombat.onSFXProjectileLaunch += SFXProjectileLaunch;
		base.Start();
	}
	protected override void SubscribeToAudioEvents()
	{
		aiCommonRangedCombat.onAttackSFXPlayOnce += OnAttackSFXPlayOnce;

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

	protected virtual void OnAttackSFXPlayOnce(bool obj)
	{
		// Audio code here
	}
}
