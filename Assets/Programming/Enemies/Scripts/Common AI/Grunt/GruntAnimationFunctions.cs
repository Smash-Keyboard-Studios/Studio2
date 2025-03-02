using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|






public class GruntAnimationFunctions : MonoBehaviour
{
	GruntAI gruntAI;

	void Awake()
	{
		gruntAI = GetComponentInParent<GruntAI>();
	}

	public void EndAttack()
	{
		gruntAI.EndAttack();
	}

	public void DealAttack()
	{
		gruntAI.DealAttack();
	}
}
