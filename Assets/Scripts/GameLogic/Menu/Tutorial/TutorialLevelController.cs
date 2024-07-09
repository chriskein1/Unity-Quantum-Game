using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelController : MonoBehaviour
{
    public GameObject levelTutorial;
    public GameObject introTutorial;
    private bool enabledOverlay = true;

    public void DisableIntroTutorial()
    {
        ClickSound();
        introTutorial.SetActive(false);
        levelTutorial.SetActive(true);
        enabledOverlay = true;

    }

    public void EnableIntroTutorial()
    {
        ClickSound();
        introTutorial.SetActive(true);
        levelTutorial.SetActive(false);
        enabledOverlay = true;
    }

    public void DisableLevelTutorial()
    {
        ClickSound();
        levelTutorial.SetActive(false);
        enabledOverlay = false;
    }
    public bool GetStatus()
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
