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
/// Allows this object to be damaged without actually taking damage.
/// </summary>
public class Dummy : MonoBehaviour, IDamageable
{
	bool IDamageable.TakeDamage(float Ammount)
	{
		print($"{gameObject.name}: Taken {Ammount} damage!");
		return true;
	}
}
