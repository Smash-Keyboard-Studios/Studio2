using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|



public class AIShieldObject : MonoBehaviour, IShieldObject
{
	[SerializeField]
	private GameObject shieldObject;

	[SerializeField]
	private float maxHealth = 10f;

	private float currentHealth;

	[SerializeField]
	private float coolDown = 5f;

	private float currentCoolDownTime;

	[SerializeField]
	private float activationDuration = 5f;

	private float currentUseTimer = 0;

	private bool shieldActive = false;

	void Start()
	{
		currentHealth = maxHealth;
	}

	void Update()
	{
		if (currentHealth <= 0)
		{
			BreakShield();
		}

		if (currentCoolDownTime > 0)
		{
			currentCoolDownTime -= Time.deltaTime;
		}

		if (shieldActive && currentUseTimer < activationDuration)
		{
			currentUseTimer += Time.deltaTime;
		}
		else if (shieldActive)
		{
			BreakShield();
		}
	}


	public void ActivateShield()
	{
		if (currentCoolDownTime > 0) return;

		currentHealth = maxHealth;

		currentUseTimer = 0;

		shieldActive = true;
	}

	public void BreakShield()
	{
		currentCoolDownTime = coolDown;

		currentUseTimer = 0;

		shieldActive = false;

	}



	public bool CanUseShield()
	{
		if (currentCoolDownTime > 0) return false;
		if (shieldActive) return false;

		return true;
	}

	public bool IsUsingShield()
	{
		return shieldActive;
	}
}
