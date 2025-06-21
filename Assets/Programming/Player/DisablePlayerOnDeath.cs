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
        GetComponent<PlayerMovementHandler>().enabled = false;
        GetComponent<Health>().enabled = false;
        GetComponent<ShieldAbility>().enabled = false;
    }
}
