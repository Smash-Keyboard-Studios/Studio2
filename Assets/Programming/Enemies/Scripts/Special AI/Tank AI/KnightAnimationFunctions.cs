using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|



public class KnightAnimationFunctions : MonoBehaviour
{
	KnightAI knightAI;

	void Awake()
	{
		knightAI = GetComponentInParent<KnightAI>();
	}

	public void EndAttack()
	{
		knightAI.EndAttack();
	}

	public void DealAttack()
	{
		knightAI.DealAttack();
	}

	public void EndSpecialAttack()
	{
		knightAI.EndSpecialAttack();
	}

	public void DealSpecialAttack()
	{
		knightAI.DealSlamAttack();
	}
}
