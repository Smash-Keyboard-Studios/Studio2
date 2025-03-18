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


[Serializable]
public class LightAttack
{
	#region Attacking Variables
	/* Attacking */

	/// <summary>
	/// The damage the AI will inflict onto the player.
	/// </summary>
	[Header("Attacking")]
	public float lightAttackDamage = 20f;



	/// <summary>
	/// How long before next attack.
	/// </summary>
	[Tooltip("How long before next attack.")]
	public float lightAttackRate = 1f;



	/// <summary>
	/// The minimum distance possible between the AI and player before the AI will Attack.
	/// </summary>
	public float minDistanceForAttack = 2f;


	//protected Coroutine lightAttackCoroutine;

	#endregion


	#region Box Check Variables
	/* Box check to detect and damage player */

	/// <summary>
	/// AI forward (local Z), how far this will stretch.
	/// </summary>
	[Header("Box check for light attack")]
	public float boxCastDepth = 2f;

	/// <summary>
	/// AI side (local X), how wide this will be.
	/// </summary>
	public float boxCastLength = 3;

	/// <summary>
	/// AI up (local Y), how tall this check box is.
	/// </summary>
	public float boxCastHeight = 1;

	/// <summary>
	/// The offset from the AI position the box check will be.
	/// </summary>
	public float boxCastForwardOffset = 1;



	#endregion
}

/// <summary>
/// Common Melee AI behavior class. Controls movement, attacking and thinking.
/// </summary>
public class GruntAI : AIBase
{
	#region Events
	/// <summary>
	/// Called when using weapon, boolean parameter, false is normal, true is variant.
	/// </summary>
	public event Action<bool> onAttackSFXPlayOnce;


	#endregion

	#region Variables
	#endregion


	#region Light Attack Variables
	[Header("Light Attack Settings"), SerializeField]
	protected LightAttack lightAttackClass;

	/// <summary>
	/// Remaining time left before the AI can attack again.
	/// </summary>
	protected float lightAttackCoolDown = 0f;

	/// <summary>
	/// Weather the AI is currently attacking the player. Used by the IEnumerator.
	/// </summary>
	protected bool attacking = false;
	#endregion

	/// <summary>
	/// The layers it will look for to deal damage to.
	/// </summary>
	[Header("Combat Detection Layer"), SerializeField]
	protected LayerMask layersToCheckFor = Physics.AllLayers;

	#region Detection Variables
	/* The limits for detecting the player */

	/// <summary>
	/// The max range the player can be to be detected.
	/// </summary>
	[Header("AI Detecting Player")]
	[SerializeField]
	protected float maxDetectionRange = 30f;


	/* Path finding */
	/// <summary>
	/// The path used for AI navigation and calculation.
	/// </summary>
	protected NavMeshPath path;

	/// <summary>
	/// The target location for the AI to head to.
	/// </summary>
	protected Vector3 pathTarget;

	/// <summary>
	/// Player reference to compare distances and such.
	/// </summary>
	protected Transform playerTarget;

	#endregion



	#region Turning Variables 
	[Header("Turning Variables and movement"), SerializeField]
	protected float maxTurningDegreesDelta = 0.5f;

	[SerializeField]
	protected float speedWhileNextToPlayer = 0.4f;
	#endregion



	#region Debugging Variables
	/* Debugging */

	/// <summary>
	/// Display's a sphere over the AI to show it's max range.
	/// </summary>
	[Header("Debug Only")]
	[SerializeField]
	protected bool enableVisualDetectionRadius = false;

	/// <summary>
	/// Displays a line towards the player to see if the player can get detected, and to see if an object overlaps.
	/// </summary>
	[SerializeField]
	protected bool enableVisualDetectionLine = false;

	#endregion

	#region Animation Variables

	protected bool attackAnimationPlaying = false;

	protected Animator animatorController;

	#endregion


	/******************************************************************************/
	#region Functions
	#endregion


	#region Awake
	protected override void Awake()
	{
		base.Awake();

		path = new NavMeshPath();

		try
		{
			playerTarget = GameObject.FindWithTag("Player").transform;
		}
		catch (NullReferenceException)
		{
			Debug.LogError("Cannot find the player!", this);
		}

		animatorController = GetComponentInChildren<Animator>();

		pathTarget = transform.position;
	}
	#endregion



	#region Start
	protected override void Start()
	{

		InvokeRepeating(nameof(RunPathFinding), 0, 0.25f);

		base.Start();

	}
	#endregion



	#region Update
	protected virtual void Update()
	{

		// set values and deal with timers.
		agent.speed = currentSpeed;
		if (lightAttackCoolDown > 0f) lightAttackCoolDown -= Time.deltaTime;


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


		if (agent.velocity.magnitude <= 0.1f)
		{
			WalkingSFXStop();

		}
		else
		{
			WalkingSFXPlay(agent.velocity.magnitude);
		}

		animatorController.SetFloat("MovementVel", agent.velocity.normalized.magnitude);

	}
	#endregion



	#region IdleThinking
	/// <summary>
	/// How the AI acts when it's currently idle.
	/// </summary>
	protected virtual void IdleThinking()
	{
		pathTarget = transform.position;

		// creates a line cast form the AI and player. and if it is not broken, then AI is in line of sight.

		if (Physics.Linecast(transform.position, playerTarget.position, out RaycastHit hit) &&
		Vector3.Distance(transform.position, playerTarget.position) <= maxDetectionRange)
		{
			// print(hit.transform.name);
			if (hit.collider.gameObject.CompareTag("Player"))
			{
				//transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(pathTarget.x, transform.position.y, pathTarget.z) - transform.position, transform.up), 45);
				ChangeState(AIState.Alerted);
			}
		}
	}
	#endregion



	#region AlertedThinking
	/// <summary>
	/// How the AI acts when it seen / detects the player.
	/// </summary>
	protected virtual void AlertedThinking()
	{
		//Vector3 lead = Vector3.Distance(transform.position, playerTarget.position) < 3f ? Vector3.zero : playerTarget.GetComponent<CharacterController>().velocity;

		pathTarget = playerTarget.position;// + lead;


		if (Vector3.Distance(playerTarget.position, transform.position) < lightAttackClass.minDistanceForAttack)
		{
			// attack // TODO Speed needs to be handled elsewhere. It breaks now with animations
			if (Vector3.Distance(playerTarget.position, transform.position) < 1.55f || attacking) currentSpeed = speedWhileNextToPlayer; // PathTarget = transform.position;
			else currentSpeed = maxSpeed; // PathTarget = PlayerTarget.position;


			if (!attacking && lightAttackCoolDown <= 0f) StartCoroutine(LightAttack());

			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(pathTarget.x, transform.position.y, pathTarget.z) - transform.position, transform.up), maxTurningDegreesDelta);

		}





	}
	#endregion



	#region LightAttack
	/// <summary>
	/// Dealing with attacking the player and dealing damage.
	/// </summary>
	/// <returns></returns>
	protected virtual IEnumerator LightAttack()
	{
		attacking = true;
		lightAttackCoolDown = lightAttackClass.lightAttackRate;

		attackAnimationPlaying = true;

		// this picks wither normal or rare light attack animations.
		// this adds variety to attacks.

		if (UnityEngine.Random.Range(0f, 4f) < 1.5f)
		{
			animatorController.SetBool("IsHardAttack", true);
			AttackSFXPlayOnce(true);
		}
		else
		{
			animatorController.SetBool("IsHardAttack", false);
			AttackSFXPlayOnce(false);
		}


		// we start the attack.
		animatorController.SetBool("IsAttacking", true);

		// we wait for the animation to finish.

		while (attackAnimationPlaying) yield return null;

		attacking = false;

		currentSpeed = maxSpeed;
	}
	#endregion



	#region AnimationAttackFinished
	/// <summary>
	/// Reset animation variables once the attack is finished.
	/// </summary>
	public virtual void AnimationAttackFinished()
	{
		animatorController.SetBool("IsHardAttack", false);
		animatorController.SetBool("IsAttacking", false);
		attackAnimationPlaying = false;

	}
	#endregion



	#region AttackAndDamage
	/// <summary>
	/// Creates a box cast and deals damage to the player if there is one in the box cast.
	/// </summary>
	public virtual void LightAttackCheckAndDamage()
	{
		Collider[] HitObjects = Physics.OverlapBox(transform.position + (transform.forward * lightAttackClass.boxCastForwardOffset), new Vector3(lightAttackClass.boxCastLength, lightAttackClass.boxCastHeight, lightAttackClass.boxCastDepth) / 2f,
				 transform.rotation, layersToCheckFor);

		if (HitObjects.Length > 0)
		{
			foreach (var hitObject in HitObjects)
			{
				//print(hitObject.name);
				if (hitObject.gameObject.CompareTag("Player"))
				{
					hitObject.GetComponent<IDamageable>()?.TakeDamage(lightAttackClass.lightAttackDamage);
				}
			}
		}
	}
	#endregion



	#region RunPathFinding
	/// <summary>
	/// Calculate the path. This should be called in Start with InvokeRepeating to optimize path calculations.
	/// </summary>
	protected virtual void RunPathFinding()
	{
		NavMeshQueryFilter filter = new NavMeshQueryFilter();

		filter.agentTypeID = agent.agentTypeID;

		filter.areaMask = NavMesh.AllAreas;


		if (NavMesh.CalculatePath(transform.position, pathTarget, filter, path))
			agent.path = path;

		// print("NAV-ING");
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



	#region Animation functions
	// this is used by a script im between the animations and this so animations can call functions.
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
