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
[RequireComponent(typeof(HealthWithShield))]
public class KnightAI : GruntAI
{
	#region Tank Slam Attack variables
	[Header("Tank Slam Attack")]


	protected float slamAttackCoolDownTimer = 0f;

	[SerializeField]
	protected float slamAttackRateCoolDown = 5f;

	#endregion



	#region Wind up settings variables
	[Header("Slam wind up settings")]
	[SerializeField]
	protected float slamWindUpTime = 1f;

	protected float slamWindUpTimer = 1f;

	protected bool isSlamWindingUp = false;

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
	protected float minimumDistanceForForceSlam = 5f;

	protected float slamTimer = 0f;

	[SerializeField]
	protected float timeWithinRadiusBeforeSlam = 3f;

	#endregion



	#region Serrated Slash
	[Header("Serrated Slash Settings")]

	// cool down
	[SerializeField]
	protected float serratedSlashCoolDown = 10f;

	protected float serratedSlashCoolDownTimer;

	// activation requirements
	[SerializeField]
	protected float minimumDistanceForSerratedSlash = 4f;

	// wind up
	[SerializeField]
	protected float serratedSlashWindUpTime = 0.5f;

	// damage and radius
	[SerializeField]
	protected float serratedSlashRadius = 5f;

	[SerializeField]
	protected float serratedSlashDamage = 2f;

	[SerializeField]
	protected float serratedSlashTickLength = 0.25f;

	[SerializeField]
	protected float serratedSlashAttackDuration = 1f;
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


	protected DamageRingIndicator damageRingIndicator;

	/******************************************************************************/
	#region Functions
	#endregion



	#region Awake
	protected override void Awake()
	{
		slamTimer = timeWithinRadiusBeforeSlam;

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
			animatorController.SetBool("IsCharging", false);
			isSlamWindingUp = false;
		}

		if (globalAttackCoolDown >= 0) globalAttackCoolDown -= Time.deltaTime;
		if (slamAttackCoolDownTimer > 0) slamAttackCoolDownTimer -= Time.deltaTime;

		base.Update();
	}
	#endregion



	private void OnShieldBreak()
	{
		throw new NotImplementedException();
	}

	private void OnShieldActivate()
	{
		throw new NotImplementedException();
	}



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

			if (attacking) return;

			if (globalAttackCoolDown > 0f) return;

			// we need to decide what attack to use.
			// Slam attack if the player stays too close or gets really close.
			if (serratedSlashCoolDownTimer <= 0f && Vector3.Distance(playerTarget.position, transform.position) < minimumDistanceForSerratedSlash)
			{
				StartCoroutine(SerratedSlashAttack());
				print(gameObject.name + " is attacking with serrated slash");
			}
			else if (slamAttackCoolDownTimer <= 0f && (Vector3.Distance(playerTarget.position, transform.position) < minimumDistanceForForceSlam || slamTimer <= 0f))
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
			slamTimer = timeWithinRadiusBeforeSlam;

		}

		pathTarget = playerTarget.position;
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

		damageRingIndicator.ShowRing(slamWindUpTime, slamMaxRadius);

		animatorController.SetBool("IsCharging", true);
		animatorController.SetBool("IsAttacking", true);

		slamWindUpTimer = slamWindUpTime;
		isSlamWindingUp = true;

		SpecialAttackStartSFXPlayOnce();

		while (isSlamWindingUp) yield return null;


		animatorController.SetBool("IsSpecialAttack", true);

		slamAttackCoolDownTimer = slamAttackRateCoolDown;


		while (attackAnimationPlaying) yield return null;

		slamTimer = timeWithinRadiusBeforeSlam;


		attacking = false;

		currentSpeed = maxSpeed;

	}
	#endregion



	#region AnimationAttackFinished
	public override void AnimationAttackFinished()
	{
		animatorController.SetBool("IsCharging", false);
		animatorController.SetBool("IsSpecialAttack", false);

		damageRingIndicator.HideRing();
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
		damageRingIndicator.ShowRing(serratedSlashWindUpTime, serratedSlashRadius);
		yield return new WaitForSeconds(serratedSlashWindUpTime);


		float localTimer = 0f;
		while (localTimer < serratedSlashAttackDuration)
		{
			DamageInRadius(serratedSlashRadius, serratedSlashDamage);
			yield return new WaitForSeconds(serratedSlashTickLength);
			localTimer += serratedSlashTickLength;
		}

		damageRingIndicator.HideRing();

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


	/* special attack */
	public virtual void EndSpecialAttack()
	{
		AnimationAttackFinished();
	}

	public virtual void DealSpecialAttack()
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
