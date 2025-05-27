using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//by	_             	_ _                	 
// 	| |           	(_) |               	 
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|






public class RangedAttackingStateUpdator : MonoBehaviour
{
	AICommonRangedCombat animationStateUpdator;

	void Awake()
	{
		animationStateUpdator = GetComponentInParent<AICommonRangedCombat>();
	}

	public void EndAttack()
	{
		animationStateUpdator.EndAttack();
	}

	public void DealAttack()
	{
		animationStateUpdator.DealAttack();
	}

	public void StartAttack()
	{

	}
}
