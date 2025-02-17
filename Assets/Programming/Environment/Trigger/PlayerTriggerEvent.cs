using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|



public class PlayerTriggerEvent : MonoBehaviour
{
	[Header("Settings"), SerializeField]
	private bool triggerOnce = false;

	[Header("Events")]
	public UnityEvent onTriggerEnter;
	public UnityEvent onTriggerStay;
	public UnityEvent onTriggerExit;

	private bool triggered = false;


	void OnTriggerEnter(Collider other)
	{
		if (!other.gameObject.CompareTag("Player")) return;

		if (triggerOnce && triggered)
		{
			return;
		}

		triggered = true;

		onTriggerEnter?.Invoke();
	}

	void OnTriggerStay(Collider other)
	{
		if (!other.gameObject.CompareTag("Player")) return;

		if (triggerOnce && triggered)
		{
			return;
		}

		triggered = true;

		onTriggerStay?.Invoke();
	}

	void OnTriggerExit(Collider other)
	{
		if (!other.gameObject.CompareTag("Player")) return;

		if (triggerOnce && triggered)
		{
			return;
		}

		triggered = true;

		onTriggerExit?.Invoke();
	}
}
