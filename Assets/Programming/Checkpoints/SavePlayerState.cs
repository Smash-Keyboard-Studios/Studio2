using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|

/// <summary>
/// Stores the player's state into the player checkpoint manager.
/// </summary>
public class SavePlayerState : MonoBehaviour
{
    /// <summary>
    /// Called when a checkpoint was triggered successfully. Useful for UI display.
    /// </summary>
    public event Action onSaveData;

    /// <summary>
    /// Saves the player's state to the checkpoint manager.
    /// </summary>
    public void SaveState()
    {
        // check if the player checkpoint manager exists.
        if (PlayerCheckpointManager.instance == null)
        {
            Debug.LogError($"Cannot find the {nameof(PlayerCheckpointManager)}", this);

            return;
        }

        // Get the player.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // check to see if the player is null.
        if (playerObject == null)
        {
            Debug.LogError($"Cannot find the player", this);

            return;
        }

        onSaveData?.Invoke();

        GameObject.FindGameObjectWithTag("LoadPlayerState")?.GetComponent<CheckpointSavedUI>()?.DisplayIcon();

        // pass in the player's data to be saved.
        PlayerCheckpointManager.instance.StorePlayerState(playerObject.transform.position, playerObject.GetComponent<PlayerAttack>().unlockedHeavyAttack,
        playerObject.GetComponent<ShieldAbility>().unlockedShield, SceneManager.GetSceneAt(1).buildIndex);
    }
}
