using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|
// © 2025 Dominic McNeill dommcneill@outlook.com

/// <summary>
/// Used for interacting with shields.
/// </summary>
public interface IShieldObject
{
    /// <summary>
    /// Gets whether the shield is active or not.
    /// </summary>
    public bool isShieldActive { get; }

    /// <summary>
    /// Breaks the shield.
    /// </summary>
    public void BreakShield();
}
