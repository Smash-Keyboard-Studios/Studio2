using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//by	_             	_ _                	 
// 	| |           	(_) |               	 
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|






public class RangedAttackingStateUpdater : MonoBehaviour
{
	AIImprovedCommonRanged animationStateUpdater;

	void Awake()
	{
		animationStateUpdater = GetComponentInParent<AIImprovedCommonRanged>();
	}

	public void EndAttack()
	{
		animationStateUpdater.EndAttack();
	}

	public void DealAttack()
	{
		animationStateUpdater.DealAttack();
	}
}
