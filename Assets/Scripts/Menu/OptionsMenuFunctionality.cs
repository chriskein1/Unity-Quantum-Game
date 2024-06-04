using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class OptionsMenuFunctionality : MonoBehaviour
{
    // Reference to the AudioMixer for adjusting volumes
    public AudioMixer audioMixer; 
    
    // UI Sliders for music and SFX volume
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        // Set the slider values to the current volumes
        float currentMusicVolume;
        if (audioMixer.GetFloat("musicVolume", out currentMusicVolume))
        {
            musicSlider.value = Mathf.Pow(10, currentMusicVolume / 20); //changing value to base10
        }

        float currentSFXVolume;
        if (audioMixer.GetFloat("sfxVolume", out currentSFXVolume))
        {
            sfxSlider.value = Mathf.Pow(10, currentSFXVolume / 20); //changing value to base10
        }
    }
        /// <summary>
        /// sets music volume to slider value
        /// </summary>
        public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume",Mathf.Log10(volume) * 20);
        Debug.Log($"Music volume set to {volume}");
    }
    /// <summary>
    /// sets SFX volume to slider value
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume",Mathf.Log10( volume)*20);
        Debug.Log($"SFX volume set to {volume}");
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
        Debug.Log($"quality set to {quality}");
    }
}


