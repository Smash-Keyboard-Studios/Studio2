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
public class BarrageAttackSettings
{
    public Transform spawnPoint;

    public float forceForProjectile = 15f;

    public GameObject projectilePrefab;

    public float attackCoolDown = 5f;

    public int projectileCount = 6;

    public float verticalOffset = 10f;

    public float horizontalDisplacement = 10f;

    public float verticalDisplacement = 10f;

    public float delayPerShot = 1f;

    public float windUpTime = 2f;
}

public class ImprovedPriestAI : AIBase
{

    public float maxAttackRange = 30f;

    #region Light Attack Variables
    [Header("Light Attack Settings")]
    public LightAttack lightAttackSettings;

    /// <summary>
    /// Remaining time left before the AI can attack again.
    /// </summary>
    protected float lightAttackCoolDown = 0f;

    #endregion

    public BeamAttackSettings beamAttackSettings;


    protected float beamAttackCoolDownTimer = 0;

    public BarrageAttackSettings barrageAttackSettings;

    protected float barrageWindUpTimer = 0f;

    protected float barrageAttackCoolDownTimer = 0f; // no class

    // Global attack variables

    /// <summary>
    /// Weather the AI is currently attacking the player. Used by the IEnumerator.
    /// </summary>
    protected bool attacking = false;

    public float globalAttackDelay = 0.5f;

    protected float globalAttackCoolDown = 0f;

    protected bool isAttacking = false;




    public float timePlayerCloseBeforeRetreating = 5f;

    protected float playerCloseTimer = 0f;

    public float distanceTooCloseToPlayer = 2f;




    public Transform[] warpPoints;

    protected bool isTeleporting = false;

    protected bool isAnimationPlaying = false;

    protected Transform teleportPoint;


    #region Turning Variables 
    [Header("Turning Variables and movement"), SerializeField]
    protected float turningSpeed = 5f;
    #endregion

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

    #region Start
    protected override void Start()
    {

        base.Start();

        beamAttackSettings.lineRenderer.enabled = false;

        // onStateChanged += ResetRetreatingThinking;
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

        // print(GetAngleForFireProjectile(5, Mathf.Abs(Physics.gravity.y), Vector3.Distance(transform.position, playerTarget.position)));

        if (barrageAttackCoolDownTimer > 0) barrageAttackCoolDownTimer -= Time.deltaTime;

        if (globalAttackCoolDown > 0) globalAttackCoolDown -= Time.deltaTime;


        agent.destination = pathTarget;

        if (beamAttackCoolDownTimer > 0) beamAttackCoolDownTimer -= Time.deltaTime;

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

        if (!isAttacking)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation((new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z) - transform.position).normalized, transform.up),
                turningSpeed * Time.deltaTime);
        }

        float playerDistance = Vector3.Distance(playerTarget.position, transform.position);

        if (playerDistance < distanceTooCloseToPlayer)
        {
            if (playerCloseTimer < timePlayerCloseBeforeRetreating)
            {
                playerCloseTimer += Time.deltaTime;
            }
            else if (playerCloseTimer >= timePlayerCloseBeforeRetreating && !isAttacking)
            {
                ChangeState(AIState.Retreating);
            }
        }
        else
        {
            if (playerCloseTimer > 0) playerCloseTimer -= Time.deltaTime;
        }

        if (playerDistance < maxAttackRange && !isAttacking)
        {
            pathTarget = transform.position;

            // range attackable
            if (playerDistance < maxAttackRange && playerDistance > lightAttackSettings.minDistanceForAttack)
            {
                int attackType = UnityEngine.Random.Range(1, 2);
                if ((attackType == 1 && barrageAttackCoolDownTimer <= 0) || (attackType == 2 && beamAttackCoolDownTimer > 0))
                {
                    StartCoroutine(BarrageAttack());
                }
                else if ((attackType == 2 && beamAttackCoolDownTimer <= 0) || (attackType == 1 && barrageAttackCoolDownTimer > 0))
                {
                    StartCoroutine(BeamAttack());
                }
            }
            else if (playerDistance < lightAttackSettings.minDistanceForAttack)
            {
                StartCoroutine(LightAttack());

            }

            return;
        }
        else if (playerDistance < maxAttackRange && isAttacking)
        {
            pathTarget = transform.position;

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
        if (!isTeleporting)
            StartCoroutine(StartTeleport());
    }
    #endregion

    protected virtual IEnumerator StartTeleport()
    {
        isTeleporting = true;
        isAnimationPlaying = true;
        pathTarget = transform.position;

        while (teleportPoint == null)
        {
            teleportPoint = GetRandomFurthestTeleportPoint();
            yield return new WaitForSeconds(1f);
        }

        teleportPoint.GetComponent<BossTeleportingEvent>()?.StartTeleport();


        animatorController.SetBool("IsTeleporting", true);

        while (isAnimationPlaying) yield return null;

        teleportPoint = null;

        ChangeState(AIState.Alerted);

        isTeleporting = false;
    }

    protected Transform GetRandomFurthestTeleportPoint()
    {
        List<Transform> possiblePoint = new List<Transform>();

        foreach (var point in warpPoints)
        {
            if (Vector3.Distance(playerTarget.position, point.position) < 10f) continue;

            possiblePoint.Add(point);
        }

        if (possiblePoint.Count > 0)
            return possiblePoint[UnityEngine.Random.Range(0, possiblePoint.Count - 1)];
        else return null;
    }

    public virtual void Teleport()
    {
        teleportPoint.GetComponent<BossTeleportingEvent>()?.EndTeleport();
        transform.position = teleportPoint.position;
        pathTarget = transform.position;
    }

    public virtual void EndTeleportAnimation()
    {
        animatorController.SetBool("IsTeleporting", false);
        isAnimationPlaying = false;
    }

    #region LightAttack
    /// <summary>
    /// Dealing with attacking the player and dealing damage.
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator LightAttack()
    {
        attacking = true;

        attackAnimationPlaying = true;


        // we start the attack.
        animatorController.SetBool("IsMeleeAttacking", true);

        // we wait for the animation to finish.
        lightAttackCoolDown = lightAttackSettings.lightAttackRate;

        globalAttackCoolDown = globalAttackDelay;

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
        animatorController.SetBool("IsBeamAttacking", false);
        animatorController.SetBool("IsMeleeAttacking", false);
        animatorController.SetBool("IsCharging", false);

        attackAnimationPlaying = false;

    }
    #endregion

    #region LightAttackCheckAndDamage
    /// <summary>
    /// Creates a box cast and deals damage to the player if there is one in the box cast.
    /// </summary>
    public virtual void MeleeAttackCheckAndDamage()
    {
        Collider[] HitObjects = Physics.OverlapBox(transform.position + (transform.forward * lightAttackSettings.boxCastForwardOffset),
            new Vector3(lightAttackSettings.boxCastLength, lightAttackSettings.boxCastHeight, lightAttackSettings.boxCastDepth) / 2f,
            transform.rotation, layersToCheckFor);


        if (HitObjects.Length > 0)
        {
            foreach (var hitObject in HitObjects)
            {
                //print(hitObject.name);
                if (hitObject.gameObject.CompareTag("Player"))
                {
                    hitObject.GetComponent<IDamageable>()?.TakeDamage(lightAttackSettings.lightAttackDamage);
                }
            }
        }
    }
    #endregion



    #region BeamAttack
    protected virtual IEnumerator BeamAttack()
    {
        // we set the initial variables.
        isAttacking = true;
        attackAnimationPlaying = true;

        animatorController.SetBool("IsBeamAttacking", true);
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


            beamAttackSettings.lineRenderer.SetPosition(1,
                transform.InverseTransformPoint(hitSomething ? hitReturn.point - (-transform.forward.normalized * beamAttackSettings.beamRadius)
                : transform.position + transform.forward * 999f));

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

            Collider[] colliders = Physics.OverlapCapsule(beamAttackSettings.lineRenderer.transform.position,
                (hitSuccess ? hit.point - ((-transform.forward) * beamAttackSettings.beamRadius) : transform.position + transform.forward * 999f),
                beamAttackSettings.beamRadius, LayerMask.GetMask("Player"),
                QueryTriggerInteraction.Collide);


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

        animatorController.SetBool("IsBeamAttacking", false);

        beamAttackCoolDownTimer = beamAttackSettings.coolDown;

        globalAttackCoolDown = globalAttackDelay;

        while (attackAnimationPlaying) yield return null;

        isAttacking = false;
    }
    #endregion

    #region BarrageAttack
    protected IEnumerator BarrageAttack()
    {
        isAttacking = true;
        attackAnimationPlaying = true;

        // animatorController.SetBool("IsBeamAttacking", false);

        animatorController.SetBool("IsBeamAttacking", true);
        animatorController.SetBool("IsCharging", true);

        while (barrageWindUpTimer < barrageAttackSettings.windUpTime)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation((new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z) - transform.position).normalized, transform.up),
                turningSpeed * Time.deltaTime);

            barrageWindUpTimer += Time.deltaTime;
            yield return null;
        }

        animatorController.SetBool("IsCharging", false);

        barrageAttackSettings.spawnPoint.rotation = Quaternion.LookRotation((playerTarget.position + Vector3.down) - transform.position);
        // player target - up is so we can hit the floor, this is a AOE attack not a direct attack.
        if (Vector3.Distance(playerTarget.position, transform.position) > 10f) // Dont know if we need this as the arc might hit directly now.
            barrageAttackSettings.spawnPoint.localRotation = Quaternion.Euler(GetAngleForFireProjectile(barrageAttackSettings.spawnPoint.position,
                playerTarget.position + Vector3.down, barrageAttackSettings.forceForProjectile), 0, 0);

        Quaternion baseRotation = barrageAttackSettings.spawnPoint.rotation;


        for (int i = 0; i < barrageAttackSettings.projectileCount; i++)
        {
            barrageAttackSettings.spawnPoint.rotation = baseRotation
                * Quaternion.AngleAxis(barrageAttackSettings.verticalOffset, barrageAttackSettings.spawnPoint.right)
                * Quaternion.AngleAxis(UnityEngine.Random.Range(-barrageAttackSettings.horizontalDisplacement, barrageAttackSettings.horizontalDisplacement),
                    barrageAttackSettings.spawnPoint.up)
                * Quaternion.AngleAxis(UnityEngine.Random.Range(-barrageAttackSettings.verticalDisplacement, barrageAttackSettings.verticalDisplacement),
                    barrageAttackSettings.spawnPoint.right);


            GameObject go = Instantiate(barrageAttackSettings.projectilePrefab, barrageAttackSettings.spawnPoint.position, barrageAttackSettings.spawnPoint.rotation);
            go.GetComponent<Rigidbody>().AddForce(go.transform.forward * barrageAttackSettings.forceForProjectile, ForceMode.Impulse);
            yield return new WaitForSeconds(barrageAttackSettings.delayPerShot);
        }


        animatorController.SetBool("IsBeamAttacking", false);
        barrageAttackCoolDownTimer = barrageAttackSettings.attackCoolDown;

        globalAttackCoolDown = globalAttackDelay;

        barrageWindUpTimer = 0f;


        while (attackAnimationPlaying) yield return null;

        isAttacking = false;
    }
    #endregion

    #region GetAngleForFireProjectile
    protected float GetAngleForFireProjectile(Vector3 startPos, Vector3 targetPos, float force)
    {
        float gravity = Mathf.Abs(Physics.gravity.y);

        float distance = Vector2.Distance(new Vector2(startPos.x, startPos.z), new Vector2(targetPos.x, targetPos.z));
        float heightOffset = startPos.y - targetPos.y;
        //subtract to get direct curve and adding is high curve.
        //A = arctan((v^2 ± SQRT(v^4 - g(gx^2 + 2yv^2)))/gx)

        //float angle01 = Mathf.Atan((Mathf.Pow(force, 2) + Mathf.Sqrt(Mathf.Pow(force, 4) - (Physics.gravity.y * (Physics.gravity.y * Mathf.Pow(distance, 2) + (2 * heightOffset) * Mathf.Pow(force, 2))))) / (Physics.gravity.y * distance));

        //print("Angle 01 : " + angle01);

        float angle02 = Mathf.Atan((Mathf.Pow(force, 2) - Mathf.Sqrt(Mathf.Pow(force, 4) - (Physics.gravity.y * (Physics.gravity.y * Mathf.Pow(distance, 2) + (2 * heightOffset) * Mathf.Pow(force, 2))))) / (Physics.gravity.y * distance));

        //print("Angle 02 : " + angle02);

        //float angleResult = Mathf.Min(angle01, angle02);
        float angleResult = angle02;

        //print("Angle : " + angleResult * Mathf.Rad2Deg);
        return angleResult * Mathf.Rad2Deg;
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
