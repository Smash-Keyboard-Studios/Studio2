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
/// Donno if we can use this for the special enemies or it has to be unquie per.
/// </summary>
public interface ITankAnimationStateUpdator
{
	public void EndAttack();

	public void DealAttack();

	public void StartAttack();

	public void EndSpecialAttack();

	public void DealSpecialAttack();

	public void StartSpecialAttack();
}
