using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestBeam : BaseEnemyBeam
{
	[SerializeField] public Transform playerObject;
	[SerializeField] public float turnSpeed;
	private Vector3 boxEndPoint;
	float width;
	protected float distanceToPlayer;
	protected float adjustedTurnSpeed;
	public float maxAggro;

	protected override void Awake()

	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.material = beamMaterialStart;
		try
		{
			playerObject = GameObject.FindWithTag("Player").transform;
		}
		catch (NullReferenceException)
		{
			Debug.LogError("Cannot find the player!", this);
		}

	}
	protected override void Start()
	{
		Destroy(gameObject, rangedLifespan + beamWindUp);
		widthTimer = beamWindUp;
		damageTimer = damageTick;	
		CastRaycast(transform);
	}
	protected override void Update()
	{
		if (widthTimer > 0) widthTimer -= Time.deltaTime; // Timer that manages the beam's width.
		if (damageTimer > 0) damageTimer -= Time.deltaTime;
		width = Mathf.Lerp(boxCastWidth, 0.01f, widthTimer / beamWindUp);
		distanceToPlayer = Vector3.Distance(playerObject.position, transform.position);
		lineRenderer.startWidth = width;
		lineRenderer.endWidth = width;
		if (width == boxCastWidth)
		{
			lineRenderer.material = beamMaterialEnd;
			
			adjustedTurnSpeed = (turnSpeed * (maxAggro / distanceToPlayer));
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(playerObject.position.x, transform.position.y, playerObject.position.z) - transform.position, transform.up), adjustedTurnSpeed);
			CastRaycast(transform);
			makeABox(transform.position, boxEndPoint);
			// This checks if the player reenters the beam when after it's done charging, and damages them for it.
			Collider[] boxColliders = (Physics.OverlapBox(midPoint, new Vector3(boxCastWidth, boxCastHeight, distanceForBoxLength * 2), boxRotation, boxCastMask, QueryTriggerInteraction.Ignore));
			if (damageTimer <= 0)
			{
				CheckForPlayer(boxColliders);
			}
		}

		if (AttackCoroutine != null) return;
	}
	void CastRaycast(Transform startingPoint)
	{
		RaycastHit wallTargetRaycast;
		if (Physics.Raycast(startingPoint.position, startingPoint.forward, out wallTargetRaycast, rangeLimiter, raycastMask, QueryTriggerInteraction.Ignore))
		{
			lineRenderer.SetPosition(0, startingPoint.position);
			lineRenderer.SetPosition(1, wallTargetRaycast.point);
			boxEndPoint = wallTargetRaycast.point;
			AttackCoroutine = StartCoroutine(WaitForSeconds(beamWindUp, wallTargetRaycast.point));
		}
		else
		{
			lineRenderer.SetPosition(0, startingPoint.position);
			lineRenderer.SetPosition(1, startingPoint.position + startingPoint.forward * rangeLimiter);
			boxEndPoint = startingPoint.forward * rangeLimiter;
			AttackCoroutine = StartCoroutine(WaitForSeconds(beamWindUp, startingPoint.forward * rangeLimiter));
		}
	}
}
