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
// Licensed for use within the Wraiths of Retail by Smash Keyboard Studios (SKS) only.
// Redistribution or modification outside of this project is prohibited without explicit written permission.
// For full license terms, see DOMIBRON_CODE_LICENSE.md at the project root.

/// <summary>
/// Removes the game object this is attached to after a period of time.
/// </summary>
public class RemoveAfterTime : MonoBehaviour
{
    /// <summary>
    /// How long to wait before destroying the game object this is attached to.
    /// </summary>
    public float timeToWaitBeforeRemoving = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToWaitBeforeRemoving);
    }
}
