using System;
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



[RequireComponent(typeof(BoxCollider))]
public class EnemyRoomTracking : MonoBehaviour
{
	private BoxCollider boxCollider;

	public LayerMask layerToCheckFor = Physics.AllLayers;

	private int enemyCount = 0;

	public UnityEvent onAllEnemiesKilled;

	private bool ready = false;

	void Awake()
	{
		boxCollider = GetComponent<BoxCollider>();


		boxCollider.enabled = false;
		boxCollider.isTrigger = true;


	}

	void Start()
	{
		Collider[] colliders = Physics.OverlapBox(transform.position + boxCollider.center, boxCollider.size / 2f,
		transform.rotation, layerToCheckFor, QueryTriggerInteraction.Ignore);

		if (colliders.Length > 0)
		{
			foreach (Collider collider in colliders)
			{
				if (collider.gameObject.CompareTag("Enemy"))
				{
					if (collider.GetComponent<AIBase>() != null)
					{
						// we cannot remove this object without risking braking the events.
						collider.GetComponent<AIBase>().onDeath += RemoveEnemy;
						enemyCount++;
					}
				}
			}
		}

		ready = true;
	}

	private void RemoveEnemy(Transform EntityTransform)
	{
		enemyCount--;
	}

	void Update()
	{
		if (!ready) return;

		if (enemyCount <= 0)
		{
			onAllEnemiesKilled?.Invoke();
		}
	}



}
