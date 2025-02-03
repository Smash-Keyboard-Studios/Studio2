using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public class AICommonRangedCombat : AIBase
{
    [SerializeField] protected float AttackRate = 1f;
    [SerializeField] protected float Damage = 10f;
    protected NavMeshPath Path;
    protected Vector3 PathTarget;
    protected Transform PlayerTarget;
    [SerializeField] protected float AttackDamage = 20f;
    [SerializeField] protected float AttackCooldown = 0f;

    protected bool Attacking = false;
    [SerializeField] protected float MinDistanceForAttack = 2f;
    [Header("Projectiles")]
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private Transform[] ProjectileSpawnPoint;
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

        if (AttackCooldown > 0f) { AttackCooldown -= Time.deltaTime; }

        if (Vector3.Distance(PlayerTarget.position, transform.position) < MinDistanceForAttack)
        {
            // attack
            if (Vector3.Distance(PlayerTarget.position, transform.position) < MinDistanceForAttack || Attacking) CurrentSpeed = -0.1f; // PathTarget = transform.position;
            else CurrentSpeed = MaxSpeed; // PathTarget = PlayerTarget.position;


            if (!Attacking && AttackCooldown <= 0f) StartCoroutine(Attack());
        }

        PathTarget = PlayerTarget.position;
    }
    protected IEnumerator Attack()
    {
        Attacking = true;
        AttackCooldown = AttackRate;

        foreach(Transform SpawnPoint in ProjectileSpawnPoint)
		{
            Instantiate(ProjectilePrefab, SpawnPoint.position, transform.rotation);
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
