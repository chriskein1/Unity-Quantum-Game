using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOverlay : MonoBehaviour
{
    private bool enabledOverlay=true;

    public void DisableTutorial()
    {
        ClickSound();
        gameObject.SetActive(false);
        enabledOverlay = false;
        
    }

    public void EnableTutorial()
    {
        ClickSound();
        gameObject.SetActive(true);
        enabledOverlay = true;
    }

    public bool getStatus()
    {
        return enabledOverlay; 
    }

    public void ClickSound()
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
