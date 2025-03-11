using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestAnimationFunctions : MonoBehaviour
{
	// by
	// 	   _	_         	  _  __
	//	  / \  | | _____  __ | |/ /
	//   / _ \ | |/ _ \ \/ / | ' /
	//  / ___ \| | __ />  <  | . \
	// /_/   \_\_|\___/_/\_\ |_|\_\
	PriestAI priestAI;
	void Awake()
	{
		priestAI = GetComponentInParent<PriestAI>();
	}

	public void EndAttack()
	{
		priestAI.EndAttack();
	}

	public void DealAttack()
	{
		priestAI.DealAttack();
	}

	public void StartAttack()
	{

	}
}
