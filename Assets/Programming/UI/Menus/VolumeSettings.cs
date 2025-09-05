using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider MainSlider;
    [SerializeField] private Slider GameplaySlider;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider VoiceSlider;

    // sets the master volume according to the slider.
    public void SetMasterVolume(float sliderValue)
    {
        audioMixer.SetFloat("MasterMix", Mathf.Log10(sliderValue) * 20);
    }

    // sets the music volume according to the slider.
    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("MusicMix", Mathf.Log10(sliderValue) * 20);
    }

    // sets the gameplay volume according to the slider.
    public void SetGameplayVolume(float sliderValue) 
    {
        audioMixer.SetFloat("GameplayMix", Mathf.Log10(sliderValue) * 20);
    }

    public void SetVoiceVolume(float sliderValue)
    {
        audioMixer.SetFloat("VoiceMix", Mathf.Log10(sliderValue) * 20);
    }

}
