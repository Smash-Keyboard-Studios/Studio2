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


#region AIState
/// <summary>
/// The AI States, should dictate thinking.
/// </summary>
public enum AIState
{
	Idle,
	Alerted,
	Retreating,
}
#endregion


#region AITier
/// <summary>
/// The rarity of the AI.
/// </summary>
//[Obsolete("No reason for it to be used at the moment.", false)]
public enum AITier
{
	Common,
	Uncommon,
	Rare,
	Epic,
	Legendary,
	Mythical,
}
#endregion

/// <summary>
/// Holds the core data of the AI.
/// </summary>
[RequireComponent(typeof(NavMeshAgent), typeof(Health))]
public class AIBase : MonoBehaviour
{
	#region Public variables

	// [Header("Tier and stat multipliers")]
	// [SerializeField]
	// protected AITier TierOfAI = AITier.Common;

	// [Header("Health")]
	// [SerializeField]
	// public float maxHealth = 100f;
	// [SerializeField]
	// public float currentHealth;

	[Header("Movement Speed")]
	[SerializeField]
	protected float maxSpeed = 5f;

	protected float currentSpeed;


	/* AI State */
	/// <summary>
	/// The current state the AI is in, at the current time. This Dictates what thinking process it will do.
	/// </summary>
	[Header("AI State")]
	[SerializeField]
	protected AIState currentAIState = AIState.Alerted;

	#endregion
	/********************************************************************/
	#region Public Events AI Events

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

	/// <summary>
	/// Called when the AI state was changed.
	/// </summary>
	public event Action<AIState> onStateChanged;

	#endregion
	/*********************************/
	#region  Public Events SFX

	/// <summary>
	/// Called when the AI moves, the value is velocity / speed.
	/// </summary>
	public event Action<float> onWalkingSFXPlay;

	/// <summary>
	/// Called when the AI stops moving.
	/// </summary>
	public event Action onWalkingSFXStop;

	/// <summary>
	/// Called when taking damage.
	/// </summary>
	public event Action onTakeDamageSFXPlayOnce;

	/// <summary>
	/// Called when the AI dies.
	/// </summary>
	public event Action onDeathSFXPlayOnce;
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
	#endregion


	#region Awake
	protected virtual void Awake()
	{

		currentSpeed = maxSpeed;

		gameObject.tag = "Enemy";

		agent = GetComponent<NavMeshAgent>();

		onStateChanged += OnStateChanged;

		GetComponent<Health>().onNoHealthLeft += KillAI;
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



	// #region Update
	// protected virtual void Update()
	// {
	// 	if (currentHealth <= 0)
	// 	{
	// 		KillAI();
	// 	}
	// }
	// #endregion



	#region KillAI
	/// <summary>
	/// Called when the AI should die. Like when its health is below zero.
	/// </summary>
	protected virtual void KillAI()
	{

		onDeathSFXPlayOnce();
		onDeath?.Invoke(transform);


		Destroy(gameObject);
	}
	#endregion


	protected virtual void OnStateChanged(AIState newState)
	{
		currentAIState = newState;
	}


	// #region IDamageable.TakeDamage
	// bool IDamageable.TakeDamage(float amount)
	// {
	// 	return TakeDamage(amount);
	// }
	// #endregion

	// #region TakeDamage
	// /// <summary>
	// /// Overridable method for taking damage. Will apply the damage to the AI.
	// /// </summary>
	// /// <param name="amount">The amount to take.</param>
	// /// <returns>If it was successful.</returns>
	// protected virtual bool TakeDamage(float amount)
	// {
	// 	TakeDamageSFXPlayOnce();
	// 	currentHealth -= amount;
	// 	return true;
	// }
	// #endregion



	#region Event Invoke Functions
	/* Needed as events cannot be inherited so base classes need to have a function
	to invoke the event.*/

	/// <summary>
	/// Invokes the on walking play event and pass in speed of AI.
	/// </summary>
	/// <param name="value">The speed of the AI / Velocity</param>
	protected virtual void WalkingSFXPlay(float value)
	{
		onWalkingSFXPlay?.Invoke(value);
	}

	/// <summary>
	/// Invokes the on walking stop event.
	/// </summary>
	protected virtual void WalkingSFXStop()
	{
		onWalkingSFXStop?.Invoke();
	}

	/// <summary>
	/// Invokes the on take damage event.
	/// </summary>
	protected virtual void TakeDamageSFXPlayOnce()
	{
		onTakeDamageSFXPlayOnce?.Invoke();
	}

	/// <summary>
	/// Invokes the on death event.
	/// </summary>
	protected virtual void DeathSFXPlayOnce()
	{
		onDeathSFXPlayOnce?.Invoke();
	}


	/// <summary>
	/// Invokes the on state changed event.
	/// </summary>
	/// <param name="newState"></param>
	public virtual void ChangeState(AIState newState)
	{
		onStateChanged?.Invoke(newState);

	}
	#endregion
}