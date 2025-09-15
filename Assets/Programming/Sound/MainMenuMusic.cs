using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayBGMusic();
    }

    private void PlayBGMusic()
    {
        AudioManager.Instance.PlayAudio(true, true, audioSource, "MainMenuTheme");
    }
}
