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


public class PlayerDeathScreen : MonoBehaviour
{
    public GameObject playerDeathScreenObject;

    private GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        CloseDeathScreen();

        playerObject = GameObject.Find("Player");

        if (playerObject != null)
            playerObject.GetComponent<Health>().onDeath += OpenDeathScreen;
        else
            throw new NullReferenceException("Cannot find the player!");
    }

    void OnDisable()
    {
        // remove the event to prevent any errors. This should be killed with the player anyway but in case something changes.
        if (playerObject != null) playerObject.GetComponent<Health>().onDeath -= OpenDeathScreen;
    }

    private void OpenDeathScreen()
    {
        playerDeathScreenObject.SetActive(true);
    }

    private void CloseDeathScreen()
    {
        playerDeathScreenObject.SetActive(false);
    }
}
