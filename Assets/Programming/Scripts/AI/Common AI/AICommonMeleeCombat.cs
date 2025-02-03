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
public class AICommonMeleeCombat : AIBase
{
	[Header("Attacking")]
	[SerializeField]
	protected float Damage = 10f;
	/// <summary>
	/// How long before next attack.
	/// </summary>
	[Tooltip("How long before next attack.")]
	[SerializeField]
	protected float AttackRate = 1f;

	protected NavMeshPath Path;

	protected Vector3 PathTarget;

	protected Transform PlayerTarget;

	[SerializeField]
	protected float MinDistanceForAttack = 2f;

	[Header("Box check for attacking")]
	// box casst
	[SerializeField]
	protected float BoxCastThickness = 2f;

	[SerializeField]
	protected float BoxCastLength = 3;

	[SerializeField]
	protected float BoxCastHeight = 1;

	[SerializeField]
	protected Vector3 BoxCastOffsetFromAI = Vector3.forward;

	[SerializeField]
	LayerMask LayersToCheckFor = Physics.AllLayers;

	[SerializeField]
	protected float AttackDamage = 20f;

	protected float AttackCooldown = 0f;

	protected bool Attacking = false;

	protected override void Awake()
	{
		base.Awake();

		Path = new NavMeshPath();

		PlayerTarget = GameObject.FindWithTag("Player").transform;



	}

	protected override void Start()
	{
		base.Start();

		InvokeRepeating(nameof(RunPathfinding), 0, 0.25f);


	}

	protected override void Update()
	{
		base.Update();

		Agent.speed = CurrentSpeed;

		if (AttackCooldown > 0f) AttackCooldown -= Time.deltaTime;

		if (Vector3.Distance(PlayerTarget.position, transform.position) < MinDistanceForAttack)
		{
			// attack
			if (Vector3.Distance(PlayerTarget.position, transform.position) < 1.55f || Attacking) CurrentSpeed = 0.4f; // PathTarget = transform.position;
			else CurrentSpeed = MaxSpeed; // PathTarget = PlayerTarget.position;


			if (!Attacking && AttackCooldown <= 0f) StartCoroutine(Attack());
		}

		PathTarget = PlayerTarget.position;
	}

	protected IEnumerator Attack()
	{
		Attacking = true;
		AttackCooldown = AttackRate;

		Collider[] HitObjects = Physics.OverlapBox(transform.position + BoxCastOffsetFromAI, new Vector3(BoxCastLength, BoxCastHeight, BoxCastThickness) / 2f,
		 transform.rotation, LayersToCheckFor, QueryTriggerInteraction.Ignore);

		if (HitObjects.Length > 0)
		{
			foreach (var hitObject in HitObjects)
			{
				if (hitObject.gameObject.CompareTag("Player"))
				{
					hitObject.GetComponent<IDamagable>()?.TakeDamage(AttackDamage);
				}
			}
		}
		yield return null;

		Attacking = false;
	}


	protected virtual void RunPathfinding()
	{
		if (NavMesh.CalculatePath(transform.position, PathTarget, NavMesh.AllAreas, Path))
			Agent.path = Path;

		// print("NAVING");
	}
}
