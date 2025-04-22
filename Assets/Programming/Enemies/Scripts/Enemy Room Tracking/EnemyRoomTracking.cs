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

	[HideInInspector]
	public bool HaltExiting = false;

	private int enemyCount = 0;


	private bool ready = false;

	private bool firedEvent = false;

	[Space]
	public UnityEvent onAllEnemiesKilled;

	private List<Transform> EnemyList = new List<Transform>();

	void Awake()
	{
		boxCollider = GetComponent<BoxCollider>();


		boxCollider.isTrigger = true;


	}

	// TODO fix formatting with fix statements, it looks ugly and hart to track.

	IEnumerator Start()
	{
		while (LevelLoading.instance.loading)
		{
			yield return null;
		}

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

						if (EnemyList.Contains(collider.transform)) continue;

						// we cannot remove this object without risking braking the events.
						collider.GetComponent<AIBase>().onDeath += RemoveEnemy;
						enemyCount++;
						EnemyList.Add(collider.transform);
					}
				}
			}

			if (enemyCount > 0) ready = true;
		}


	}

	/// <summary>
	/// Removes the enemy that died.
	/// </summary>
	/// <param name="EntityTransform">The transform of the enemy that died, may be null.</param>
	private void RemoveEnemy(Transform EntityTransform)
	{
		enemyCount--;
		EnemyList.Remove(EntityTransform);
	}

	void Update()
	{
		// returns if this did not get to setup.
		if (!ready || HaltExiting) return;

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

				if (EnemyList.Contains(other.transform)) return;

				// we cannot remove this object without risking braking the events.
				// we are presuming this object will not be disabled :3.
				other.GetComponent<AIBase>().onDeath += RemoveEnemy;
				enemyCount++;
				EnemyList.Add(other.transform);

				if (!ready) ready = true;
			}
		}

	}

	public void AddEnemy(Transform enemyTransform)
	{
		if (!transform.CompareTag("Enemy")) return;

		enemyTransform.GetComponent<AIBase>().onDeath += RemoveEnemy;
		enemyCount++;
		EnemyList.Add(enemyTransform);
	}


}
