using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|

/// <summary>
/// The AI States, should dictate thinking.
/// </summary>
public enum AIState
{
	Idle = 0,
	Alerted = 1,
}

/// <summary>
/// The raity of the AI.
/// </summary>
[Obsolete("No reason for it to be used at the moment.", false)]
public enum AITier
{
	Common,
	Uncommon,
	Rare,
	Epic,
	Legendary,
}

/// <summary>
/// Holds the core data of the AI.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class AIBase : MonoBehaviour, IDamagable
{
	#region Public variables

	[Header("Tier and stat mult")]
	[SerializeField]
	protected AITier TierOfAI = AITier.Common;

	[Header("Health")]
	[SerializeField]
	protected float maxHealth = 100f;
	[SerializeField]
	protected float murrentHealth;

	[Header("Movement Speed")]
	[SerializeField]
	protected float maxSpeed = 5f;

	protected float currentSpeed;


	#endregion
	/********************************************************************/
	#region Public Events

	/// <summary>
	/// Delegate for the AI spawn and death events so we can pass in the transform.
	/// </summary>
	/// <param name="DeadEntityTransform">The transform of the entity invoking the event.</param>
	public delegate void EntityEventHandler(Transform EntityTransform);

	/// <summary>
	/// Called when the AI dies.
	/// </summary>
	public event EntityEventHandler onDeath;

	/// <summary>
	/// Called when the AI is spawned.
	/// </summary>
	public event EntityEventHandler onSpawn;


	#endregion
	/******************************************************************************/
	#region Private variables.

	/// <summary>
	/// The agent that is attached to this AI. It must have an agent attached.
	/// </summary>
	protected NavMeshAgent agent;


	#endregion
	/******************************************************************************/
	#region Functions
	// adds spacing for VS scrol bar text.
	#region 
	#endregion

	#region Awake
	protected virtual void Awake()
	{
		murrentHealth = maxHealth;

		currentSpeed = maxSpeed;

		agent = GetComponent<NavMeshAgent>();
	}
	#endregion



	#region Start
	// Passing the unity functions to inherited members so they can use start too.
	// Also setting base values so inherited class does not need to.
	protected virtual void Start()
	{
		onSpawn?.Invoke(transform); // We call the AI on spawn if there are any listeners.
	}
	#endregion



	#region Update
	protected virtual void Update()
	{
		if (murrentHealth <= 0)
		{
			KillAI();
		}
	}
	#endregion



	#region KillAI
	/// <summary>
	/// Called when the AI should die. Like when its health is below zero.
	/// </summary>
	protected virtual void KillAI()
	{
		onDeath?.Invoke(transform);
		gameObject.SetActive(false);
	}
	#endregion



	#region IDamagable.TakeDamage
	bool IDamagable.TakeDamage(float ammount)
	{
		return TakeDamage(ammount);
	}
	#endregion

	#region
	/// <summary>
	/// Overridable method for taking damage. Will apply the damage to the AI.
	/// </summary>
	/// <param name="ammount">The ammount to take.</param>
	/// <returns>If it was successful.</returns>
	protected virtual bool TakeDamage(float ammount)
	{
		murrentHealth -= ammount;
		return true;
	}
	#endregion

	#endregion
}