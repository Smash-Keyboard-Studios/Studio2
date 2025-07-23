using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingAudio : MonoBehaviour
{
    [SerializeField] private AudioClip startEngineClip;
    [SerializeField] private AudioClip runningEngineClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = startEngineClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    private void Update()
    {
        //once stopped playing is detected we can play running engine clip looping
        if (!audioSource.isPlaying)
        {
            audioSource.clip = runningEngineClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
