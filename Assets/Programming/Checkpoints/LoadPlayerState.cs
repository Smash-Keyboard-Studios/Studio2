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

/// <summary>
/// Checks for a saved player state and loads it.
/// </summary>
public class LoadPlayerState : MonoBehaviour
{
    /// <summary>
    /// Called when loading the saved sate of the player.
    /// </summary>
    public event Action onLoadData;

    // Start is called before the first frame update
    void Start()
    {
        // check to see if the player checkpoint manager exists.
        if (PlayerCheckpointManager.instance == null)
        {
            Debug.LogError($"Cannot find the {nameof(PlayerCheckpointManager)}");

            return;
        }


        // check if we have a saved state.
        if (!PlayerCheckpointManager.instance.hasPlayerState) return;


        // get the player.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // check if we did get the player.
        if (playerObject == null)
        {
            Debug.LogError($"Cannot find the player", this);

            return;
        }

        onLoadData?.Invoke();

        // load the checkpoint.
        PlayerCheckpointManager.instance.LoadPlayerState(playerObject);
    }
}
