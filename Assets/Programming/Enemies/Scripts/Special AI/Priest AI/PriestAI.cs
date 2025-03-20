using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using Unity.VisualScripting;
//using static UnityEditor.PlayerSettings; Where'd this come from???
using Unity.Mathematics;
//using System.Numerics;

public class TeleportLocation
{
	public Transform teleportReference;
	public float teleportDistance;
	public TeleportLocation(Transform _teleportReference, float _teleportDistance)
	{
		teleportReference = _teleportReference;
		teleportDistance = _teleportDistance;
	}
}
public class PriestAI : AICommonRangedCombat
{
	// by
	// 	   _	_         	  _  __
	//	  / \  | | _____  __ | |/ /
	//   / _ \ | |/ _ \ \/ / | ' /
	//  / ___ \| | __ />  <  | . \
	// /_/   \_\_|\___/_/\_\ |_|\_\
	PriestAI priestAI;
	//[SerializeField] float furtherestTeleportPoint;
	[SerializeField] protected Transform[] retreatLocations;
	List<Vector3> retreatPositions;
	//[SerializeField] List<TeleportLocation> teleportLocations;
	[SerializeField] protected float beamAttackCooldown;
	[SerializeField] protected float projectileSpread;
	float beamAttackTimer;
	[SerializeField] protected float volleyDelay = 0.125f;
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
		distanceFromPlayer = Vector3.Distance(playerTarget.position, transform.position);
		// set values and deal with timers.
		agent.speed = currentSpeed;
		if (lightAttackCooldown > 0f) lightAttackCooldown -= Time.deltaTime;
		if (retreatTimer > 0f) retreatTimer -= Time.deltaTime;
		if (beamAttackTimer > 0f) beamAttackTimer -= Time.deltaTime;
		
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
		if ((Vector3.Distance(playerTarget.position, transform.position) < retreatDistance)){ chaseTimer -= Time.deltaTime; }

		if (Vector3.Distance(playerTarget.position, transform.position) < minDistanceForAttack)
		{
			if ((Vector3.Distance(playerTarget.position, transform.position) < retreatDistance) && chaseTimer <= 0f){ ChangeState(AIState.Retreating);}
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







		////Transform chosenRetreat;teleportReference
		//List<float> distanceArray = new List<float>();
		////int transformListIncrem = 0;
		//foreach (TeleportLocation chosenLocation in teleportLocations)
		//{
		//	chosenLocation.teleportDistance = Vector3.Distance(playerTarget.position, chosenLocation.teleportReference.position);
		//	//Debug.Log(teleportLocations);
		//}
		//Debug.Log(teleportLocations);
		//furtherestTeleportPoint = teleportLocations.Max(x => x.teleportDistance);
		//Debug.Log(furtherestTeleportPoint);
		//this.gameObject.transform = teleportLocations(furtherestTeleportPoint);
		//float chosenTeleport = UnityEngine.Random.Range(0, retreatLocations.Length);
		//Transform toTeleportTo = retreatLocations.ElementAt((int)chosenTeleport);
		//this.transform.position = toTeleportTo.position;
		//foreach (Transform optionalRetreat in teleportPoints)
		//{
		//	transformListIncrem = transformListIncrem + 1;
		//	distanceArray[transformListIncrem] = Vector3.Distance(playerTarget.position, optionalRetreat.position);
		//}
		//transform.position = chosenRetreat;
		//if (chaseTimer <= 0) { chaseFinished = true; }//If enemy isn't able to retreat to safe distance or the player keeps chasing, bypass back to attacking.
		//if (!chaseFinished)
		//{
		//	retreatTimer = retreatCooldown;
		//	Vector3 directionToPlayer = playerTarget.position - transform.position; // Calculates the direction towards the player
		//	Vector3 oppositeDirection = transform.position - directionToPlayer; // Calcualates the direction away from the player
		//	pathTarget = oppositeDirection;
		//	chaseTimer -= Time.deltaTime;
		//}

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
				GameObject instance = Instantiate(usedPrefab, SpawnPoint.position, SpawnPoint.rotation);
				instance.GetComponent<BaseEnemyBeam>().rangedDamage = rangedDamage;
				instance.GetComponent<BaseEnemyBeam>().rangedLifespan = rangedLifespan;
				instance.GetComponent<BaseEnemyBeam>().rangedSpeed = rangedSpeed;
			}
			else
			{
					randomRotation *= Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(-projectileSpread, projectileSpread), 0));
					SpawnPoint.rotation = Quaternion.Slerp(SpawnPoint.rotation, randomRotation, 1f);
					GameObject instance = Instantiate(usedPrefab, SpawnPoint.position, SpawnPoint.rotation);
					instance.GetComponent<BaseEnemyProjectile>().rangedDamage = rangedDamage;
					instance.GetComponent<BaseEnemyProjectile>().rangedLifespan = rangedLifespan;
					instance.GetComponent<BaseEnemyProjectile>().rangedSpeed = rangedSpeed;
			}

			yield return new WaitForSeconds(volleyDelay);
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
