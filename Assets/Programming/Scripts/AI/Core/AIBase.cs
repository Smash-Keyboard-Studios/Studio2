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
/// Holds the core data of the AI.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class AIBase : MonoBehaviour, IDamagable
{
	#region Public variables

	[Header("Health")]
	public float MaxHealth = 100f;
	public float CurrentHealth;

	[Header("Movement Speed")]
	public float Speed = 5f;


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
	public event EntityEventHandler OnDeath;

	/// <summary>
	/// Called when the AI is spawned.
	/// </summary>
	public event EntityEventHandler OnSpawn;


	#endregion
	/******************************************************************************/
	#region Private variables.

	/// <summary>
	/// The agent that is attached to this AI. It must have an agent attached.
	/// </summary>
	private NavMeshAgent Agent;


	#endregion
	/******************************************************************************/
	#region Functions
	// adds spacing for VS scrol bar text.
	#region 
	#endregion

	#region Awake
	protected virtual void Awake()
	{
		CurrentHealth = MaxHealth;
		Agent = GetComponent<NavMeshAgent>();
	}
	#endregion



	#region Start
	// Passing the unity functions to inherited members so they can use start too.
	// Also setting base values so inherited class does not need to.
	protected virtual void Start()
	{
		OnSpawn?.Invoke(transform); // We call the AI on spawn if there are any listeners.
	}
	#endregion



	#region Update
	protected virtual void Update()
	{
		if (CurrentHealth <= 0)
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
		OnDeath?.Invoke(transform);
		gameObject.SetActive(false);
	}
	#endregion



	#region IDamagable.TakeDamage
	bool IDamagable.TakeDamage(float Ammount)
	{
		return TakeDamage(Ammount);
	}
	#endregion

	#region
	/// <summary>
	/// Overridable method for taking damage. Will apply the damage to the AI.
	/// </summary>
	/// <param name="Ammount">The ammount to take.</param>
	/// <returns>If it was successful.</returns>
	protected virtual bool TakeDamage(float Ammount)
	{
		CurrentHealth -= Ammount;
		return true;
	}
	#endregion

	#endregion
}