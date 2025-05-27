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
            playerObject.GetComponent<Health>().onDeathEvent += OpenDeathScreen;
        else
            throw new NullReferenceException("Cannot find the player!");
    }

    void OnDisable()
    {
        // remove the event to prevent any errors. This should be killed with the player anyway but in case something changes.
        if (playerObject != null) playerObject.GetComponent<Health>().onDeathEvent -= OpenDeathScreen;
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
