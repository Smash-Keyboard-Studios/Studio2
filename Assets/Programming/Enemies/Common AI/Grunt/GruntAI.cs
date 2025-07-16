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



/// <summary>
/// Common Melee AI behavior class. Controls movement, attacking and thinking.
/// </summary>
public class GruntAI : AIBase
{

	/// <summary>
	/// Called when attacking the player with an attack, the boolean parameter is false is normal for normal, true is for variant attacks (only animations).
	/// </summary>
	public event Action<bool> onAttack;





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

	[SerializeField]
	protected bool showMelee = false;

	#endregion

	#region Animation Variables

	protected bool attackAnimationPlaying = false;

	protected Animator animatorController;

	[SerializeField, Range(0, 1)]
	protected float attackVariantChance = 0.375f;

	#endregion


	/******************************************************************************/
	#region Functions
	#endregion


	#region Awake
	protected override void Awake()
	{
		base.Awake();

		// path = new NavMeshPath();

		try
		{
			playerTarget = GameObject.FindWithTag(Constants.PlayerTag).transform;
		}
		catch (NullReferenceException)
		{
			Debug.LogError("Cannot find the player!", this);
		}

		animatorController = GetComponentInChildren<Animator>();

		pathTarget = transform.position;
	}
	#endregion



	#region Update
	protected virtual void Update()
	{

		if (UIManager.Instance.inDialogueMenu || UIManager.Instance.inGameMenu)
		{
			pathTarget = transform.position;
			agent.destination = pathTarget;

			animatorController.enabled = false;


			return;
		}
		else
		{
			animatorController.enabled = true;
		}

		agent.destination = pathTarget;


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

		// animations
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
			if (hit.collider.gameObject.CompareTag(Constants.PlayerTag))
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

		pathTarget = playerTarget.position;// + lead; // No lead, path-ing issues.


		if (Vector3.Distance(playerTarget.position, transform.position) < lightAttackClass.minDistanceForAttack)
		{
			// attack // TODO Speed needs to be handled elsewhere. It breaks now with animations. Oh no.
			if (Vector3.Distance(playerTarget.position, transform.position) < 1.55f || attacking) currentSpeed = speedWhileNextToPlayer; // PathTarget = transform.position;
			else currentSpeed = maxSpeed; // PathTarget = PlayerTarget.position;


			if (!attacking && lightAttackCoolDown <= 0f) StartCoroutine(LightAttack());

			transform.rotation = Quaternion.RotateTowards(transform.rotation,
				Quaternion.LookRotation(new Vector3(pathTarget.x, transform.position.y, pathTarget.z) - transform.position, transform.up),
				maxTurningDegreesDelta);

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

		if (UnityEngine.Random.Range(0f, 1f) < attackVariantChance)
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



	#region LightAttackCheckAndDamage
	/// <summary>
	/// Creates a box cast and deals damage to the player if there is one in the box cast.
	/// </summary>
	public virtual void LightAttackCheckAndDamage()
	{

		InvokeOnAttack(animatorController.GetBool("IsHardAttack"));

		lightAttackClass.CheckAndDamage(transform, layersToCheckFor);

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


		//if (NavMesh.CalculatePath(transform.position, pathTarget, filter, path))
		//agent.path = path;

		agent.destination = pathTarget;

		// print("NAV-ING");
	}
	#endregion



	#region OnDrawGizmos
	protected virtual void OnDrawGizmos()
	{
		if (showMelee)
		{
			Gizmos.DrawWireCube(transform.position + transform.forward * lightAttackClass.boxCastForwardOffset, new Vector3(lightAttackClass.boxCastLength, lightAttackClass.boxCastHeight, lightAttackClass.boxCastDepth));
		}

		if (enableVisualDetectionRadius)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, maxDetectionRange);
		}

		if (enableVisualDetectionLine && GameObject.FindWithTag(Constants.PlayerTag) != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, transform.position + (GameObject.FindWithTag(Constants.PlayerTag).transform.position - transform.position).normalized * maxDetectionRange);

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
	protected virtual void InvokeOnAttack(bool value)
	{
		onAttack?.Invoke(value);
	}

	#endregion
}
