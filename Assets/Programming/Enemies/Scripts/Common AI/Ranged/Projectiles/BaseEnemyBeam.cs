using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
// by
//	   _	_         	  _  __
//	  / \  | | _____  __ | |/ /
//   / _ \ | |/ _ \ \/ / | ' /
//  / ___ \| | __ />  <  | . \
// /_/   \_\_|\___/_/\_\ |_|\_\
// With help from Dom & Dan.


public class BaseEnemyBeam : BaseEnemyProjectile
{
	#region Timers
	[Header("Timer controls.")]
	[SerializeField] private float damageTick;
	[SerializeField] private float beamWindUp;
	protected float widthTimer = 0, damageTimer = 0;
	#endregion
	[Header("Do not touch these.")]
	/// <summary> 	Layermask that ignores only the player to find where the beam actually hits.  </summary>
	[SerializeField] private LayerMask raycastMask;
	/// <summary> 	For the Overlapbox to grab only the player. </summary>
	[SerializeField] private LayerMask boxCastMask;
	private float rangeLimiter = 999f; 
	#region BoxCollider
	[Header("Values for box collider check.")]
	/// <summary> 	AI up (local Y), how tall this check box is.  </summary>
	[SerializeField] protected float boxCastHeight = 5;
	/// <summary> 	How wide this check box is.  </summary>
	[SerializeField] protected float boxCastWidth = 2f;
	/// <summary> 	How long this check box is.  </summary>
	private float distanceForBoxLength;
	/// <summary> 	The actual world position for the box collider.  </summary>
	private Vector3 midPoint;
	/// <summary> 	How wide this check box is.  </summary>
	private Quaternion boxRotation;
	protected Collider[] boxHitColliders;
	#endregion
	#region Line Renderer Specifics
	[Header("Visuals.")]
	[SerializeField] private LineRenderer lineRenderer;
	[SerializeField] public Material beamMaterialStart, beamMaterialEnd;
	#endregion

	private Coroutine AttackCoroutine = null;

	protected override void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.material = beamMaterialStart;
	}
	protected override void Start()
	{
		Destroy(gameObject, rangedLifespan+beamWindUp);
		widthTimer = beamWindUp;
		damageTimer = damageTick;
		RaycastHit wallTargetRaycast;
		if (Physics.Raycast(transform.position, transform.forward, out wallTargetRaycast, rangeLimiter, raycastMask, QueryTriggerInteraction.Ignore))
		{
			lineRenderer.SetPosition(0, transform.position);
			lineRenderer.SetPosition(1, wallTargetRaycast.point);

			AttackCoroutine = StartCoroutine(WaitForSeconds(beamWindUp, wallTargetRaycast.point));
		}
		else
		{
			lineRenderer.SetPosition(0, transform.position);
			lineRenderer.SetPosition(1, transform.position + transform.forward * rangeLimiter);

			AttackCoroutine = StartCoroutine(WaitForSeconds(beamWindUp, transform.forward * rangeLimiter));
		}
	}
	protected override void Update()
	{
		if (widthTimer > 0) widthTimer -= Time.deltaTime; // Timer that manages the beam's width.
		if (damageTimer > 0) damageTimer -= Time.deltaTime;
		var width = Mathf.Lerp(boxCastWidth, 0.01f, widthTimer / beamWindUp);
		lineRenderer.startWidth = width;
		lineRenderer.endWidth = width;
		if (width == boxCastWidth) 
		{ 
			lineRenderer.material = beamMaterialEnd;
			// This checks if the player reenters the beam when after it's done charging, and damages them for it.
			Collider[] boxColliders = (Physics.OverlapBox(midPoint, new Vector3(boxCastWidth, boxCastHeight, distanceForBoxLength * 2), boxRotation, boxCastMask, QueryTriggerInteraction.Ignore)); 
			if (damageTimer <= 0)
			{
				CheckForPlayer(boxColliders);
			}
		}
		
		if (AttackCoroutine != null) return;
	}
	void CheckForPlayer(Collider[] boxHitColliders)
	{
		if (boxHitColliders.Length > 0)
		{
			foreach (var hitObject in boxHitColliders)
			{
				if (hitObject.gameObject.CompareTag("Player"))
				{
					DealDamage(hitObject);
				}
			}
		}
	}
	protected void makeABox(Vector3 startLocation, Vector3 hitPoint)
	{
		midPoint = new Vector3(startLocation.x + (hitPoint.x - startLocation.x) / 2, startLocation.y + (hitPoint.y - startLocation.y) / 2, startLocation.z + (hitPoint.z - startLocation.z) / 2);
		distanceForBoxLength = Vector3.Distance(startLocation, hitPoint);
		boxRotation = Quaternion.LookRotation((hitPoint - startLocation), Vector3.up);
		Collider[] boxColliders = ( Physics.OverlapBox(midPoint, new Vector3(boxCastWidth, boxCastHeight, distanceForBoxLength * 2), boxRotation, boxCastMask, QueryTriggerInteraction.Ignore));
		CheckForPlayer(boxColliders);
	}

	protected IEnumerator WaitForSeconds(float waitTime, Vector3 targetPoint)
	{
		yield return new WaitForSeconds(waitTime);
		while (true)
		{
			makeABox(transform.position, targetPoint);
			yield return new WaitForSeconds(1);
		}
		AttackCoroutine = null;
	}
	protected void DealDamage(Collider collidedObject)
	{
		//Debug.Log("Player hit for: " + projectileDamage);
		collidedObject.gameObject.GetComponent<IDamageable>()?.TakeDamage(rangedDamage);
		damageTimer = damageTick; // Damage has been taken so cooldown reset.
	}
}
