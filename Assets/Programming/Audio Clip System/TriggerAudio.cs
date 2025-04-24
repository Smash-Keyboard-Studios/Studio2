using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class TriggerAudio : MonoBehaviour
{
    AudioSource source;

    public bool loopAudio = false;
    public bool loopFromStart = false;
    public string audioName;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    public void PlaySound()
    {
        AudioManager.Instance.PlayAudio(loopAudio, loopFromStart, source, audioName);
    }
}