using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System;

//by	_             	_ _                	 
// 	| |           	(_) |               	 
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|
// And a few adjustments by
//     _    _             _  __
//    / \  | | _____  __ | |/ /
//   / _ \ | |/ _ \ \/ / | ' / 
//  / ___ \| | __ />  <  | . \ 
// /_/   \_\_|\___/_/\_\ |_|\_\





public class AICommonRangedCombat : AIBase, IAnimationStateUpdator
{
	#region Variables
	#endregion

	#region AI State Vars
	/* AI State */
	/// <summary> /// The current state the AI is in, at the current time. This Dictates what thinking process it will do. /// </summary>
	[Header("AI State")]
	[SerializeField] protected AIState currentAIState = AIState.Alerted;
	#endregion
	#region Attacking Vars
	/* Attacking */

	/// <summary>
	/// The damage the AI will inflict onto the player.
	/// </summary>
	[Header("Attacking")]
	[SerializeField] protected float lightAttackDamage = 20f;

	/// <summary> /// Remaining time left before the AI can attack again. /// </summary>
	protected float lightAttackCooldown = 0f;

	/// <summary> /// How long before next attack. /// </summary>
	[SerializeField, Tooltip("How long before next attack.")]
	protected float lightAttackRate = 1f;

	/// <summary> /// Weather the AI is currently attacking the player. Used by the IEnumerator. /// </summary>
	protected bool attacking = false;

	/// <summary> /// The minimum distance possible between the AI and player before the AI will Attack. /// </summary>
	[SerializeField] protected float minDistanceForAttack = 2f;
	protected Coroutine lightAttackCoroutine;
	private float speedReduction = -0.1f; // Reducing enemy movement for ranged attacks
	#endregion

	#region Detection Vars 
	/* The limits for detecting the player */

	/// <summary> /// The max range the player can be to be detected. /// </summary>
	[Header("AI Detecting Player")]
	[SerializeField] protected float maxDetectionRange = 30f;

	/* Pathfinding */
	/// <summary> /// The path used for AI navigation and calculation. /// </summary>
	protected NavMeshPath path;

	/// <summary> /// The target location for the AI to head to. /// </summary>
	protected Vector3 pathTarget;
	/// <summary> /// Player referance to compare distances and such. /// </summary>
	protected Transform playerTarget;
	#endregion
	#region Animation Vars

	protected bool attackAnimationPlaying = false;

	protected Animator animatorController;
	#endregion
	#region Projectile Vars
	[Header("Projectiles")]
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private Transform[] projectileSpawnPoint;
	[SerializeField] public float projectileDamage;
	[SerializeField] private float projectileLifespan; // How long the object will last
	[SerializeField] public float projectileSpeed;
	[SerializeField] public bool projectileGravityUsage;

	#endregion
	#region Retreat Vars
	[Header("Retreating")]
	[SerializeField] private float retreatDistance = 10f; // The distance from the player to the enemy to trigger a retreat
	[SerializeField] protected float retreatCooldown = 6f; // Time between retreats
	protected float retreatTimer = 0f; // Tracks current time between retreats
	[SerializeField] private float chaseTimer = 6f; // Tracks how long the enemy is chased for
	protected bool chaseFinished = false; // A bypass to avoid the enemy AI getting stuck/chased forever 
	#endregion
	#region Sound Effect Vars
	public event Action sfxOnProjectileLaunch;
	#endregion
	#region Debugging Vars
	/* Debugging */
	/// <summary> /// Display's a sphere over the AI to show it's max range. /// </summary>
	[Header("Debug Only")]
	[SerializeField] protected bool enableVisualDetectionRadius = false;

	/// <summary> /// Displays a line towards the player to see if the player can get detected, and to see if an onbject overlaps. /// </summary>
	[SerializeField] protected bool enableVisualDetectionLine = false;
	[SerializeField] private float distanceFromPlayer;
	#endregion

	#region Awake
	protected override void Awake()
	{
		base.Awake();

		path = new NavMeshPath();
		playerTarget = GameObject.FindWithTag("Player").transform;

		animatorController = GetComponentInChildren<Animator>();

		pathTarget = transform.position;
	}
	#endregion

	#region Start
	protected override void Start()
	{

		InvokeRepeating(nameof(RunPathfinding), 0, 0.25f);

		base.Start();

	}
	#endregion
	#region Update
	protected override void Update()
	{
		base.Update();
		distanceFromPlayer = Vector3.Distance(playerTarget.position, transform.position);
		// set values and deal with timers.
		agent.speed = currentSpeed;
		if (lightAttackCooldown > 0f) lightAttackCooldown -= Time.deltaTime;
		if (retreatTimer > 0f) retreatTimer -= Time.deltaTime;


		// thinking based on current state state.
		// call appropriate functions.

		if (currentAIState == AIState.Idle)
		{
			IdleThinking();
		}
		else if (currentAIState == AIState.Alerted)
		{
			AlertedThinking();
		}
		else if (currentAIState == AIState.Retreating)
		{
			RetreatingThinking();
		}

		if (lightAttackCooldown > 0f) { lightAttackCooldown -= Time.deltaTime; }

		animatorController.SetFloat("MovementVel", agent.velocity.normalized.magnitude);

	}
	#endregion
	#region IdleThinking
	/// <summary>    /// How the AI acts when it's currently idle.    /// </summary>
	protected virtual void IdleThinking()
	{
		pathTarget = transform.position;

		// creates a line cast form the AI and player. and if it is not broken, then AI is in line of sight.

		if (Physics.Linecast(transform.position, playerTarget.position, out RaycastHit hit) && Vector3.Distance(transform.position, playerTarget.position) <= maxDetectionRange)
		{
			if (hit.collider.gameObject.CompareTag("Player"))
				currentAIState = AIState.Alerted;
		}
	}
	#endregion



	#region AlertedThinking
	/// <summary>    /// How the AI acts when it seen / detects the player. /// </summary>
	protected virtual void AlertedThinking()
	{
		pathTarget = playerTarget.position;


		if (Vector3.Distance(playerTarget.position, transform.position) < minDistanceForAttack)
		{
			if ((Vector3.Distance(playerTarget.position, transform.position) < retreatDistance) && retreatTimer <= 0f && !chaseFinished) { currentAIState = AIState.Retreating; }
			else
			{
				if (Vector3.Distance(playerTarget.position, transform.position) < minDistanceForAttack || attacking) currentSpeed = speedReduction; // PathTarget = transform.position;
				else currentSpeed = maxSpeed; // PathTarget = PlayerTarget.position;


				if (!attacking && lightAttackCooldown <= 0f && lightAttackCoroutine == null) lightAttackCoroutine = StartCoroutine(LightAttack());
			}
		}

	}
	#endregion
	#region RetreatingThinking
	///<summary> /// How the AI acts when retreating. /// </summary>
	protected virtual void RetreatingThinking()
	{
		if (chaseTimer <= 0) { chaseFinished = true; }//If enemy isn't able to retreat to safe distance or the player keeps chasing, bypass back to attacking. 
		if (!chaseFinished)
		{
			retreatTimer = retreatCooldown;
			Vector3 directionToPlayer = playerTarget.position - transform.position; // Calculates the direction towards the player
			Vector3 oppositeDirection = transform.position - directionToPlayer; // Calcualates the direction away from the player
			pathTarget = oppositeDirection;
			chaseTimer -= Time.deltaTime;
		}

		if (Vector3.Distance(playerTarget.position, transform.position) >= minDistanceForAttack || chaseFinished)
		{
			chaseTimer = retreatCooldown; // Resets the chase timer 
			currentAIState = AIState.Alerted;
		}
	}
	#endregion
	#region LightAttack
	/// <summary>    /// Dealing with attacking the player and dealing damage. /// </summary>
	/// <returns></returns>
	protected virtual IEnumerator LightAttack()
	{
		attacking = true;
		lightAttackCooldown = lightAttackRate;
		transform.LookAt(new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z)); // Turns the AI manually towards the player.

		attackAnimationPlaying = true;

		// this picks wither normal or rare light attack animations.
		// this adds variaty to attacks.

		if (Random.Range(0f, 4f) < 1.5f)
		{
			animatorController.SetBool("IsHardAttack", true);
		}
		else
		{
			animatorController.SetBool("IsHardAttack", false);
		}


		// we start the attack.
		animatorController.SetBool("IsAttacking", true);

		// we wait for the animation to finish.
		while (attackAnimationPlaying) yield return null;
		attacking = false;

		currentSpeed = maxSpeed;

		lightAttackCoroutine = null;
	}
	#endregion



	#region AnimationAttackFinished
	/// <summary>    /// Reset animation varibles once the attack is finished. /// </summary>
	public virtual void AnimationAttackFinished()
	{
		animatorController.SetBool("IsHardAttack", false);
		animatorController.SetBool("IsAttacking", false);
		attackAnimationPlaying = false;

	}
	#endregion



	#region AttackAndDamage
	/// <summary>
	/// Creates a launches a projectile using instantiate, and projectile prefab itself holds the stats, and damage triggering.
	/// </summary>
	public virtual void LightAttackCheckAndDamage()
	{

		foreach (Transform SpawnPoint in projectileSpawnPoint)
		{
			sfxOnProjectileLaunch?.Invoke();
			SpawnPoint.LookAt(playerTarget.position);
			GameObject instance = Instantiate(projectilePrefab, SpawnPoint.position, SpawnPoint.rotation);
			instance.GetComponent<BaseEnemyProjectile>().projectileDamage = projectileDamage;
			instance.GetComponent<BaseEnemyProjectile>().projectileLifespan = projectileLifespan;
			instance.GetComponent<BaseEnemyProjectile>().projectileSpeed = projectileSpeed;
			instance.GetComponent<Rigidbody>().useGravity = projectileGravityUsage;
		}
	}
	#endregion
	#region RunPathfinding
	/// <summary>     /// Calculate the path. This should be called in Start with InvokeRepeating to optimise path calculations.    /// </summary>
	protected virtual void RunPathfinding()
	{
		NavMeshQueryFilter filter = new NavMeshQueryFilter();

		filter.agentTypeID = agent.agentTypeID;

		filter.areaMask = NavMesh.AllAreas;


		if (NavMesh.CalculatePath(transform.position, pathTarget, filter, path))
			agent.path = path;

		// print("NAVING");
	}
	#endregion
	#region OnDrawGizmos
	protected virtual void OnDrawGizmos()
	{
		if (enableVisualDetectionRadius)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(transform.position, maxDetectionRange);
		}

		if (enableVisualDetectionLine && GameObject.FindWithTag("Player") != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, transform.position + (GameObject.FindWithTag("Player").transform.position - transform.position).normalized * maxDetectionRange);

		}
	}
	#endregion
	#region IAnimationStateUpdator
	// this is used by a script imbetween the animations and this so animations can call functions.
	void IAnimationStateUpdator.EndAttack()
	{
		AnimationAttackFinished();
	}

	void IAnimationStateUpdator.DealAttack()
	{
		LightAttackCheckAndDamage();
	}

	void IAnimationStateUpdator.StartAttack()
	{

	}
	#endregion
}