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
	#region Teleport Retreat Variables
	[Header("Teleport prefabs in Priest additonals folder, PLACE AND CONNECT THEM. ")]
	[SerializeField] protected Transform[] retreatLocations;
	List<Vector3> retreatPositions;
	#endregion
	#region Retreat Variables
	[Header("How far the Priest will vary his position and how long he'll run for.")]
	[SerializeField] protected float retreatRange;
	[SerializeField] protected float volleyRunDuration;
	protected float volleyRunTimer;
	protected bool retreatingAfterVolley = false;
	Vector3 randomRetreat;
	#endregion
	#region Variables for Priest attacks
	[Header("Additional Variables for Priest's beam.")]
	[SerializeField] protected float beamAttackCooldown;
	protected float beamAttackTimer;
	[SerializeField] protected float beamDamage;
	[SerializeField] protected float beamTurnSpeed;
	[Header("Additional Variables for Priest's volley.")]
	[SerializeField] protected float projectileSpread;
	[SerializeField] protected float volleyDelay = 0.125f;
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
		//foreach (Transform assignedTransform in retreatLocations) {retreatPositions.Add(assignedTransform.transform.position); }
		base.Start();

	}
	#endregion
	#region Update
	protected override void Update()
	{
		if (UIManager.Instance.inDialogueMenu || UIManager.Instance.inGameMenu)
		{
			pathTarget = transform.position;
			return;
		}
		distanceFromPlayer = Vector3.Distance(playerTarget.position, transform.position);
		// set values and deal with timers.
		agent.speed = currentSpeed;
		if (lightAttackCooldown > 0f) lightAttackCooldown -= Time.deltaTime;
		if (retreatTimer > 0f) retreatTimer -= Time.deltaTime;
		if (beamAttackTimer > 0f) beamAttackTimer -= Time.deltaTime;
		if (beamTracker > 0f) { beamTracker -= Time.deltaTime; beamStillActive = true; } else { beamStillActive = false; }
		if (beamStillActive)
		{
			if (instance != null) { transform.rotation = instance.transform.rotation; }
			pathTarget = transform.position;
			return;
		}
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
		if (volleyRunTimer <= 0)
		{
			retreatingAfterVolley = false;
			volleyRunTimer = volleyRunDuration;
		}
		if (!isBeam && retreatingAfterVolley)
		{
			pathTarget = (transform.position + randomRetreat);

			volleyRunTimer -= Time.deltaTime;
		}
		else
		{

			pathTarget = playerTarget.position;

			if ((Vector3.Distance(playerTarget.position, transform.position) < retreatDistance)) { chaseTimer -= Time.deltaTime; }

			if (Vector3.Distance(playerTarget.position, transform.position) < minDistanceForAttack)
			{
				if ((Vector3.Distance(playerTarget.position, transform.position) < retreatDistance) && chaseTimer <= 0f) { ChangeState(AIState.Retreating); }
				else
				{
					if (Physics.Linecast(transform.position, playerTarget.position, out RaycastHit hit) && Vector3.Distance(transform.position, playerTarget.position) <= maxDetectionRange) //Checks if the player is still in Line of Sight
					{
						if (hit.collider.gameObject.CompareTag("Player"))
						{
							if (Vector3.Distance(playerTarget.position, transform.position) < minDistanceForAttack || attacking) currentSpeed = speedReduction; // PathTarget = transform.position;
							if (beamAttackTimer <= 0f) { isBeam = true; } else { isBeam = false; };
							if (!attacking && lightAttackCooldown <= 0f && lightAttackCoroutine == null) { lightAttackCoroutine = StartCoroutine(LightAttack()); }
						}
					}
					else
					{
						//currentSpeed = maxSpeed;// PathTarget = PlayerTarget.position;
					}
				}
			}
		}
	}
	#endregion
	#region RetreatingThinking
	///<summary> /// How the AI acts when retreating. /// </summary>
	protected override void RetreatingThinking()
	{
		Vector3 furtherestTeleportPoint = Vector3.zero;
		float teleportDistance = 0;

		foreach (var checkingLocation in retreatLocations)
		{

			// if the distance is zero, we know we dont have a point yet, so we set the point to the first pos in the list. assignedTransform.transform.position
			if (teleportDistance == 0)
			{
				teleportDistance = Vector3.Distance(transform.position, checkingLocation.position);
				furtherestTeleportPoint = checkingLocation.position;
				continue;
			}

			if (Vector3.Distance(transform.position, checkingLocation.position) > teleportDistance) // if the distance is bigger to the one we currently have, use this one instead.
			{
				furtherestTeleportPoint = checkingLocation.position;
				teleportDistance = Vector3.Distance(transform.position, checkingLocation.position);
			}
		}
		//furtherestTeleportPoint = retreatPositions.Max(x => x.Distance(transform.position, x));
		// we should have a final point
		if (chaseTimer <= 0)
		{
			transform.position = furtherestTeleportPoint;
		}

		if (Vector3.Distance(playerTarget.position, transform.position) >= minDistanceForAttack)
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

		if (!isBeam)
		{
			randomRetreat = new Vector3(UnityEngine.Random.Range(-retreatRange, retreatRange), 0, UnityEngine.Random.Range(-retreatRange, retreatRange));
			retreatingAfterVolley = true;
		}
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
		StartCoroutine(multifireDelay());
	}


	IEnumerator multifireDelay()
	{
		Transform[] usedSpawn = isBeam ? beamSpawnPoint : projectileSpawnPoint;
		//Action usedSFXAction = isBeam ? onSFXBeamStart : onSFXProjectileLaunch;
		GameObject usedPrefab = isBeam ? beamPrefab : projectilePrefab;
		if (isBeam) { beamAttackTimer = beamAttackCooldown; }
		Quaternion randomRotation;

		foreach (Transform SpawnPoint in usedSpawn)
		{

			SpawnPoint.LookAt(playerTarget.position);
			randomRotation = Quaternion.LookRotation(playerTarget.position - SpawnPoint.position);
			if (isBeam)
			{
				instance = Instantiate(usedPrefab, SpawnPoint.position, SpawnPoint.rotation);
				if (instance != null)
				{
					instance.GetComponent<PriestBeam>().rangedDamage = beamDamage;
					instance.GetComponent<PriestBeam>().rangedLifespan = rangedLifespan;
					instance.GetComponent<PriestBeam>().rangedSpeed = rangedSpeed;
					instance.GetComponent<PriestBeam>().beamWindUp = beamWindUp;
					instance.GetComponent<PriestBeam>().turnSpeed = beamTurnSpeed;
					instance.GetComponent<PriestBeam>().maxAggro = minDistanceForAttack;
					beamTracker = rangedLifespan + beamWindUp;
				}
			}
			else
			{
				randomRotation *= Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(-projectileSpread, projectileSpread), 0));
				SpawnPoint.rotation = Quaternion.Slerp(SpawnPoint.rotation, randomRotation, 1f);
				instance = Instantiate(usedPrefab, SpawnPoint.position, SpawnPoint.rotation);
				instance.GetComponent<PriestProjectile>().rangedDamage = rangedDamage;
				instance.GetComponent<PriestProjectile>().rangedLifespan = rangedLifespan;
				instance.GetComponent<PriestProjectile>().rangedSpeed = rangedSpeed;
				instance.GetComponent<PriestProjectile>().gravityScale = rangedGravity;
			}
		}
		yield return new WaitForSeconds(volleyDelay);

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
