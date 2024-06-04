using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    //plays button pressed sound 
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
}
