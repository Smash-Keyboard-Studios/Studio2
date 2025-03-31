using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
