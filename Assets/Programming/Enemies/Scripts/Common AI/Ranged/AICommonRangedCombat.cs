



using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System;

//by	_               	_ _                	 
//    	| |            	(_) |               	 
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|
// And a few adjustments by
// 	_	_         	_  __
//	/ \  | | _____  __ | |/ /
//   / _ \ | |/ _ \ \/ / | ' /
//  / ___ \| | __ />  <  | . \
// /_/   \_\_|\___/_/\_\ |_|\_\





public class AICommonRangedCombat : AIBase
{
	#region Events
	/// <summary>
	/// Called when using weapon, boolean parameter, false is normal, true is variant.
	/// </summary>
	public event Action<bool> onAttackSFXPlayOnce;


	#endregion
	#region Variables
	#endregion

	#region Attacking Vars
	/* Attacking */
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
	protected float speedReduction = -0.1f; // Reducing enemy movement for ranged attacks
	/// <summary> /// Global delay between all attacks variables /// </summary>
	[Header("Delay between all attacks")]
	[SerializeField] protected float globalAttackCoolDown = 0f;
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
	#region Ranged Vars
	[Header("Ranged Specific")]
	[SerializeField] public float rangedDamage;
	[SerializeField] protected float rangedLifespan; // How long the object will last
	[SerializeField] public float rangedSpeed;
	[SerializeField] public bool isBeam;
	#endregion
	#region Projectile Vars
	[Header("Projectiles")]
	[SerializeField] protected GameObject projectilePrefab;
	[SerializeField] protected Transform[] projectileSpawnPoint;


	#endregion
	#region Beam Variant
	[Header("Beam Specific")]
	[SerializeField] protected GameObject beamPrefab;
	[SerializeField] protected Transform[] beamSpawnPoint;
	#endregion	
	#region Retreat Vars
	[Header("Retreating")]
	[SerializeField] protected float retreatDistance = 10f; // The distance from the player to the enemy to trigger a retreat
	[SerializeField] protected float retreatCooldown = 6f; // Time between retreats
	protected float retreatTimer = 0f; // Tracks current time between retreats
	[SerializeField] protected float chaseTimer = 6f; // Tracks how long the enemy is chased for
	protected bool chaseFinished = false; // A bypass to avoid the enemy AI getting stuck/chased forever
	#endregion
	#region Sound Effect Vars
	public event Action onSFXProjectileLaunch;
	public event Action onSFXBeamStart;
	#endregion
	#region Debugging Vars
	/* Debugging */
	/// <summary> /// Display's a sphere over the AI to show it's max range. /// </summary>
	[Header("Debug Only")]
	[SerializeField] protected bool enableVisualDetectionRadius = false;

	/// <summary> /// Displays a line towards the player to see if the player can get detected, and to see if an onbject overlaps. /// </summary>
	[SerializeField] protected bool enableVisualDetectionLine = false;
	[SerializeField] protected float distanceFromPlayer;
	#endregion

	#region Awake
	protected override void Awake()
	{
		base.Awake();
		globalAttackCoolDown = lightAttackRate;
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
	protected virtual void Update()
	{
		distanceFromPlayer = Vector3.Distance(playerTarget.position, transform.position);
		// set values and deal with timers.
		agent.speed = currentSpeed;
		if (lightAttackCooldown > 0f) lightAttackCooldown -= Time.deltaTime;
		if (retreatTimer > 0f) retreatTimer -= Time.deltaTime;
		if (globalAttackCoolDown >= 0) globalAttackCoolDown -= Time.deltaTime;

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

		if (agent.velocity.magnitude <= 0.1f) { WalkingSFXStop(); }
		else { WalkingSFXPlay(agent.velocity.magnitude); }

		animatorController.SetFloat("MovementVel", agent.velocity.normalized.magnitude);


	}
	#endregion
	#region IdleThinking
	/// <summary>	/// How the AI acts when it's currently idle.	/// </summary>
	protected virtual void IdleThinking()
	{
		pathTarget = transform.position;

		// creates a line cast form the AI and player. and if it is not broken, then AI is in line of sight.

		if (Physics.Linecast(transform.position, playerTarget.position, out RaycastHit hit) && Vector3.Distance(transform.position, playerTarget.position) <= maxDetectionRange)
		{
			if (hit.collider.gameObject.CompareTag("Player"))
				ChangeState(AIState.Alerted);
		}
	}
	#endregion



	#region AlertedThinking
	/// <summary>	/// How the AI acts when it seen / detects the player. /// </summary>
	protected virtual void AlertedThinking()
	{
		pathTarget = playerTarget.position;


		if (Vector3.Distance(playerTarget.position, transform.position) < minDistanceForAttack)
		{
			if ((Vector3.Distance(playerTarget.position, transform.position) < retreatDistance) && retreatTimer <= 0f && !chaseFinished) { ChangeState(AIState.Retreating); }
			else
			{
				if (Physics.Linecast(transform.position, playerTarget.position, out RaycastHit hit) && Vector3.Distance(transform.position, playerTarget.position) <= maxDetectionRange) //Checks if the player is still in Line of Sight
				{
					if (hit.collider.gameObject.CompareTag("Player"))
					{
						if (Vector3.Distance(playerTarget.position, transform.position) < minDistanceForAttack || attacking) currentSpeed = speedReduction; // PathTarget = transform.position;
						if (!attacking && lightAttackCooldown <= 0f && lightAttackCoroutine == null && globalAttackCoolDown <= 0f) { lightAttackCoroutine = StartCoroutine(LightAttack()); }
					}
				}
				else
				{
					currentSpeed = maxSpeed;// PathTarget = PlayerTarget.position;
				}
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
			ChangeState(AIState.Alerted);
		}
	}
	#endregion
	#region LightAttack
	/// <summary>	/// Dealing with attacking the player and dealing damage. /// </summary>
	/// <returns></returns>
	protected virtual IEnumerator LightAttack()
	{
		attacking = true;
		lightAttackCooldown = lightAttackRate;
		globalAttackCoolDown = isBeam ? lightAttackRate + rangedLifespan : lightAttackRate;
		transform.LookAt(new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z)); // Turns the AI manually towards the player.

		attackAnimationPlaying = true;

		// this picks wither normal or rare light attack animations.
		// this adds variaty to attacks.

		if (UnityEngine.Random.Range(0f, 4f) < 1.5f)
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
	/// <summary>	/// Reset animation varibles once the attack is finished. /// </summary>
	public virtual void AnimationAttackFinished()
	{
		animatorController.SetBool("IsHardAttack", false);
		animatorController.SetBool("IsAttacking", false);
		attackAnimationPlaying = false;

	}
	#endregion



	#region AttackAndDamage
	/// <summary>
	/// Creates a launches a projectile or a beam using instantiate, and prefabs for both hold the stats, logic and damage triggering.
	/// </summary>
	public virtual void LightAttackCheckAndDamage()
	{
		Transform[] usedSpawn = isBeam ? beamSpawnPoint : projectileSpawnPoint;
		Action usedSFXAction = isBeam ? onSFXBeamStart : onSFXProjectileLaunch;
		GameObject usedpreFab = isBeam ? beamPrefab : projectilePrefab;
		foreach (Transform SpawnPoint in usedSpawn)
		{
			usedSFXAction?.Invoke();
			SpawnPoint.LookAt(playerTarget.position);
			GameObject instance = Instantiate(usedpreFab, SpawnPoint.position, SpawnPoint.rotation);
			if (isBeam)
			{
				instance.GetComponent<BaseEnemyBeam>().rangedDamage = rangedDamage;
				instance.GetComponent<BaseEnemyBeam>().rangedLifespan = rangedLifespan;
				instance.GetComponent<BaseEnemyBeam>().rangedSpeed = rangedSpeed;
			}
			else
			{
				instance.GetComponent<BaseEnemyProjectile>().rangedDamage = rangedDamage;
				instance.GetComponent<BaseEnemyProjectile>().rangedLifespan = rangedLifespan;
				instance.GetComponent<BaseEnemyProjectile>().rangedSpeed = rangedSpeed;
			}
		}
	}
	#endregion
	#region RunPathfinding
	/// <summary> 	/// Calculate the path. This should be called in Start with InvokeRepeating to optimise path calculations.	/// </summary>
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
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(pathTarget, 1);
		}

		if (enableVisualDetectionLine && GameObject.FindWithTag("Player") != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, transform.position + (GameObject.FindWithTag("Player").transform.position - transform.position).normalized * maxDetectionRange);

		}
	}
	#endregion
	#region Animation functions
	public virtual void EndAttack()
	{
		AnimationAttackFinished();
	}

	public virtual void DealAttack()
	{
		LightAttackCheckAndDamage();
	}
	#endregion
	#region Event Invoke Functions
	/// <summary>
	/// Invokes the on attack event.
	/// </summary>
	/// <param name="value">True if this is a variant of the normal attack.</param>
	protected virtual void AttackSFXPlayOnce(bool value)
	{
		onAttackSFXPlayOnce?.Invoke(value);
	}

	#endregion
}
