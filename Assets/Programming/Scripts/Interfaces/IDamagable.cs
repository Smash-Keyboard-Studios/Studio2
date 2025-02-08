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
/// The interface for reciving damage.
/// </summary>
public interface IDamagable
{
	/// <summary>
	/// Tells the entity to recive the damage provided.
	/// </summary>
	/// <param name="ammount">Ammount of damage to give.</param>
	/// <returns>True if it was successful.</returns>
	public bool TakeDamage(float ammount);
}
