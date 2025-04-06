using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider MainSlider;
    [SerializeField] private Slider GameplaySlider;
    [SerializeField] private Slider MusicSlider;

    // sets the master volume according to the slider.
    public void SetMasterVolume()
    {
        float volume = MainSlider.value;

        audioMixer.SetFloat("MasterMix", Mathf.Log10(volume) * 20);
    }

    // sets the music volume according to the slider.
    public void SetMusicVolume()
    {
        float volume = MainSlider.value;

        audioMixer.SetFloat("MusicMix", Mathf.Log10(volume) * 20);
    }

    // sets the gameplay volume according to the slider.
    public void SetGameplayVolume() 
    {
        float volume = MainSlider.value;

        audioMixer.SetFloat("GameplayMix", Mathf.Log10(volume) * 20);
    }


}
