using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|


public class AudioClipFetcher : MonoBehaviour
{
    public static AudioClipFetcher instance { get; private set; }

    public AudioCollectionSO[] audioCollection;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public AudioClip GetClipFromKey(string key)
    {
        string[] keys = key.Split('.');

        AudioCollectionSO foundCollection = null;

        // find the collection on the list.
        foreach (var collection in audioCollection)
        {
            if (collection == null)
            {
                Debug.LogError("There is a null collection, please remove it!", this);
                continue;
            }

            if (collection.CollectionName == keys[0])
            {
                foundCollection = collection;
            }
        }

        // find the clip on that collection
        foreach (var audioData in foundCollection.audioCollection)
        {
            if (audioData.audioClip == null)
            {
                Debug.LogError($"There is no clip on {gameObject.name} > {foundCollection.CollectionName} > {audioData.clipName},"
                + "please add a audio clip or remove the item from list", this);
                continue;
            }

            if (audioData.clipName == keys[1])
            {
                return audioData.audioClip;
            }
        }

        return null;
    }
}
