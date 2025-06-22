using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|


[Serializable]
public class BeamAttackSettings
{
    public float coolDown = 5f;

    public float tickDamage = 5f;

    public float tickRate = 0.5f;

    public float windUpTime = 1f;

    public float attackDuration = 2f;

    public float beamRadius = 1f;

    public float turnSpeedWhileCharging = 1f;

    public LineRenderer lineRenderer;
}


public class AICommonBeam : AIBase
{
    public float maxBeamAttackRange = 7f;

    public BeamAttackSettings beamAttackSettings;


    public float minDistanceForPlayerToRetreat = 3f;


    public float retreatDuration = 5f;

    protected float retreatTimer = 0f;

    public float delayUntilRetreatAgain = 5f;

    protected float retreatCoolDown = 0f;

    public float defaultStoppingDistance = 0f;


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
            playerTarget = GameObject.FindWithTag("Player").transform;
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

        beamAttackSettings.lineRenderer.enabled = false;

        onStateChanged += ResetRetreatingThinking;
    }
    #endregion

    #region Update
    // Update is called once per frame
    protected virtual void Update()
    {
        if (UIManager.Instance.inDialogueMenu || UIManager.Instance.inGameMenu)
        {
            //agent.stoppingDistance = defaultStoppingDistance;
            pathTarget = transform.position;


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

        if (!isAttacking)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation((new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z) - transform.position).normalized, transform.up),
                turningSpeed);
        }

        if (Vector3.Distance(playerTarget.position, transform.position) < maxBeamAttackRange || isAttacking)
        {
            // attack
            pathTarget = transform.position;

            // begin attack.
            // coroutine
            if (!isAttacking && attackCoolDownTimer <= 0)
            {
                StartCoroutine(BeamAttack());
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
        pathTarget = fleeDirection * 10f;

        // if (Vector3.Distance(agent.pathEndPosition, transform.position) < 2f)
        // {
        //     ChangeState(AIState.Alerted);
        //     return;
        // }
    }
    #endregion

    #region BasicProjectileAttack
    protected virtual IEnumerator BeamAttack()
    {
        // we set the initial variables.
        isAttacking = true;
        attackAnimationPlaying = true;

        animatorController.SetBool("IsAttacking", true);
        animatorController.SetBool("IsCharging", true);

        // prep the line renderer. might be able to remove as this is also done whilst charging.
        var curve = new AnimationCurve();

        beamAttackSettings.lineRenderer.enabled = true;



        // while we are charging the attack
        float localTimer = 0;
        while (localTimer < beamAttackSettings.windUpTime)
        {
            beamAttackSettings.lineRenderer.startWidth = Mathf.Lerp(0, beamAttackSettings.beamRadius * 2f, localTimer / beamAttackSettings.windUpTime);

            Color colorLerp = Color.Lerp(Color.yellow, Color.red, localTimer / (beamAttackSettings.windUpTime + 1f));

            beamAttackSettings.lineRenderer.colorGradient = new Gradient()
            {
                colorKeys = new GradientColorKey[] { new GradientColorKey(colorLerp, 0), new GradientColorKey(colorLerp, 1) },
                alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) }
            };

            transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation((new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z) - transform.position).normalized, transform.up),
            beamAttackSettings.turnSpeedWhileCharging * Time.deltaTime);

            bool hitSomething = Physics.Raycast(transform.position, transform.forward, out RaycastHit hitReturn, 999f, LayerMask.GetMask("Default"));


            beamAttackSettings.lineRenderer.SetPosition(1, transform.InverseTransformPoint(hitSomething ? hitReturn.point : transform.position + transform.forward * 999f));

            localTimer += Time.deltaTime;
            yield return null;
        }

        // We stop charging and we now do that attack.

        animatorController.SetBool("IsCharging", false);



        // We see if we hit then environment so the beam does not go through the wall.
        bool hitSuccess = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 999f, LayerMask.GetMask("Default"));

        // we then set the line render.
        beamAttackSettings.lineRenderer.startWidth = beamAttackSettings.beamRadius * 2f;


        beamAttackSettings.lineRenderer.SetPosition(1, transform.InverseTransformPoint(hitSuccess ? hit.point : transform.position + transform.forward * 999f));
        beamAttackSettings.lineRenderer.colorGradient = new Gradient()
        {
            colorKeys = new GradientColorKey[] { new GradientColorKey(Color.red, 0), new GradientColorKey(Color.red, 1) },
            alphaKeys = new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) }
        };



        // deals the actual attack.
        localTimer = 0;
        while (localTimer < beamAttackSettings.attackDuration)
        {

            Collider[] colliders = Physics.OverlapCapsule(transform.position, (hitSuccess ? hit.point : transform.position + transform.forward * 999f),
                beamAttackSettings.beamRadius, LayerMask.GetMask("Player"), QueryTriggerInteraction.Collide);


            // print(colliders.Length);
            // yes, I know that the player is basically the only thing that can be in here.
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.CompareTag("Player"))
                {
                    // Using IDamageable when we have heal class default on everything now. Why?
                    collider.GetComponent<IDamageable>().TakeDamage(beamAttackSettings.tickDamage);
                    break;
                }
            }

            yield return new WaitForSeconds(beamAttackSettings.tickRate);
            localTimer += beamAttackSettings.tickRate;
        }



        // end attack

        beamAttackSettings.lineRenderer.enabled = false;



        animatorController.SetBool("IsAttacking", false);

        attackCoolDownTimer = beamAttackSettings.coolDown;

        while (attackAnimationPlaying) yield return null;

        isAttacking = false;
    }
    #endregion



    #region ResetRetreatingThinking
    #endregion

    protected virtual void ResetRetreatingThinking(AIState prevState, AIState newState)
    {
        if (newState == AIState.Alerted)
        {
            retreatTimer = 0f;
        }
    }



    #region Animation Functions

    public virtual void EndAttack()
    {
        animatorController.SetBool("IsAttacking", false);
        attackAnimationPlaying = false;
    }

    public virtual void DealAttack()
    {
        // SpawnProjectile();
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

        if (enableVisualDetectionLine && GameObject.FindWithTag("Player") != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (GameObject.FindWithTag("Player").transform.position - transform.position).normalized * maxDetectionRange);

        }
    }
    #endregion

}
