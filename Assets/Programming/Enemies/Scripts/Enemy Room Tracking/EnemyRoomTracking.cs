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


/// <summary>
/// Collects any enemies in a zone and tracks them. Once all of them die it will fire a unity event.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class EnemyRoomTracking : MonoBehaviour
{
	private BoxCollider boxCollider;

	public LayerMask layerToCheckFor = Physics.AllLayers;


	public bool onlyDetectOnStart = false;



	private int enemyCount = 0;


	private bool ready = false;

	private bool firedEvent = false;

	[Space]
	public UnityEvent onAllEnemiesKilled;

	void Awake()
	{
		boxCollider = GetComponent<BoxCollider>();


		boxCollider.enabled = false;
		boxCollider.isTrigger = true;


	}

	void Start()
	{
		// we get all the enemies inside the designated area, we are using a box collider to get the unit measurements.
		Collider[] colliders = Physics.OverlapBox(transform.position + boxCollider.center, boxCollider.size / 2f,
		transform.rotation, layerToCheckFor, QueryTriggerInteraction.Ignore);

		// if we have enemies go through the array and check if they are enemies.
		// if they are enemies, subscript to their death event and increment the counter.
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

	/// <summary>
	/// Removes the enemy that died.
	/// </summary>
	/// <param name="EntityTransform">The transform of the enemy that died, may be null.</param>
	private void RemoveEnemy(Transform EntityTransform)
	{
		enemyCount--;
	}

	void Update()
	{
		// returns if this did not get to setup.
		if (!ready) return;

		// trigger the event when no more enemies in the tracking list.
		if (enemyCount <= 0 && !firedEvent)
		{
			onAllEnemiesKilled?.Invoke();
			firedEvent = true;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (onlyDetectOnStart) return;

		if (other.gameObject.CompareTag("Enemy"))
		{
			if (other.GetComponent<AIBase>() != null)
			{
				// we cannot remove this object without risking braking the events.
				// we are presuming this object will not be disabled :3.
				other.GetComponent<AIBase>().onDeath += RemoveEnemy;
				enemyCount++;
			}
		}

	}


}
