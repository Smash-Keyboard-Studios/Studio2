using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAudioBase : MonoBehaviour
{
	protected AIBase aiBase;

	protected virtual void Start()
	{
		aiBase = GetComponent<AIBase>();

		// subscribe to audio
		SubscribeToAudioEvents();
	}

	protected virtual void SubscribeToAudioEvents()
	{
		aiBase.onDeathSFXPlayOnce += DeathSFXPlayOnce;

		aiBase.onTakeDamageSFXPlayOnce += TakeDamageSFXPlayOnce;

		aiBase.onWalkingSFXPlay += WalkingSFXPlay;
		aiBase.onWalkingSFXStop += WalkingSFXStop;
	}


	protected virtual void DeathSFXPlayOnce()
	{
		// audio code here
	}

	protected virtual void TakeDamageSFXPlayOnce()
	{
		// audio code here
	}

	protected virtual void WalkingSFXPlay(float speed)
	{
		// speed is the velocity magnitude of the AI.
		// audio code here
	}

	protected virtual void WalkingSFXStop()
	{
		// audio code here
	}

}
