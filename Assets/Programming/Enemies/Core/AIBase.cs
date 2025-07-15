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
// © 2025 Dominic McNeill dommcneill@outlook.com
// Licensed for use within the Wraiths of Retail by Smash Keyboard Studios (SKS) only.
// Redistribution or modification outside of this project is prohibited without explicit written permission.
// For full license terms, see DOMIBRON_CODE_LICENSE.md at the project root.

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

	/// <summary>
	/// Used to set the speed of the AI.
	/// </summary>
	protected float currentSpeed;


	/* AI State */
	/// <summary>
	/// The current state the AI is in, at the current time. This Dictates what thinking process it will do.
	/// </summary>
	[Header("AI State")]
	public AIState currentAIState = AIState.Alerted;

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
	/// Called when the AI state was changed. T1 is old state and T2 is new state.
	/// </summary>
	public event Action<AIState, AIState> onStateChanged;

	#endregion
	/*********************************/
	#region  Public Events SFX

	/// <summary>
	/// Called when the AI moves, the value is velocity / speed.
	/// </summary>
	// public event Action<float> onWalkingSFXPlay; // ! not needed as we can use the velocity.

	/// <summary>
	/// Called when the AI stops moving.
	/// </summary>
	// public event Action onWalkingSFXStop;

	/// <summary>
	/// Called when the AI dies.
	/// </summary>
	// public event Action onDeathSFXPlayOnce; // ! why? no need to duplicate the event, just use the existing one.
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

		gameObject.tag = Constants.EnemyTag;

		agent = GetComponent<NavMeshAgent>();

		onStateChanged += OnStateChanged;

		GetComponent<Health>().onDeath += KillAI;
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




	#region KillAI
	/// <summary>
	/// Called when the AI should die. Like when its health is below zero.
	/// </summary>
	protected virtual void KillAI()
	{

		OnDeathInvoke();



		Destroy(gameObject);
	}
	#endregion


	protected virtual void OnStateChanged(AIState prevState, AIState newState)
	{
		currentAIState = newState;
	}

	public virtual AIState GetAIState()
	{
		return currentAIState;
	}



	#region Event Invoke Functions
	/* Needed as events cannot be inherited so base classes need to have a function
	to invoke the event.*/



	/// <summary>
	/// Invokes the on death event.
	/// </summary>
	protected virtual void OnDeathInvoke()
	{
		onDeath?.Invoke(transform);
	}


	/// <summary>
	/// Invokes the on state changed event.
	/// </summary>
	/// <param name="newState"></param>
	public virtual void ChangeState(AIState newState)
	{
		onStateChanged?.Invoke(currentAIState, newState);

	}
	#endregion
}