using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioTest : MonoBehaviour
{
    AudioSource audioSource;
    public string clipName;
    public bool isLooping;
    public bool loopFromStartOfClip;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayAudio(isLooping, loopFromStartOfClip, audioSource, clipName);
        }
    }
}
