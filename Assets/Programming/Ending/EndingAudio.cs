using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingAudio : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private AudioClip startEngineClip;
    [SerializeField] private AudioClip runningEngineClip;

    private AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("road?", false);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        
        StartCoroutine("PlayEndingAudio");
    }

    private IEnumerator PlayEndingAudio()
    {
        yield return new WaitForSeconds(2f);

        animator.SetBool("road?", true);

        audioSource.clip = startEngineClip;
        audioSource.Play();

        yield return new WaitForSeconds(startEngineClip.length);

        audioSource.clip = runningEngineClip;
        audioSource.Play();
    }
}
