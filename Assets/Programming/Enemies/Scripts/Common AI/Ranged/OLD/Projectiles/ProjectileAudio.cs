using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAudio : MonoBehaviour
{
	//by
	//     _    _             _  __
	//    / \  | | _____  __ | |/ /
	//   / _ \ | |/ _ \ \/ / | ' / 
	//  / ___ \| | __ />  <  | . \ 
	// /_/   \_\_|\___/_/\_\ |_|\_\
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
