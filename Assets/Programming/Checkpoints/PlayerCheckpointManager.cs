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
/// Its the checkpoint manager that deals with storing and loading data.
/// </summary>
public class PlayerCheckpointManager : MonoBehaviour
{
    // you can still follow technical doc with this style of singleton. -.-
    public static PlayerCheckpointManager instance { get { return localInstance; } private set { localInstance = value; } }
    private static PlayerCheckpointManager localInstance;

    /// <summary>
    /// Does the checkpoint manage have any saved data.
    /// </summary>
    public bool hasPlayerState = false;

    /// <summary>
    /// What level has the saved data.
    /// </summary>
    public int levelWithState = -1;

    /// <summary>
    /// The player's position after hitting the checkpoint.
    /// </summary>
    public Vector3 playerLastPosition;

    // Need to store unlocks as they will be reset.
    /// <summary>
    /// Does the player have the heavy attack when triggering the checkpoint.
    /// </summary>
    public bool hasHeavyAttack = false;

    /// <summary>
    /// Does the player have the shield unlocked when triggering the checkpoint.
    /// </summary>
    public bool hasShield = false;



    void Awake()
    {
        // singleton.
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        // silent error
        try
        {
            // do we have any data to reset.
            if (!hasPlayerState) return;

            // are we loading still?
            if (LevelLoading.instance.loading) return;

            // are we fucked?
            if (SceneManager.GetSceneAt(1) == null) return;

            // are we on the main menu, credits or on a different scene to the saved data. 
            if (SceneManager.GetSceneAt(1).buildIndex == 1 || SceneManager.GetSceneAt(1).buildIndex == 2 || SceneManager.GetSceneAt(1).buildIndex != levelWithState)
                ResetState(); // if so, reset it.
        }
        catch
        {
            return; // fail silently otherwise others will complain.
        }

    }



    /// <summary>
    /// Stores the player data onto the manager.
    /// </summary>
    /// <param name="playerPosition">The location of the player to save.</param>
    /// <param name="heavyUnlocked">If the heavy attack was unlocked.</param>
    /// <param name="shieldUnlocked">If the shield was unlocked.</param>
    /// <param name="level">What level did the save occur on.</param>
    public void StorePlayerState(Vector3 playerPosition, bool heavyUnlocked, bool shieldUnlocked, int level)
    {
        playerLastPosition = playerPosition + new Vector3(0, 1, 0);

        hasHeavyAttack = heavyUnlocked;

        hasShield = shieldUnlocked;

        levelWithState = level;

        hasPlayerState = true;

        Debug.Log("Saved player's state");
    }



    /// <summary>
    /// Resets the save data to a blank slate, good for loading onto a new level.
    /// </summary>
    public void ResetState()
    {
        hasPlayerState = false;

        playerLastPosition = Vector3.zero;

        hasHeavyAttack = false;

        hasShield = false;

        levelWithState = -1;


        Debug.Log("Last's player state was reset");
    }


    /// <summary>
    /// Sets the player state that was saved.
    /// </summary>
    /// <param name="playerObject">The player object.</param>
    public void LoadPlayerState(GameObject playerObject)
    {
        if (!hasPlayerState) return;

        if (SceneManager.GetSceneAt(1).buildIndex != levelWithState) return;

        // playerObject.transform.position = playerLastPosition;

        playerObject.GetComponent<PlayerAttack>().unlockedHeavyAttack = hasHeavyAttack;
        playerObject.GetComponent<ShieldAbility>().unlockedShield = hasShield;

        playerObject.GetComponent<PlayerMovement>().Warp(playerLastPosition);

        Debug.Log("Loaded player's last state");
    }
}



