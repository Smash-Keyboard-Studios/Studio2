using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingStateUpdator : MonoBehaviour
{
	AICommonMeleeCombat commonMeleeCombat;

	void Awake()
	{
		commonMeleeCombat = GetComponentInParent<AICommonMeleeCombat>();
	}

	public void EndAttack()
	{
		commonMeleeCombat.AnimationAttackFinished();
	}

	public void DealAttack()
	{
		commonMeleeCombat.AttackAndDamage();
	}

	public void StartAttack()
	{

	}
}
