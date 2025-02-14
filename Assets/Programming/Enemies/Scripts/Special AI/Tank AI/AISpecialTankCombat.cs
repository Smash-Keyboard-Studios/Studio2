using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|




/// <summary>
/// Special Tank AI behavior class. Controls movement, attacking and thinking.
/// </summary>
public class AISpecialTankCombat : AICommonMeleeCombat, ITankAnimationStateUpdater
{
	#region Tank Slam Attack variables
	[Header("Tank Slam Attack")]

	[SerializeField]
	protected float specialAttackCoolDown = 0f;

	[SerializeField]
	protected float specialAttackRate = 5f;

	protected Coroutine specialAttackCoroutine;

	#endregion



	#region Wind up settings variables
	[Header("Wind up settings")]
	[SerializeField]
	protected float windUpTime = 1f;

	protected float windUpTimer = 1f;

	protected bool isWindingUp = false;

	#endregion



	#region Slam size and damage variables
	[Header("Slam size and damage")]

	[SerializeField]
	protected float slamMaxRadius = 8f;

	[SerializeField]
	protected float slamAttackDamageAtMaxRange = 15f;

	[Space]

	[SerializeField]
	protected float slamMinRadius = 5;

	[SerializeField]
	protected float slamAttackDamageAtMinRange = 55f;

	#endregion



	#region Slam Requirements for activating variables
	[Header("Slam Requirements for activating")]
	[SerializeField]
	protected float minimumDistanceForForceSpecial = 5f;

	protected float slamTimer = 0f;

	[SerializeField]
	protected float timeWithinRadiusBeforeSlam = 3f;

	#endregion



	#region Global delay between all attacks variables
	[Header("Delay between all attacks")]
	[SerializeField]
	protected float globalAttackDelay = 0.5f;

	protected float globalAttackCoolDown = 0f;

	#endregion

	#region Audio Events

	public event Action onSpecialAttackStartSFXPlayOnce;
	public event Action onSpecialHitGroundSFXPlayOnce;

	#endregion


	/******************************************************************************/
	#region Functions
	#endregion



	#region Awake
	protected override void Awake()
	{
		slamTimer = timeWithinRadiusBeforeSlam;

		base.Awake();
	}
	#endregion



	#region Update
	protected override void Update()
	{
		if (isWindingUp && windUpTimer > 0f)
		{
			windUpTimer -= Time.deltaTime;
		}
		else if (isWindingUp && windUpTimer <= 0f)
		{
			animatorController.SetBool("IsCharging", false);
			isWindingUp = false;
		}

		if (globalAttackCoolDown >= 0) globalAttackCoolDown -= Time.deltaTime;
		if (specialAttackCoolDown > 0) specialAttackCoolDown -= Time.deltaTime;

		base.Update();
	}
	#endregion



	#region AlertedThinking
	/// <summary>
	/// How the tank AI thinks while alerted. Handles 2 attacks, light and special.
	/// </summary>
	protected override void AlertedThinking()
	{
		if (Vector3.Distance(playerTarget.position, transform.position) < agent.radius + 0.1f || attacking) currentSpeed = 0.4f;
		else currentSpeed = maxSpeed;



		if (Vector3.Distance(playerTarget.position, transform.position) < minDistanceForAttack)
		{
			if (slamTimer > 0) slamTimer -= Time.deltaTime;


			// we need to decide what attack to use.
			// Special attack.
			if (!attacking && specialAttackCoolDown <= 0f && specialAttackCoroutine == null && globalAttackCoolDown <= 0f &&
			(Vector3.Distance(playerTarget.position, transform.position) < minimumDistanceForForceSpecial || slamTimer <= 0f))
			{
				specialAttackCoroutine = StartCoroutine(SpecialAttack());
				print("Attacking");
			}
			// light attack
			else if (!attacking && lightAttackCoolDown <= 0f && lightAttackCoroutine == null && globalAttackCoolDown <= 0f && slamTimer > 0f)
			{
				lightAttackCoroutine = StartCoroutine(LightAttack());
				print("Attacking light");
			}
		}
		else
		{
			slamTimer = timeWithinRadiusBeforeSlam;

		}

		pathTarget = playerTarget.position;
	}
	#endregion



	#region SpecialAttack
	/// <summary>
	/// Coroutine for dealing with the special attack, it just starts the animations and waits.
	/// </summary>
	/// <returns></returns>
	protected virtual IEnumerator SpecialAttack()
	{
		attacking = true;

		attackAnimationPlaying = true;



		animatorController.SetBool("IsCharging", true);
		animatorController.SetBool("IsAttacking", true);

		windUpTimer = windUpTime;
		isWindingUp = true;

		SpecialAttackStartSFXPlayOnce();

		while (isWindingUp) yield return null;


		animatorController.SetBool("IsSpecialAttack", true);

		specialAttackCoolDown = specialAttackRate;


		while (attackAnimationPlaying) yield return null;

		slamTimer = timeWithinRadiusBeforeSlam;


		attacking = false;

		currentSpeed = maxSpeed;

		specialAttackCoroutine = null;

	}
	#endregion



	#region AnimationAttackFinished
	public override void AnimationAttackFinished()
	{
		animatorController.SetBool("IsCharging", false);
		animatorController.SetBool("IsSpecialAttack", false);


		// this has a end termination.
		base.AnimationAttackFinished();
	}
	#endregion



	#region SpecialAttackCheckAndDamage
	/// <summary>
	/// Creates a check sphere and deals the damage accordingly.
	/// </summary>
	protected virtual void SpecialAttackCheckAndDamage()
	{
		SpecialHitGroundSFXPlayOnce();

		Collider[] HitObjects = Physics.OverlapSphere(transform.position, slamMaxRadius,
			layersToCheckFor, QueryTriggerInteraction.Ignore);

		if (HitObjects.Length > 0)
		{
			foreach (var hitObject in HitObjects)
			{
				if (hitObject.gameObject.CompareTag("Player"))
				{
					float distanceFromPlayer = Vector3.Distance(hitObject.transform.position, transform.position);

					float percentageDistanceWithinOuterRing = (distanceFromPlayer - slamMinRadius) / (slamMaxRadius - slamMinRadius);

					float calculatedDamage = Mathf.Lerp(slamAttackDamageAtMinRange, slamAttackDamageAtMaxRange, percentageDistanceWithinOuterRing);

					hitObject.GetComponent<IDamageable>()?.TakeDamage(calculatedDamage);
				}
			}
		}
	}
	#endregion



	#region ITankAnimationStateUpdater
	// this is used by a script im between the animations and this so animations can call functions.

	/* normal attack */
	void ITankAnimationStateUpdater.EndAttack()
	{
		AnimationAttackFinished();
	}
	void ITankAnimationStateUpdater.DealAttack()
	{
		LightAttackCheckAndDamage();
	}

	void ITankAnimationStateUpdater.StartAttack()
	{

	}


	/* special attack */
	void ITankAnimationStateUpdater.EndSpecialAttack()
	{
		AnimationAttackFinished();
	}

	void ITankAnimationStateUpdater.DealSpecialAttack()
	{
		SpecialAttackCheckAndDamage();
	}

	void ITankAnimationStateUpdater.StartSpecialAttack()
	{

	}
	#endregion


	#region Event Invoke Functions
	/// <summary>
	/// Invokes the onSpecialAttackStartSFXPlayOnce event;
	/// </summary>
	protected virtual void SpecialAttackStartSFXPlayOnce()
	{
		onSpecialAttackStartSFXPlayOnce?.Invoke();
	}

	/// <summary>
	/// Invokes the onSpecialHitGroundSFXPlayOnce event;
	/// </summary>
	protected virtual void SpecialHitGroundSFXPlayOnce()
	{
		onSpecialHitGroundSFXPlayOnce?.Invoke();
	}
	#endregion
}
