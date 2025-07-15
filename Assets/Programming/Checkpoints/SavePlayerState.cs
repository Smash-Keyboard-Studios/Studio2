using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
/// Stores the player's state into the player checkpoint manager.
/// </summary>
public class SavePlayerState : MonoBehaviour
{
    /// <summary>
    /// A string to display the GUID to the inspector, that's it.
    /// </summary>
    public string guid = Guid.NewGuid().ToString();


    /// <summary>
    /// Called when the player is loading back to this checkpoint.
    /// </summary>
    public UnityEvent onLoadCheckpoint;


    /// <summary>
    /// Called when a checkpoint was triggered successfully. Useful for UI display.
    /// </summary>
    public event Action onSaveData;


    void OnValidate()
    {
        if (string.IsNullOrEmpty(guid)) // we create a new guid
        {
            guid = Guid.NewGuid().ToString();
        }
    }

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
        PlayerCheckpointManager.instance.StorePlayerState(playerObject.transform.position, playerObject.GetComponent<PlayerAttackHandler>().heavyAttackUnlocked,
        playerObject.GetComponent<ShieldAbility>().unlockedShield, SceneManager.GetSceneAt(1).buildIndex, guid);
    }

    public void InvokeLoadCheckpoint()
    {
        onLoadCheckpoint?.Invoke();
    }
}
