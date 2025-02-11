using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSFXPlayer : MonoBehaviour
{
	BaseEnemyProjectile referencedProjectile;
	void Start()
	{
		referencedProjectile = GetComponent<BaseEnemyProjectile>();
		referencedProjectile.onSFXImpact += SFXImpact;
	}

	void SFXImpact()
	{
		// Audio people work here, ty
	}
}
