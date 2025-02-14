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
		aiBase.onDeathSFXPlayOnce += OnDeathSFXPlayOnce;

		aiBase.onTakeDamageSFXPlayOnce += OnTakeDamageSFXPlayOnce;

		aiBase.onWalkingSFXPlay += OnWalkingSFXPlay;
		aiBase.onWalkingSFXStop += OnWalkingSFXStop;
	}


	protected virtual void OnDeathSFXPlayOnce()
	{
		// audio code here
	}

	protected virtual void OnTakeDamageSFXPlayOnce()
	{
		// audio code here
	}

	protected virtual void OnWalkingSFXPlay(float speed)
	{
		// speed is the velocity magnitude of the AI.
		// audio code here
	}

	protected virtual void OnWalkingSFXStop()
	{
		// audio code here
	}

}
