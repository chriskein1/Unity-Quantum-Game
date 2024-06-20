using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOverlay : MonoBehaviour
{
    private bool enabledOverlay=true;

    public void DisableTutorial()
    {
        gameObject.SetActive(false);
        enabledOverlay = false;
    }

    public void EnableTutorial()
    {
        gameObject.SetActive(true);
        enabledOverlay = true;
    }

    public bool getStatus()
    {
        return enabledOverlay; 
    }
}
