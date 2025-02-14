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
