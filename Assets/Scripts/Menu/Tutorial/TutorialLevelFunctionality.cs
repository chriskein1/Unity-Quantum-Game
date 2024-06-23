using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialLevelFunctionality : MonoBehaviour
{
    public List<GameObject> tutorialSteps; // List to hold the tutorial step GameObjects
    public GameObject Replaybutton;
    public GameObject endTutorialButton;

    private int currentStep = 0;

    // Method to be called when the continue button is clicked
    public void Continue()
    {
        if (currentStep < tutorialSteps.Count - 1)
        {
            ClickSound();
            // Disable the current step
            tutorialSteps[currentStep].SetActive(false);
            // Enable the next step
            currentStep++;
            tutorialSteps[currentStep].SetActive(true);

           
        }
        else
        {
            ClickSound();
            // If it's the last step, you might want to add functionality for ending the tutorial
            Replaybutton.SetActive(true);
            endTutorialButton.SetActive(true);
            tutorialSteps[tutorialSteps.Count-1].SetActive(false);
        }
    }
    public void Replay()
    {
        ClickSound();
        OnEnable();

    }

    // Start is called before the first frame update
    void OnEnable()
    {
        currentStep = 0;
        // Initialize by setting only the first step active and all others inactive
        for (int i = 0; i < tutorialSteps.Count; i++)
        {
            tutorialSteps[i].SetActive(i == 0);
        }
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
