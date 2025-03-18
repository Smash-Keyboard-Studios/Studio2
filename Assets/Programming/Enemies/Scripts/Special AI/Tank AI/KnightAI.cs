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


[Serializable]
public class SlamAttack
{
	#region Slam cool down settings
	[Header("Tank Slam Attack")]

	public float slamAttackRateCoolDown = 5f;

	#endregion



	#region Wind up for slam
	[Header("Slam wind up settings")]
	public float slamWindUpTime = 1f;

	#endregion



	#region Slam size and damage variables
	[Header("Slam size and damage")]

	public float slamMaxRadius = 8f;

	public float slamAttackDamageAtMaxRange = 15f;

	[Space]

	public float slamMinRadius = 5;

	public float slamAttackDamageAtMinRange = 55f;

	#endregion



	#region Slam Requirements for activating variables
	[Header("Slam Requirements for activating")]

	public float minimumDistanceForForceSlam = 5f;

	public float timeWithinRadiusBeforeSlam = 3f;
	#endregion
}

[Serializable]
public class SerratedAttackSlash
{
	[Header("Serrated Slash Settings")]

	// cool down
	public float serratedSlashCoolDown = 10f;

	// activation requirements
	public float minimumDistanceForSerratedSlash = 4f;

	// wind up
	public float serratedSlashWindUpTime = 0.5f;

	// damage and radius
	public float serratedSlashRadius = 5f;

	public float serratedSlashDamage = 2f;

	public float serratedSlashTickLength = 0.25f;

	public float serratedSlashAttackDuration = 1f;
}

/// <summary>
/// Special Tank AI behavior class. Controls movement, attacking and thinking.
/// </summary>
[RequireComponent(typeof(HealthWithShield))]
public class KnightAI : GruntAI
{
	#region Tank Slam Attack variables
	[Header("Slam Settings"), SerializeField]
	protected SlamAttack slamAttackClass;

	protected float slamAttackCoolDownTimer = 0f;

	protected float slamWindUpTimer = 1f;

	protected bool isSlamWindingUp = false;

	protected float slamTimer = 0f;
	#endregion



	#region Serrated Slash
	[Header("Serrated Slash Settings")]
	[SerializeField]
	protected SerratedAttackSlash serratedSlashAttackClass;

	protected float serratedSlashCoolDownTimer = 0f;
	#endregion



	#region Global delay between all attacks variables
	[Header("Delay between all attacks")]
	[SerializeField]
	protected float globalAttackDelay = 0.5f;

	protected float globalAttackCoolDown = 0f;

	#endregion


	#region Shield break settings
	[Header("Shield break settings")]
	[SerializeField]
	protected float stunDuration = 3f;

	protected float stunTimer = 0f;

	[SerializeField]
	protected float speedWhileStunned = 0.5f;

	[SerializeField]
	protected float speedWhileShieldIsBroken = 6f;

	protected bool shieldIsBroken = false;

	#endregion



	#region Audio Events

	public event Action onSlamAttackStartSFXPlayOnce;
	public event Action onSlamHitGroundSFXPlayOnce;

	public event Action onSlashAttackSFXPlay;
	public event Action onSlashAttackSFXStop;

	#endregion


	protected DamageRingIndicator damageRingIndicator;

	/******************************************************************************/
	#region Functions
	#endregion



	#region Awake
	protected override void Awake()
	{
		slamTimer = slamAttackClass.timeWithinRadiusBeforeSlam;

		damageRingIndicator = GetComponent<DamageRingIndicator>();

		HealthWithShield healthWithShield = GetComponent<HealthWithShield>();

		healthWithShield.onShieldBreak += OnShieldBreak;
		healthWithShield.onShieldActivate += OnShieldActivate;

		base.Awake();
	}
	#endregion



	#region Update
	protected override void Update()
	{
		if (isSlamWindingUp && slamWindUpTimer > 0f)
		{
			slamWindUpTimer -= Time.deltaTime;
		}
		else if (isSlamWindingUp && slamWindUpTimer <= 0f)
		{

			isSlamWindingUp = false;
		}

		if (globalAttackCoolDown > 0) globalAttackCoolDown -= Time.deltaTime;
		if (slamAttackCoolDownTimer > 0) slamAttackCoolDownTimer -= Time.deltaTime;
		if (serratedSlashCoolDownTimer > 0) serratedSlashCoolDownTimer -= Time.deltaTime;

		base.Update();
	}
	#endregion



	#region Shield Events
	private void OnShieldBreak()
	{
		stunTimer = stunDuration;
		shieldIsBroken = true;
	}

	private void OnShieldActivate()
	{
		shieldIsBroken = false;

	}
	#endregion



	#region AlertedThinking
	/// <summary>
	/// How the tank AI thinks while alerted. Handles 2 attacks, light and special.
	/// </summary>
	protected override void AlertedThinking()
	{
		// speed setting
		if (stunTimer > 0)
		{
			currentSpeed = speedWhileStunned;
			stunTimer -= Time.deltaTime;
		}
		else if (Vector3.Distance(playerTarget.position, transform.position) < agent.radius + 0.1f || attacking) currentSpeed = 0.4f;
		else if (shieldIsBroken)
		{
			currentSpeed = speedWhileShieldIsBroken;
		}
		else currentSpeed = maxSpeed;

		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(pathTarget.x, transform.position.y, pathTarget.z) - transform.position, transform.up), maxTurningDegreesDelta);

		if (Vector3.Distance(playerTarget.position, transform.position) < lightAttackClass.minDistanceForAttack)
		{
			if (slamTimer > 0) slamTimer -= Time.deltaTime;

			if (attacking) return;

			if (globalAttackCoolDown > 0f) return;

			// we need to decide what attack to use.
			// Slam attack if the player stays too close or gets really close.
			if (serratedSlashCoolDownTimer <= 0f && Vector3.Distance(playerTarget.position, transform.position) < serratedSlashAttackClass.minimumDistanceForSerratedSlash)
			{
				StartCoroutine(SerratedSlashAttack());
				print(gameObject.name + " is attacking with serrated slash");
			}
			else if (slamAttackCoolDownTimer <= 0f && (Vector3.Distance(playerTarget.position, transform.position) < slamAttackClass.minimumDistanceForForceSlam || slamTimer <= 0f))
			{
				StartCoroutine(SlamAttack());
				print(gameObject.name + " is attacking with slam");
			}
			// light attack
			else if (lightAttackCoolDown <= 0f && globalAttackCoolDown <= 0f && slamTimer > 0f)
			{
				StartCoroutine(LightAttack());
				print(gameObject.name + " is attacking light");
			}
		}
		else
		{
			slamTimer = slamAttackClass.timeWithinRadiusBeforeSlam;

		}


		Vector3 lead = Vector3.Distance(playerTarget.position, transform.position) < 3.5f ? Vector3.zero : playerTarget.GetComponent<CharacterController>().velocity.normalized * 2f;

		pathTarget = playerTarget.position + lead;


	}
	#endregion



	#region SlamAttack
	/// <summary>
	/// Coroutine for dealing with the special attack, it just starts the animations and waits.
	/// </summary>
	/// <returns></returns>
	protected virtual IEnumerator SlamAttack()
	{
		attacking = true;

		attackAnimationPlaying = true;

		damageRingIndicator.ShowRing(slamAttackClass.slamWindUpTime, slamAttackClass.slamMaxRadius);

		animatorController.SetBool("IsCharging", true);
		animatorController.SetBool("IsSlamAttack", true);

		slamWindUpTimer = slamAttackClass.slamWindUpTime;
		isSlamWindingUp = true;

		SpecialAttackStartSFXPlayOnce();

		while (isSlamWindingUp) yield return null;

		animatorController.SetBool("IsCharging", false);




		while (attackAnimationPlaying) yield return null;

		slamAttackCoolDownTimer = slamAttackClass.slamAttackRateCoolDown;

		globalAttackCoolDown = globalAttackDelay;

		slamTimer = slamAttackClass.timeWithinRadiusBeforeSlam;


		attacking = false;

		currentSpeed = maxSpeed;

	}
	#endregion



	#region AnimationAttackFinished
	public override void AnimationAttackFinished()
	{
		animatorController.SetBool("IsCharging", false);
		animatorController.SetBool("IsSlamAttack", false);
		animatorController.SetBool("IsSlashAttack", false);
		animatorController.SetBool("IsAttacking", false);


		// this has a end termination.
		base.AnimationAttackFinished();
	}
	#endregion



	#region SlamAttackCheckAndDamage
	/// <summary>
	/// Creates a check sphere and deals the damage accordingly.
	/// </summary>
	protected virtual void SlamAttackCheckAndDamage()
	{
		SpecialHitGroundSFXPlayOnce();

		Collider[] HitObjects = Physics.OverlapSphere(transform.position, slamAttackClass.slamMaxRadius,
					layersToCheckFor, QueryTriggerInteraction.Ignore);

		if (HitObjects.Length > 0)
		{
			foreach (var hitObject in HitObjects)
			{
				if (hitObject.gameObject.CompareTag("Player"))
				{
					float distanceFromPlayer = Vector3.Distance(hitObject.transform.position, transform.position);

					float percentageDistanceWithinOuterRing = (distanceFromPlayer - slamAttackClass.slamMinRadius) / (slamAttackClass.slamMaxRadius - slamAttackClass.slamMinRadius);

					float calculatedDamage = Mathf.Lerp(slamAttackClass.slamAttackDamageAtMinRange, slamAttackClass.slamAttackDamageAtMaxRange, percentageDistanceWithinOuterRing);

					hitObject.GetComponent<IDamageable>()?.TakeDamage(calculatedDamage);
				}
			}
		}

		damageRingIndicator.HideRing();

	}

	#endregion


	#region DamageInRadius
	private void DamageInRadius(float radius, float damage)
	{
		Collider[] HitObjects = Physics.OverlapSphere(transform.position, radius,
					layersToCheckFor, QueryTriggerInteraction.Ignore);

		if (HitObjects.Length > 0)
		{
			foreach (var hitObject in HitObjects)
			{
				if (hitObject.gameObject.CompareTag("Player"))
				{
					float distanceFromPlayer = Vector3.Distance(hitObject.transform.position, transform.position);

					hitObject.GetComponent<IDamageable>()?.TakeDamage(damage);
				}
			}
		}
	}
	#endregion


	/*
	Activates if it's chosen at the beginning of the boss’ attack phase. 
	Performs an AoE slash attack around the boss, 
	doing tick-based damage (every 0.25s) while in the hurt box, 
	dealing high damage, up to 4 times.
	*/

	#region SerratedSlashAttack
	protected virtual IEnumerator SerratedSlashAttack()
	{
		attacking = true;

		attackAnimationPlaying = true;


		damageRingIndicator.ShowRing(serratedSlashAttackClass.serratedSlashWindUpTime, serratedSlashAttackClass.serratedSlashRadius);

		animatorController.SetBool("IsSlashAttack", true);
		animatorController.SetBool("IsCharging", true);

		yield return new WaitForSeconds(serratedSlashAttackClass.serratedSlashWindUpTime);

		animatorController.SetBool("IsCharging", false);


		float localTimer = 0f;
		while (localTimer < serratedSlashAttackClass.serratedSlashAttackDuration)
		{
			DamageInRadius(serratedSlashAttackClass.serratedSlashRadius, serratedSlashAttackClass.serratedSlashDamage);
			yield return new WaitForSeconds(serratedSlashAttackClass.serratedSlashTickLength);
			localTimer += serratedSlashAttackClass.serratedSlashTickLength;
		}

		animatorController.SetBool("IsSlashAttack", false);

		damageRingIndicator.HideRing();

		while (attackAnimationPlaying) yield return null;

		//AnimationAttackFinished();

		serratedSlashCoolDownTimer = serratedSlashAttackClass.serratedSlashCoolDown;

		globalAttackCoolDown = globalAttackDelay;

		attacking = false;

		currentSpeed = maxSpeed;

	}

	#endregion



	#region Animation State Updater
	// this is used by a script im between the animations and this so animations can call functions.

	/* normal attack */
	public override void EndAttack()
	{
		AnimationAttackFinished();
	}
	public override void DealAttack()
	{
		LightAttackCheckAndDamage();
	}


	/* Special attacks */
	public virtual void EndSpecialAttack()
	{
		AnimationAttackFinished();
	}

	public virtual void DealSlamAttack()
	{
		SlamAttackCheckAndDamage();
	}

	#endregion


	#region Event Invoke Functions
	/// <summary>
	/// Invokes the onSpecialAttackStartSFXPlayOnce event;
	/// </summary>
	protected virtual void SpecialAttackStartSFXPlayOnce()
	{
		onSlamAttackStartSFXPlayOnce?.Invoke();
	}

	/// <summary>
	/// Invokes the onSpecialHitGroundSFXPlayOnce event;
	/// </summary>
	protected virtual void SpecialHitGroundSFXPlayOnce()
	{
		onSlamHitGroundSFXPlayOnce?.Invoke();
	}
	#endregion
}
