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
/// Sits on the player prefab root and returns and has a function to get a ref so we dont rely on object names.
/// </summary>
public class PlayerReferenceFetcher : MonoBehaviour
{
    public static PlayerReferenceFetcher instance { private set; get; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogError("Duplicate " + nameof(PlayerReferenceFetcher) + " found, please remove the one of them!", this);
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public GameObject GetPlayerReference()
    {
        return gameObject;
    }
}
