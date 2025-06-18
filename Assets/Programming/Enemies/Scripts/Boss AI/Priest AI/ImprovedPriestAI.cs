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



public class ImprovedPriestAI : AIBase
{


    #region Light Attack Variables
    [Header("Light Attack Settings"), SerializeField]
    protected LightAttack lightAttackClass;

    /// <summary>
    /// Remaining time left before the AI can attack again.
    /// </summary>
    protected float lightAttackCoolDown = 0f;

    #endregion

    // Global attack variables

    /// <summary>
    /// Weather the AI is currently attacking the player. Used by the IEnumerator.
    /// </summary>
    protected bool attacking = false;

    protected float globalAttackCoolDown = 0f;

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


    #region Update
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
        /*
        AI checklist
        Melee attack for when all attacks are on cool down.
        Beam attack with 4 cannons. 2 each side. fire in sequence. separate firing class with Fire function.
        Barrage attack.
        Warp away to keep distance.
        */



    }
    #endregion


    #region RetreatingThinking
    /// <summary>
    /// How the AI acts when it want to retreat from the player.
    /// </summary>
    protected virtual void RetreatingThinking()
    {

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


        // we start the attack.
        animatorController.SetBool("IsMeleeAttacking", true);

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
        animatorController.SetBool("IsMeleeAttacking", false);
        attackAnimationPlaying = false;

    }
    #endregion

    #region LightAttackCheckAndDamage
    /// <summary>
    /// Creates a box cast and deals damage to the player if there is one in the box cast.
    /// </summary>
    public virtual void MeleeAttackCheckAndDamage()
    {
        Collider[] HitObjects = Physics.OverlapBox(transform.position + (transform.forward * lightAttackClass.boxCastForwardOffset), new Vector3(lightAttackClass.boxCastLength, lightAttackClass.boxCastHeight, lightAttackClass.boxCastDepth) / 2f,
                 transform.rotation, layersToCheckFor);


        //AttackSFXPlayOnce(animatorController.GetBool("IsHardAttack"));

        if (HitObjects.Length > 0)
        {
            foreach (var hitObject in HitObjects)
            {
                //print(hitObject.name);
                if (hitObject.gameObject.CompareTag("Player"))
                {
                    hitObject.GetComponent<IDamageable>()?.TakeDamage(lightAttackClass.lightAttackDamage);
                }
            }
        }
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
