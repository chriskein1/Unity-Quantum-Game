using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Array of Sound objects to manage different audio clips
    public Sound[] sounds;

    // Singleton instance of AudioManager
    public static AudioManager instance;

    // Audio mixer groups for music and SFX
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup SFXMixerGroup;

    private void Awake()
    {
        // Implementing Singleton pattern to ensure only one instance of AudioManager exists
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject); // Prevent AudioManager from being destroyed when loading new scenes

        // Initialize each sound in the sounds array
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>(); // Add an AudioSource component for each sound
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.loop = s.loop;

            // Assign the appropriate AudioMixerGroup based on the sound's name
            if (s.name == "GameMusic")
                s.source.outputAudioMixerGroup = musicMixerGroup;
            else if (s.name == "ButtonClick")
                s.source.outputAudioMixerGroup = SFXMixerGroup;
        }
    }

    private void Start()
    {
        // Load and apply volume settings from PlayerPrefs
        float savedMusicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        musicMixerGroup.audioMixer.SetFloat("musicVolume", Mathf.Log10(savedMusicVolume) * 20);

        float savedSFXVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
        SFXMixerGroup.audioMixer.SetFloat("sfxVolume", Mathf.Log10(savedSFXVolume) * 20);

        // Play the background music when the game starts
        Play("GameMusic");
    }

    // Method to play a sound by name
    public void Play(string name)
    {
        // Find the sound object by name in the sounds array
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play(); // Play the sound
    }
}
