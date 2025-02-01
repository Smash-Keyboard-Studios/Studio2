using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
	protected float AttackRate = 10;

	protected NavMeshPath path;

	protected override void Start()
	{
		base.Start();

		path = new NavMeshPath();

		InvokeRepeating(nameof(RunPathfinding), 0, 0.25f);
	}

	protected override void Update()
	{
		base.Update();


	}

	protected virtual void RunPathfinding()
	{
		if (NavMesh.CalculatePath(transform.position, GameObject.FindWithTag("Player").transform.position, NavMesh.AllAreas, path))
			Agent.path = path;
		// print("NAVING");
	}
}
