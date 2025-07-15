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
// © 2025 Dominic McNeill dommcneill@outlook.com
// Licensed for use within the Wraiths of Retail by Smash Keyboard Studios (SKS) only.
// Redistribution or modification outside of this project is prohibited without explicit written permission.
// For full license terms, see DOMIBRON_CODE_LICENSE.md at the project root.

// :3



public class DisablePlayerOnDeath : MonoBehaviour
{
    void Start()
    {
        GetComponent<Health>().onDeath += OnDeath;
    }

    private void OnDeath()
    {
        GetComponentInChildren<Animator>().SetBool("IsDead", true);

        GetComponent<PlayerAttackHandler>().enabled = false;
        GetComponentInChildren<PlayerRotation>().enabled = false;
        GetComponent<PlayerMovementHandler>().enabled = false;
        GetComponent<Health>().enabled = false;
        GetComponent<ShieldAbility>().enabled = false;
    }
}
