using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //stuff ensuring there is a single instance of the Audio Manager
    [HideInInspector] public static AudioManager Instance { get { return _instance; } }
    private static AudioManager _instance;

    public List<Dictionary<string, AudioClip[]>> audioClips = new List<Dictionary<string, AudioClip[]>>();

    private void Awake()
    {
        //makes sure there is only one Audio Manager and that it is set to this
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // prevents this from being destroyed between scenes
        }
    }

    public void PlayAudio(AudioSource audioSource, string clipName)
    {
        AudioClip[] clipsToPlay = null;

        foreach (Dictionary<string, AudioClip[]> dictionary in audioClips)
        {
            if (dictionary.ContainsKey(clipName))
            {
                clipsToPlay = dictionary[clipName];
                break;
            }
        }

        if (clipsToPlay != null)
        {
            AudioClip clipToPlay = clipsToPlay[UnityEngine.Random.Range(0, clipsToPlay.Length)];
            audioSource.PlayOneShot(clipToPlay);
        }
    }
}
