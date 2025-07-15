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

/// <summary>
/// Audio clip data, has a name given to the associated audio clip.
/// </summary>
[Serializable]
public struct AudioData
{
    /// <summary>
    /// The clip name / key.
    /// </summary>
    public string clipName;

    /// <summary>
    /// The audio clip associated with the name / key.
    /// </summary>
    public AudioClip audioClip;
}

/// <summary>
/// Audio collection used to store audio clips associated to the collection name
/// </summary>
[CreateAssetMenu(menuName = "AudioSystem/AudioCollectionSO")]
public class AudioCollectionSO : ScriptableObject
{
    /// <summary>
    /// The collection key / name.
    /// </summary>
    public string CollectionName = "ACollection";

    /// <summary>
    /// The audio clips to associate with the collection.
    /// </summary>
    public AudioData[] audioCollection;
}
