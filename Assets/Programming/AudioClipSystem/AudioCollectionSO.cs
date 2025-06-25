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
