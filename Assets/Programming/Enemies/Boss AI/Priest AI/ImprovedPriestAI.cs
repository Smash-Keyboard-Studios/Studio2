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
// © 2025 Dominic McNeill dommcneill@outlook.com

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

public class ImprovedPriestAI : AICommonBeam
{

    #region Light Attack Variables
    [Header("Attack Settings")]
    public LightAttack lightAttackSettings;

    /// <summary>
    /// Remaining time left before the AI can attack again.
    /// </summary>
    protected float lightAttackCoolDown = 0f;

    #endregion



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



    [Header("Teleport Settings")]

    public float timePlayerCloseBeforeRetreating = 5f;

    protected float playerCloseTimer = 0f;

    public float distanceTooCloseToPlayer = 2f;




    public Transform[] warpPoints;

    protected bool isTeleporting = false;

    protected bool isAnimationPlaying = false;

    protected Transform teleportPoint;


    /// <summary>
    /// The layers it will look for to deal damage to.
    /// </summary>
    [Header("Combat Detection Layer"), SerializeField]
    protected LayerMask layersToCheckFor = Physics.AllLayers;


    [Header("Debug Only"), SerializeField]
    protected bool showMelee = false;





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

        beamAttack.SetBeamActive(false);


        // onStateChanged += ResetRetreatingThinking;
    }
    #endregion



    #region Update
    protected override void Update()
    {
        base.Update();

        // print(GetAngleForFireProjectile(5, Mathf.Abs(Physics.gravity.y), Vector3.Distance(transform.position, playerTarget.position)));

        if (barrageAttackCoolDownTimer > 0) barrageAttackCoolDownTimer -= Time.deltaTime;

        if (globalAttackCoolDown > 0) globalAttackCoolDown -= Time.deltaTime;
    }
    #endregion



    #region AlertedThinking
    /// <summary>
    /// How the AI acts when it seen / detects the player.
    /// </summary>
    protected override void AlertedThinking()
    {
        /*
        AI checklist
        Melee attack for when all attacks are on cool down.
        Beam attack with 4 cannons. 2 each side. fire in sequence. separate firing class with Fire function. // Lol no, maybe later or never.
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
    protected override void RetreatingThinking()
    {
        if (warpPoints.Length <= 0)
        {
            Debug.LogError($"You need teleport points in the level and assign it to the priest in {nameof(warpPoints)}!");
            ChangeState(AIState.Alerted);
            return;
        }

        if (!isTeleporting)
        {
            StartCoroutine(StartTeleport());
        }
    }
    #endregion

    protected virtual IEnumerator StartTeleport()
    {
        isTeleporting = true;
        isAnimationPlaying = true;
        pathTarget = transform.position;


        teleportPoint = GetRandomFurthestTeleportPoint();


        if (teleportPoint == null)
        {
            Debug.LogError($"Failed to find a valid teleport point, make sure there are no null values in the {nameof(warpPoints)}!");
            isTeleporting = false;

        }
        else
        {
            teleportPoint.GetComponent<BossTeleportingEvent>()?.StartTeleport();


            animatorController.SetBool("IsTeleporting", true);

            while (isAnimationPlaying) yield return null;

            teleportPoint = null;

            ChangeState(AIState.Alerted);

            isTeleporting = false;
        }
    }

    protected Transform GetRandomFurthestTeleportPoint()
    {
        List<Transform> possiblePoint = new List<Transform>();

        foreach (var point in warpPoints)
        {
            if (point == null) continue;

            if (Vector3.Distance(playerTarget.position, point.position) < 10f) continue; // TODO figure out why 10f was chosen

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
                if (hitObject.gameObject.CompareTag(Constants.PlayerTag))
                {
                    hitObject.GetComponent<IDamageable>()?.TakeDamage(lightAttackSettings.lightAttackDamage);
                }
            }
        }
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


        for (int i = 0; i < barrageAttackSettings.projectileCount; i++) // TODO WTF is this!!!, holy shit! redo for readability.
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
    protected override void OnDrawGizmos()
    {
        if (showMelee)
        {
            Gizmos.DrawWireCube(transform.position + transform.forward * lightAttackSettings.boxCastForwardOffset, new Vector3(lightAttackSettings.boxCastLength, lightAttackSettings.boxCastHeight, lightAttackSettings.boxCastDepth));
        }

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
