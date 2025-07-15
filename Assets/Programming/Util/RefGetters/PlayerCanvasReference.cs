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


public class PlayerCanvasReference : MonoBehaviour
{
    public static PlayerCanvasReference instance { private set; get; }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogError("Duplicate " + nameof(PlayerCanvasReference) + " found, please remove the one of them!", this);
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public GameObject GetPlayerCanvasReference()
    {
        return gameObject;
    }
}
