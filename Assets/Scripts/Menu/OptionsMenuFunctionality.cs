using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenuFunctionality : MonoBehaviour
{
    // Reference to the AudioMixer for adjusting volumes
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    // UI Sliders for music and SFX volume
    [Header("Volume Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Quality Dropdown")]
    public TMP_Dropdown qualityDropdown;

    [Header("Default Quality")]
    public int defaultQuality = 5;

    public bool setToMax = false; // set to max volume
    public float volume;

    private void Awake()
    { 

        // Load the quality setting from PlayerPrefs or set to default if not found
        int savedQuality = PlayerPrefs.GetInt("qualitySetting", defaultQuality);
        QualitySettings.SetQualityLevel(savedQuality);
        if (qualityDropdown != null)
        {
            qualityDropdown.value = savedQuality;
        }

        // Load the volume settings from PlayerPrefs
        float savedMusicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        print($"Saved music volume{savedMusicVolume}");
        audioMixer.SetFloat("musicVolume", Mathf.Log10(savedMusicVolume) * 20);
        if (musicSlider != null)
        {
            musicSlider.value = savedMusicVolume;
        }

        float savedSFXVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
        print($"Saved sfx volume{savedSFXVolume}");
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(savedSFXVolume) * 20);
        if (sfxSlider != null)
        {
            sfxSlider.value = savedSFXVolume;
        }
    }


    /// <summary>
    /// Sets music volume to slider value
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
        
    }

    /// <summary>
    /// Sets SFX volume to slider value
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
        
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
        PlayerPrefs.SetInt("qualitySetting", quality);
        
    }
}
