using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|




/// <summary>
/// Special Tank AI behavior class. Controls movement, attacking and thinking.
/// </summary>
public class AISpecialTank : AICommonMeleeCombat
{


	protected Coroutine heavyAttackCoroutine;










	protected override void AlertedThinking()
	{
		// ! exact same as base.

		if (Vector3.Distance(playerTarget.position, transform.position) < minDistanceForAttack)
		{
			// attack // TODO Speed needs to be handled elsewhere. It breaks with animations
			if (Vector3.Distance(playerTarget.position, transform.position) < 1.55f || attacking) currentSpeed = 0.4f;
			else currentSpeed = maxSpeed;


			// we need to decide what attack to use. We want to use the powerful attack first.



			// light attack


			if (!attacking && lightAttackCooldown <= 0f && lightAttackCoroutine == null) lightAttackCoroutine = StartCoroutine(LightAttack());
		}

		pathTarget = playerTarget.position;
	}

	protected virtual IEnumerator HeavyAttack()
	{
		yield return null;

	}
}
