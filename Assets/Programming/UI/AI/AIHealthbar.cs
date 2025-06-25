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
public class AIHealthBar : MonoBehaviour
{
	private Health health;

	public Slider healthBar;

	// Start is called before the first frame update
	void Start()
	{
		health = GetComponent<Health>();
	}

	// Update is called once per frame
	void Update()
	{
		healthBar.value = health.GetHealthNormalized();
	}
}
