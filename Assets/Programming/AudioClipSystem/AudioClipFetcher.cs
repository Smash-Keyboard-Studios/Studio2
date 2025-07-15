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
/// Handles fetching audio clips by using a given key to look for the clip in the stored collection.
/// </summary>
public class AudioClipFetcher : MonoBehaviour
{
    /// <summary>
    /// The singleton, use to get a reference to this object globally.
    /// </summary>
    public static AudioClipFetcher instance { get; private set; }

    /// <summary>
    /// The a collection of audio collections.
    /// </summary>
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

    /// <summary>
    /// Returns either the audio clip associated with the key or null.
    /// </summary>
    /// <param name="key">The key to search the database "Collection.Clip".</param>
    /// <returns>Either null or the audio clip.</returns>
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

        // if we dont return anything then we found nothing so we return null.
        return null;
    }
}
