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
	protected float damage = 10f;
	/// <summary>
	/// How long before next attack.
	/// </summary>
	[Tooltip("How long before next attack.")]
	[SerializeField]
	protected float attackRate = 1f;

	protected NavMeshPath path;

	protected Vector3 pathTarget;

	protected Transform playerTarget;

	[SerializeField]
	protected float minDistanceForAttack = 2f;

	[Header("Box check for attacking")]
	// box casst
	[SerializeField]
	protected float boxCastThickness = 2f;

	[SerializeField]
	protected float boxCastLength = 3;

	[SerializeField]
	protected float boxCastHeight = 1;

	[SerializeField]
	protected Vector3 boxCastOffsetFromAI = Vector3.forward;

	[SerializeField]
	LayerMask layersToCheckFor = Physics.AllLayers;

	[SerializeField]
	protected float attackDamage = 20f;

	protected float attackCooldown = 0f;

	protected bool attacking = false;

	protected override void Awake()
	{
		base.Awake();

		path = new NavMeshPath();

		playerTarget = GameObject.FindWithTag("Player").transform;



	}

	protected override void Start()
	{
		base.Start();

		InvokeRepeating(nameof(RunPathfinding), 0, 0.25f);


	}

	protected override void Update()
	{
		base.Update();

		agent.speed = currentSpeed;

		if (attackCooldown > 0f) attackCooldown -= Time.deltaTime;

		if (Vector3.Distance(playerTarget.position, transform.position) < minDistanceForAttack)
		{
			// attack
			if (Vector3.Distance(playerTarget.position, transform.position) < 1.55f || attacking) currentSpeed = 0.4f; // PathTarget = transform.position;
			else currentSpeed = maxSpeed; // PathTarget = PlayerTarget.position;


			if (!attacking && attackCooldown <= 0f) StartCoroutine(Attack());
		}

		pathTarget = playerTarget.position;
	}

	protected IEnumerator Attack()
	{
		attacking = true;
		attackCooldown = attackRate;

		Collider[] HitObjects = Physics.OverlapBox(transform.position + boxCastOffsetFromAI, new Vector3(boxCastLength, boxCastHeight, boxCastThickness) / 2f,
		 transform.rotation, layersToCheckFor, QueryTriggerInteraction.Ignore);

		if (HitObjects.Length > 0)
		{
			foreach (var hitObject in HitObjects)
			{
				if (hitObject.gameObject.CompareTag("Player"))
				{
					hitObject.GetComponent<IDamagable>()?.TakeDamage(attackDamage);
				}
			}
		}
		yield return null;

		attacking = false;
	}


	protected virtual void RunPathfinding()
	{
		if (NavMesh.CalculatePath(transform.position, pathTarget, NavMesh.AllAreas, path))
			agent.path = path;

		// print("NAVING");
	}
}
