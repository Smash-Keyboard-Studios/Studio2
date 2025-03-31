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



[Serializable]
public struct AudioData
{
    public string clipName;
    public AudioClip audioClip;
}

[CreateAssetMenu(menuName = "AudioSystem/AudioCollectionSO")]
public class AudioCollectionSO : ScriptableObject
{
    public string CollectionName = "ACollection";

    public AudioData[] audioCollection;
}
