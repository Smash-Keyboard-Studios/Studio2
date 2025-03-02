using System;
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
/// The interface for receiving damage.
/// </summary>

//[Obsolete("Please use the Health component instead.", false)]
public interface IDamageable
{
	/// <summary>
	/// Tells the entity to receive the damage provided.
	/// </summary>
	/// <param name="amount">Amount of damage to give.</param>
	/// <returns>True if it was successful.</returns>
	public bool TakeDamage(float amount);
}
