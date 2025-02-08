using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|




/// <summary>
/// Common Melee AI behavior class. Controls movement, attacking and thinking.
/// </summary>
public class AICommonMeleeCombat : AIBase, IAnimationStateUpdator
{
	#region Variables
	#endregion

	#region AI State Vars
	/* AI State */
	/// <summary>
	/// The current state the AI is in, at the current time. This Dictates what thinking process it will do.
	/// </summary>
	[Header("AI State")]
	[SerializeField]
	protected AIState currentAIState = AIState.Alerted;

	#endregion


	#region Attacking Vars
	/* Attacking */

	/// <summary>
	/// The damage the AI will inflict onto the player.
	/// </summary>
	[Header("Attacking")]
	[SerializeField]
	protected float lightAttackDamage = 20f;

	/// <summary>
	/// Remaining time left before the AI can attack again.
	/// </summary>
	protected float lightAttackCooldown = 0f;

	/// <summary>
	/// How long before next attack.
	/// </summary>
	[SerializeField, Tooltip("How long before next attack.")]
	protected float lightAttackRate = 1f;

	/// <summary>
	/// Weather the AI is currently attacking the player. Used by the IEnumerator.
	/// </summary>
	protected bool attacking = false;

	/// <summary>
	/// The minimum distance possible between the AI and player before the AI will Attack.
	/// </summary>
	[SerializeField]
	protected float minDistanceForAttack = 2f;


	protected Coroutine lightAttackCoroutine;

	#endregion


	#region Box Check Vars
	/* Box check to detect and damage player */

	/// <summary>
	/// AI forward (local Z), how far this will stretch.
	/// </summary>
	[Header("Box check for light attack")]
	[SerializeField]
	protected float boxCastThickness = 2f;

	/// <summary>
	/// AI side (local X), how wide this will be.
	/// </summary>
	[SerializeField]
	protected float boxCastLength = 3;

	/// <summary>
	/// AI up (local Y), how tall this check box is.
	/// </summary>
	[SerializeField]
	protected float boxCastHeight = 1;

	/// <summary>
	/// The offsect from the AI positon the box check will be.
	/// </summary>
	[SerializeField]
	protected Vector3 boxCastOffsetFromAI = Vector3.forward;

	/// <summary>
	/// The layers it will look for to deal damage to.
	/// </summary>
	[SerializeField]
	protected LayerMask layersToCheckFor = Physics.AllLayers;

	#endregion


	#region Detection Vars
	/* The limits for detecting the player */

	/// <summary>
	/// The max range the player can be to be detected.
	/// </summary>
	[Header("AI Detecting Player")]
	[SerializeField]
	protected float maxDetectionRange = 30f;


	/* Pathfinding */
	/// <summary>
	/// The path used for AI navigation and calculation.
	/// </summary>
	protected NavMeshPath path;

	/// <summary>
	/// The target location for the AI to head to.
	/// </summary>
	protected Vector3 pathTarget;

	/// <summary>
	/// Player referance to compare distances and such.
	/// </summary>
	protected Transform playerTarget;

	#endregion


	#region Debugging Vars
	/* Debugging */

	/// <summary>
	/// Display's a sphere over the AI to show it's max range.
	/// </summary>
	[Header("Debug Only")]
	[SerializeField]
	protected bool enableVisualDetectionRadius = false;

	/// <summary>
	/// Displays a line towards the player to see if the player can get detected, and to see if an onbject overlaps.
	/// </summary>
	[SerializeField]
	protected bool enableVisualDetectionLine = false;

	#endregion

	#region Animation Vars

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

		playerTarget = GameObject.FindWithTag("Player").transform;

		animatorController = GetComponentInChildren<Animator>();

		pathTarget = transform.position;
	}
	#endregion



	#region Start
	protected override void Start()
	{
		base.Start();

		InvokeRepeating(nameof(RunPathfinding), 0, 0.25f);


	}
	#endregion



	#region Update
	protected override void Update()
	{
		base.Update();

		// set values and deal with timers.
		agent.speed = currentSpeed;
		if (lightAttackCooldown > 0f) lightAttackCooldown -= Time.deltaTime;


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
				currentAIState = AIState.Alerted;
		}
	}
	#endregion



	#region AlertedThinking
	/// <summary>
	/// How the AI acts when it seen / detects the player.
	/// </summary>
	protected virtual void AlertedThinking()
	{
		if (Vector3.Distance(playerTarget.position, transform.position) < minDistanceForAttack)
		{
			// attack // TODO Speed needs to be handled elsewhere. It breaks now with animations
			if (Vector3.Distance(playerTarget.position, transform.position) < 1.55f || attacking) currentSpeed = 0.4f; // PathTarget = transform.position;
			else currentSpeed = maxSpeed; // PathTarget = PlayerTarget.position;


			if (!attacking && lightAttackCooldown <= 0f && lightAttackCoroutine == null) lightAttackCoroutine = StartCoroutine(LightAttack());
		}

		pathTarget = playerTarget.position;
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
		lightAttackCooldown = lightAttackRate;

		attackAnimationPlaying = true;


		if (Random.Range(0f, 4f) < 1.5f)
		{
			animatorController.SetBool("IsHardAttack", true);
		}
		else
		{
			animatorController.SetBool("IsHardAttack", false);
		}

		animatorController.SetBool("IsAttacking", true);


		// AttackAndDamage();
		//yield return null;



		while (attackAnimationPlaying) yield return null;

		attacking = false;

		currentSpeed = maxSpeed;

		lightAttackCoroutine = null;
	}
	#endregion



	#region AnimationAttackFinished
	public virtual void AnimationAttackFinished()
	{
		animatorController.SetBool("IsHardAttack", false);
		animatorController.SetBool("IsAttacking", false);
		attackAnimationPlaying = false;

	}
	#endregion



	#region AttackAndDamage
	/// <summary>
	/// Creates a boxcast and deals damage to the player if there is one in the boxcast.
	/// </summary>
	public virtual void AttackAndDamage()
	{
		Collider[] HitObjects = Physics.OverlapBox(transform.position + boxCastOffsetFromAI, new Vector3(boxCastLength, boxCastHeight, boxCastThickness) / 2f,
				 transform.rotation, layersToCheckFor, QueryTriggerInteraction.Ignore);

		if (HitObjects.Length > 0)
		{
			foreach (var hitObject in HitObjects)
			{
				if (hitObject.gameObject.CompareTag("Player"))
				{
					hitObject.GetComponent<IDamagable>()?.TakeDamage(lightAttackDamage);
				}
			}
		}
	}
	#endregion



	#region RunPathfinding
	/// <summary>
	/// Calculate the path. This should be called in Start with InvokeRepeating to optimise path calculations.
	/// </summary>
	protected virtual void RunPathfinding()
	{
		if (NavMesh.CalculatePath(transform.position, pathTarget, NavMesh.AllAreas, path))
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

	void IAnimationStateUpdator.EndAttack()
	{
		AnimationAttackFinished();
	}

	void IAnimationStateUpdator.DealAttack()
	{
		AttackAndDamage();
	}

	void IAnimationStateUpdator.StartAttack()
	{

	}
	#endregion
}
