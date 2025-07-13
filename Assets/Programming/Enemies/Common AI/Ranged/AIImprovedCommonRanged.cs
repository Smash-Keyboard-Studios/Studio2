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

[Serializable]
public class ProjectileAttackSettings
{
    public float maxAttackRange = 7f;

    public float attackCoolDown = 1f;

    public float projectileSpeed = 10f;

    public float projectileDamage = 5f;

    public GameObject projectilePrefab;
}


/// <summary>
/// Replacement for the current common ranged AI.
/// </summary>
public class AIImprovedCommonRanged : AIBase
{
    [SerializeField]
    protected ProjectileAttackSettings projectileAttackSettings;


    public float minDistanceForPlayerToRetreat = 3f;


    public float retreatDuration = 5f;

    protected float retreatTimer = 0f;

    public float delayUntilRetreatAgain = 5f;

    protected float retreatCoolDown = 0f;

    // public float defaultStoppingDistance = 0f;


    protected bool isAttacking = false;

    protected float attackCoolDownTimer = 0f;


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
    // protected NavMeshPath path;

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
    protected float turningSpeed = 5f;
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

    #region Start
    protected override void Start()
    {

        base.Start();

        onStateChanged += ResetRetreatingThinking;
    }
    #endregion

    #region Update
    // Update is called once per frame
    protected virtual void Update()
    {
        agent.destination = pathTarget;

        if (UIManager.Instance.inDialogueMenu || UIManager.Instance.inGameMenu)
        {
            //agent.stoppingDistance = defaultStoppingDistance;
            pathTarget = transform.position;
            // agent.destination = pathTarget;


            animatorController.enabled = false;


            return;
        }
        else
        {
            animatorController.enabled = true;
        }



        // set values and deal with timers.
        agent.speed = currentSpeed;


        if (retreatCoolDown > 0) retreatCoolDown -= Time.deltaTime;
        if (attackCoolDownTimer > 0) attackCoolDownTimer -= Time.deltaTime;

        if (currentAIState == AIState.Alerted)
        {
            AlertedThinking();
        }
        else if (currentAIState == AIState.Retreating)
        {
            RetreatingThinking();
        }
        else if (currentAIState == AIState.Idle)
        {
            IdleThinking();
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
        //agent.stoppingDistance = defaultStoppingDistance;
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



        if (Vector3.Distance(playerTarget.position, transform.position) < minDistanceForPlayerToRetreat && retreatCoolDown <= 0f && !isAttacking)
        {
            ChangeState(AIState.Retreating);
            return;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation((new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z) - transform.position).normalized, transform.up),
            turningSpeed);

        if (Vector3.Distance(playerTarget.position, transform.position) < projectileAttackSettings.maxAttackRange || isAttacking)
        {
            // attack
            pathTarget = transform.position;

            // begin attack.
            // coroutine
            if (!isAttacking && attackCoolDownTimer <= 0)
            {
                StartCoroutine(BasicProjectileAttack());
            }

            return;
        }
        else
        {
            pathTarget = playerTarget.position;

        }



    }
    #endregion


    #region RetreatingThinking
    /// <summary>
    /// How the AI acts when it want to retreat from the player.
    /// </summary>
    protected virtual void RetreatingThinking()
    {
        retreatTimer += Time.deltaTime;

        if (retreatTimer >= retreatDuration)
        {
            retreatCoolDown = delayUntilRetreatAgain;
            ChangeState(AIState.Alerted);
            return;
        }

        Vector3 fleeDirection = (transform.position - playerTarget.position).normalized;

        //agent.stoppingDistance = defaultStoppingDistance;
        pathTarget = fleeDirection * 10f; // TODO remove magic number, what does 10 mean? magic number.

        // if (Vector3.Distance(agent.pathEndPosition, transform.position) < 2f)
        // {
        //     ChangeState(AIState.Alerted);
        //     return;
        // }
    }
    #endregion

    #region BasicProjectileAttack
    protected virtual IEnumerator BasicProjectileAttack()
    {
        isAttacking = true;
        animatorController.SetBool("IsAttacking", true);
        attackAnimationPlaying = true;



        attackCoolDownTimer = projectileAttackSettings.attackCoolDown;

        while (attackAnimationPlaying) yield return null;

        isAttacking = false;
    }
    #endregion

    #region SpawnProjectile

    protected virtual void SpawnProjectile()
    {
        Vector3 targetVel = (playerTarget.position - transform.position + transform.forward).normalized * projectileAttackSettings.projectileSpeed;


        GameObject projectile = Instantiate(projectileAttackSettings.projectilePrefab, transform.position + transform.forward, Quaternion.identity);

        projectile.GetComponent<RangedProjectilePhysicsBased>().SetUpProjectile(targetVel, projectileAttackSettings.projectileDamage);
    }
    #endregion

    #region ResetRetreatingThinking

    protected virtual void ResetRetreatingThinking(AIState prevState, AIState newState)
    {
        if (newState == AIState.Alerted)
        {
            retreatTimer = 0f;
        }
    }
    #endregion



    #region Animation Functions

    public virtual void EndAttack()
    {
        animatorController.SetBool("IsAttacking", false);
        attackAnimationPlaying = false;

    }

    public virtual void DealAttack()
    {
        SpawnProjectile();
    }

    #endregion



    #region OnDrawGizmos
    protected virtual void OnDrawGizmos()
    {
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
}
