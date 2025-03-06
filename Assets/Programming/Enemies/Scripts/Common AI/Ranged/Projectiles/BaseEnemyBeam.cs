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


public class BaseEnemyBeam : BaseEnemyProjectile
{
	[SerializeField] private float increment;
	[SerializeField] LineRenderer lineRenderer;
	[SerializeField] private float rangeLimiter;
	//[SerializeField] private RaycastHit wallTargetRaycast;
	[SerializeField] private LayerMask raycastMask;
	[SerializeField] private LayerMask boxCastMask;
	[Header("Box check for light attack")]
	[SerializeField] protected float boxCastThickness = 2f;
	[SerializeField] private Vector3 midPoint;
	[SerializeField] private float distanceForBoxLength;
	[SerializeField] private float beamWindUp;
	/// <summary> 	AI up (local Y), how tall this check box is.  </summary>
	[SerializeField] protected float boxCastHeight = 1;
	[SerializeField] protected Collider[] boxHitColliders;

	float timer = 0;

	private Coroutine AttackCoroutine = null;

	protected override void Awake()
	{
	}
	protected override void Start()
	{
		Destroy(gameObject, projectileLifespan);

		RaycastHit wallTargetRaycast;
		if (Physics.Raycast(transform.position, transform.forward, out wallTargetRaycast, rangeLimiter, raycastMask, QueryTriggerInteraction.Ignore))
		{
			lineRenderer.SetPosition(0, transform.position);
			lineRenderer.SetPosition(1, wallTargetRaycast.point);

			AttackCoroutine = StartCoroutine(WaitForSeconds(beamWindUp, wallTargetRaycast.point));
			timer = beamWindUp;

			//StartCoroutine(waitForSeconds(beamWindUp));
			//CheckForPlayer();
		}
		else
		{
			lineRenderer.SetPosition(0, transform.position);
			lineRenderer.SetPosition(1, transform.position + transform.forward * 999f);

			AttackCoroutine = StartCoroutine(WaitForSeconds(beamWindUp, wallTargetRaycast.point));
			//makeABox(transform.position, transform.position + transform.forward * 999f);
		}
	}
	protected override void Update()
	{
		if (timer > 0) timer -= Time.deltaTime;

		var width = Mathf.Lerp(boxCastThickness, 00.01f, timer / beamWindUp);
		lineRenderer.startWidth = width;
		lineRenderer.endWidth = width;

		if (AttackCoroutine != null) return;

		increment = increment + Time.deltaTime;
		if (increment <= (projectileLifespan + beamWindUp))
		{
			
		}
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
					//boxHitColliders. 
				}
			}
		}
	}
	protected void makeABox(Vector3 startLocation, Vector3 hitPoint)
	{
		midPoint = new Vector3(startLocation.x + (hitPoint.x - startLocation.x) / 2, startLocation.y + (hitPoint.y - startLocation.y) / 2, startLocation.z + (hitPoint.z - startLocation.z) / 2);
		distanceForBoxLength = Vector3.Distance(startLocation, hitPoint);
		Quaternion boxRotation = Quaternion.LookRotation((hitPoint - startLocation), Vector3.up);
		Collider[] colliders = ( Physics.OverlapBox(midPoint, new Vector3(boxCastThickness, boxCastHeight, distanceForBoxLength * 2), boxRotation, boxCastMask, QueryTriggerInteraction.Ignore));
		
		foreach(Collider collider in colliders)
		{
			if (collider.gameObject.CompareTag("Player"))
			collider.GetComponent<IDamageable>()?.TakeDamage(1f);
		}

	}

	private void OnDrawGizmos()
	{
		var startLocation = transform.position;
		Vector3 wallTargetRaycast = transform.position + transform.forward * 100f;

		//Physics.Raycast(transform.position, transform.forward,  out wallTargetRaycast, rangeLimiter, raycastMask, QueryTriggerInteraction.Ignore);
		midPoint = new Vector3(startLocation.x + (wallTargetRaycast.x - startLocation.x) / 2, startLocation.y + (wallTargetRaycast.y - startLocation.y) / 2, startLocation.z + (wallTargetRaycast.z - startLocation.z) / 2);
		distanceForBoxLength = Vector3.Distance(startLocation, wallTargetRaycast);
		Quaternion boxRotation = Quaternion.LookRotation((wallTargetRaycast - startLocation), Vector3.up);

		
		Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, boxRotation, Vector3.one);
		Gizmos.DrawWireCube(midPoint, new Vector3(boxCastThickness, boxCastHeight, distanceForBoxLength * 2));
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
		//yield return new WaitForSeconds(beamWindUp);
		//Debug.Log("Player hit for: " + projectileDamage);
		collidedObject.gameObject.GetComponent<IDamageable>()?.TakeDamage(projectileDamage);
    }
}
