using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|



public class TankAttackingStateUpdater : MonoBehaviour
{
	ITankAnimationStateUpdater animationStateUpdater;

	void Awake()
	{
		animationStateUpdater = GetComponentInParent<ITankAnimationStateUpdater>();
	}

	public void EndAttack()
	{
		animationStateUpdater.EndAttack();
	}

	public void DealAttack()
	{
		animationStateUpdater.DealAttack();
	}

	public void StartAttack()
	{

	}

	public void EndSpecialAttack()
	{
		animationStateUpdater.EndSpecialAttack();
	}

	public void DealSpecialAttack()
	{
		animationStateUpdater.DealSpecialAttack();
	}

	public void StartSpecialAttack()
	{

	}
}
