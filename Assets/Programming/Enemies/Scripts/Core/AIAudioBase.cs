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
/// The base class for audio listeners.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AIAudioBase : MonoBehaviour
{
	protected AIBase aiBase;

	protected AudioSource aiAudioSource;

	[SerializeField]
	protected float walkSFXPlayDelay = 1f;

	protected float walkSFXPlayTimer = 0f;


	protected virtual void Start()
	{
		aiBase = GetComponent<AIBase>();
		aiAudioSource = GetComponent<AudioSource>();

		// subscribe to audio
		SubscribeToAudioEvents();
	}

	protected virtual void SubscribeToAudioEvents()
	{
		aiBase.onDeathSFXPlayOnce += OnDeathSFXPlayOnce;

		aiBase.onWalkingSFXPlay += OnWalkingSFXPlay;
		aiBase.onWalkingSFXStop += OnWalkingSFXStop;
	}

	protected virtual void Update()
	{
		if (walkSFXPlayTimer > 0) walkSFXPlayTimer -= Time.deltaTime;
	}

	protected virtual void OnDeathSFXPlayOnce()
	{
		// audio code here
		AudioManager.Instance.PlayAudio(false, false, aiAudioSource, "Common.Enemy.Death");
	}

	protected virtual void OnWalkingSFXPlay(float speed)
	{
		// speed is the velocity magnitude of the AI.
		// audio code here
		if (walkSFXPlayTimer <= 0)
		{
			AudioManager.Instance.PlayAudio(true, true, aiAudioSource, "Common.Enemy.Walk");
			walkSFXPlayTimer = walkSFXPlayDelay;
		}
	}

	protected virtual void OnWalkingSFXStop()
	{
		// audio code here
		walkSFXPlayTimer = 0;
	}

}
