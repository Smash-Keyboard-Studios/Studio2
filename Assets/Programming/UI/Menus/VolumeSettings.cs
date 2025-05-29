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
    [SerializeField] private Slider MasterSlider;
    [SerializeField] private Slider GameplaySlider;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider VoiceSlider;

    private void Start()
    {
        LoadVolume();
    }

    // sets the master volume according to the slider.
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterMix", volume);
    }

    // sets the music volume according to the slider.
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicMix", volume);
    }

    // sets the gameplay volume according to the slider.
    public void SetGameplayVolume(float volume) 
    {
        audioMixer.SetFloat("GameplayMix", volume);
    }

    // sets the voice volume according to slider
    public void SetVoiceVolume(float volume)
    {
        audioMixer.SetFloat("VoiceMix", volume);
    }

    // saves the volume to the player prefs
    // avoids the player having to redo audio settings each launch.
    public void SaveVolume()
    {
        // saves master volume
        audioMixer.GetFloat("MasterMix", out float masterVolume);
        PlayerPrefs.SetFloat("MasterMix", masterVolume);

        // saves music volume
        audioMixer.GetFloat("MusicMix", out float musicVolume);
        PlayerPrefs.SetFloat("MusicMix", musicVolume);

        // saves gameplay volume
        audioMixer.GetFloat("GameplayMix", out float gameplayVolume);
        PlayerPrefs.SetFloat("GameplayMix", gameplayVolume);

        // saves voice volume
        audioMixer.GetFloat("VoiceMix", out float voiceVolume);
        PlayerPrefs.SetFloat("VoiceMix", voiceVolume);
    }

    // loads the volume settings saved to player prefs.
    public void LoadVolume()
    {
        MasterSlider.value = PlayerPrefs.GetFloat("MasterMix");
        MusicSlider.value = PlayerPrefs.GetFloat("MusicMix");
        GameplaySlider.value = PlayerPrefs.GetFloat("GameplayMix");
        VoiceSlider.value = PlayerPrefs.GetFloat("VoiceMix");
    }

}
