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
/// Sets the healthbar on the AI to match the current health.
/// </summary>
public class AIHealthbar : MonoBehaviour
{
	private AIBase aiBase;

	public Slider healthBar;

	// Start is called before the first frame update
	void Start()
	{
		aiBase = GetComponent<AIBase>();
	}

	// Update is called once per frame
	void Update()
	{
		healthBar.value = (aiBase.currentHealth / aiBase.maxHealth);
	}
}
