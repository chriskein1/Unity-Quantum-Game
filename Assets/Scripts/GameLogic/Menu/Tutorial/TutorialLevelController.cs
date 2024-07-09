using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevelController : MonoBehaviour
{

    public GameObject introTutorial;
    private bool enabledOverlay = true;

    public void DisableIntroTutorial()
    {
        ClickSound();
        introTutorial.SetActive(false);
        enabledOverlay = false;

    }

    public void EnableIntroTutorial()
    {
        ClickSound();
        introTutorial.SetActive(true);
        enabledOverlay = true;
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
