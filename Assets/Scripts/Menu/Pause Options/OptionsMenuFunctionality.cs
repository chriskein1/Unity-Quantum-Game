using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenuFunctionality : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Volume Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Quality Dropdown")]
    public TMP_Dropdown qualityDropdown;
    [Header("Resolution Dropdown")]
    public TMP_Dropdown resolutionDropdown;
    [Header("Fullscreen Toggle")]
    public Toggle fullscreenToggle;

    [Header("Default Quality")]
    public int defaultQuality = 5;

    private Resolution[] resolutions;

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
        audioMixer.SetFloat("musicVolume", Mathf.Log10(savedMusicVolume) * 20);
        if (musicSlider != null)
        {
            musicSlider.value = savedMusicVolume;
        }

        float savedSFXVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(savedSFXVolume) * 20);
        if (sfxSlider != null)
        {
            sfxSlider.value = savedSFXVolume;
        }

        // Get available resolutions and populate the dropdown
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        HashSet<string> optionsSet = new HashSet<string>();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            if (optionsSet.Add(option))
            {
                options.Add(option);
                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = options.Count - 1;
                }
            }
        }
        resolutionDropdown.AddOptions(options);

        // Check if it's the first launch and save the current resolution as default if it is
        if (!PlayerPrefs.HasKey("resolutionIndex"))
        {
            PlayerPrefs.SetInt("resolutionIndex", currentResolutionIndex);
            PlayerPrefs.Save();
        }

        // Load the resolution setting from PlayerPrefs
        int savedResolutionIndex = PlayerPrefs.GetInt("resolutionIndex", currentResolutionIndex);
        SetResolution(savedResolutionIndex);
        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Load the fullscreen setting from PlayerPrefs or set to current fullscreen state if not found
        bool isFullScreen = PlayerPrefs.GetInt("fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        Screen.fullScreen = isFullScreen;
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = isFullScreen;
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

    public void PlayButtonSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play("ButtonClick");
        }
        else
        {
            Debug.LogError("AudioManager instance not found!");
        }
    }

    public void ToggleFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("fullscreen", isFullScreen ? 1 : 0);
    }

    public void SetResolution(int resolutionIndex)
    {
        string[] res = resolutionDropdown.options[resolutionIndex].text.Split('x');
        int width = int.Parse(res[0].Trim());
        int height = int.Parse(res[1].Trim());
        Screen.SetResolution(width, height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
    }
}
