using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PriestAI : AICommonRangedCombat
{
	// by
	// 	   _	_         	  _  __
	//	  / \  | | _____  __ | |/ /
	//   / _ \ | |/ _ \ \/ / | ' /
	//  / ___ \| | __ />  <  | . \
	// /_/   \_\_|\___/_/\_\ |_|\_\
	PriestAI priestAI;


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

		if (agent.velocity.magnitude <= 0.1f) { WalkingSFXStop(); }
		else { WalkingSFXPlay(agent.velocity.magnitude); }

		animatorController.SetFloat("MovementVel", agent.velocity.normalized.magnitude);


	}
	#endregion
	#region IdleThinking
	/// <summary>	/// How the AI acts when it's currently idle.	/// </summary>
	protected override void IdleThinking()
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
	protected override void AlertedThinking()
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
						if (!attacking && lightAttackCooldown <= 0f && lightAttackCoroutine == null) { lightAttackCoroutine = StartCoroutine(LightAttack()); }
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
	protected override void RetreatingThinking()
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
	protected override IEnumerator LightAttack()
	{
		attacking = true;
		lightAttackCooldown = lightAttackRate;
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
	public override void AnimationAttackFinished()
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
	public override void LightAttackCheckAndDamage()
	{
		Transform[] usedSpawn = isBeam ? beamSpawnPoint : projectileSpawnPoint;
		//Action usedSFXAction = isBeam ? onSFXBeamStart : onSFXProjectileLaunch;
		GameObject usedpreFab = isBeam ? beamPrefab : projectilePrefab;
		foreach (Transform SpawnPoint in usedSpawn)
		{
			//usedSFXAction?.Invoke();
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
	protected override void RunPathfinding()
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
	protected override void OnDrawGizmos()
	{
	}
	#endregion
	#region Animation functions
	public override void EndAttack()
	{
		AnimationAttackFinished();
	}

	public override void DealAttack()
	{
		LightAttackCheckAndDamage();
	}
	#endregion
	#region Event Invoke Functions
	/// <summary>
	/// Invokes the on attack event.
	/// </summary>
	/// <param name="value">True if this is a variant of the normal attack.</param>
	//protected override void AttackSFXPlayOnce(bool value)
	//{
	//	onAttackSFXPlayOnce?.Invoke(value);
	//}

	#endregion
}
