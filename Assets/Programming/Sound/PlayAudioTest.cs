using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioTest : MonoBehaviour
{
    AudioSource audioSource;
    public string clipName;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayAudio(false, audioSource, clipName);
        }
    }
}
