using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //stuff ensuring there is a single instance of the Audio Manager
    [HideInInspector] public static AudioManager Instance { get { return instance; } }
    private static AudioManager instance;

    //audio clips as separate class so its got a name and then all the audio clips associated with that
    [System.Serializable] public class AudioLibrary
    {
        public string clipName;
        public AudioClip[] audioClips;
    }

    //the audio clips and their names stored here
    public AudioLibrary[] audioClips;

    private void Awake()
    {
        //makes sure there is only one Audio Manager and that it is set to this
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // prevents this from being destroyed between scenes
        }
    }




    private AudioClip[] FindAudioFromClipName(string clipName)
    {
        //finds which clips to play from the list of dictionaries
        foreach (AudioLibrary audioLibrary in audioClips)
        {
            if (audioLibrary.clipName == clipName)
            {
                return audioLibrary.audioClips;
            }
        }
        return null; //if clip no found
    }

    private void PlayAudioClip(bool looping, bool loopFromStart, AudioSource audioSource, AudioClip clipToPlay) //plays an audio clip
    {
        //check if looping
        if (looping)
        {
            int currentAudioTime = loopFromStart? 0 : audioSource.timeSamples; //gets current point where music is playing at if not looping from the start of the song

            StopAudio(audioSource); //stops what is currently playing if its a looping audio
            audioSource.loop = true;
            audioSource.clip = clipToPlay;

            audioSource.Play(); //play the clip!!
            audioSource.timeSamples = currentAudioTime;
        }
        else
        {
            audioSource.PlayOneShot(clipToPlay); //play the clip oneshot!!
        }
    }




    //THE AUDIO BELOW IS FOR OTHER SCRIPTS TO USE FROM THE AUDIOMANAGER

    public void PlayAudio(bool looping, bool loopFromStart, AudioSource audioSource, string clipName) //plays random audio associated with the string name param
    {
        //this will be the selection of clips associated with the clipname,
        //of which a random clip will be selected to play
        AudioClip[] clipsToPlay = FindAudioFromClipName(clipName);

        //if found the clips to play
        if (clipsToPlay != null)
        {
            Debug.Log("Playing " + clipName); //we found the clips to play
            AudioClip clipToPlay = clipsToPlay[UnityEngine.Random.Range(0, clipsToPlay.Length)]; //choose a random clip from the selection of audio clips

            PlayAudioClip(looping, loopFromStart, audioSource, clipToPlay);
        }
    }

    public void PlayAudio(bool looping, bool loopFromStart, AudioSource audioSource, string clipName, int ClipIndex) //version with specific clip index to reference
    {
        //this will be the selection of clips associated with the clipname,
        //of which a random clip will be selected to play
        AudioClip[] clipsToPlay = FindAudioFromClipName(clipName);

        //if found the clips to play
        if (clipsToPlay != null)
        {
            Debug.Log("Playing " + clipName); //we found the clips to play
            AudioClip clipToPlay = clipsToPlay[ClipIndex]; //choose clipindex from the selection of audio clips

            PlayAudioClip(looping, loopFromStart, audioSource, clipToPlay);
        }
    }

    public void StopAudio(AudioSource audioSource)
    {
        audioSource.Stop();
    }
}
