using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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


public class OnDeathEvent : MonoBehaviour
{
    [SerializeField]
    private AIBase aIToSubscribeTo;

    public UnityEvent onDeath;

    // Start is called before the first frame update
    void Start()
    {
        if (aIToSubscribeTo == null)
            aIToSubscribeTo = GetComponent<AIBase>();

        if (aIToSubscribeTo != null)
            aIToSubscribeTo.onDeath += InvokeOnDeath;
    }

    private void InvokeOnDeath(Transform eventData)
    {
        onDeath?.Invoke();
    }
}
