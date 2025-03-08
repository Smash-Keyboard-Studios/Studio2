using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|



/// <summary>
/// Sets the health bar on the AI to match the current health.
/// </summary>
public class AIHealthBarAndShield : MonoBehaviour
{
	private HealthWithShield healthWithShield;

	public Slider healthBar;
	public Slider shieldBar;

	// Start is called before the first frame update
	void Start()
	{
		healthWithShield = GetComponent<HealthWithShield>();
	}

	// Update is called once per frame
	void Update()
	{
		healthBar.value = healthWithShield.GetHealthNormalized();
		shieldBar.value = healthWithShield.GetShieldNormalized();
	}
}
