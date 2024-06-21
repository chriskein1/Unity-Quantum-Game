using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialLevelFunctionality : MonoBehaviour
{
    public List<GameObject> tutorialSteps; // List to hold the tutorial step GameObjects
    public GameObject continueButton;
    public GameObject endTutorialButton;

    private int currentStep = 0;

    // Method to be called when the continue button is clicked
    public void Continue()
    {
        if (currentStep < tutorialSteps.Count - 1)
        {
            // Disable the current step
            tutorialSteps[currentStep].SetActive(false);
            // Enable the next step
            currentStep++;
            tutorialSteps[currentStep].SetActive(true);

            // Change button text if it's the last step
            if (currentStep == tutorialSteps.Count - 1)
            {
                continueButton.SetActive(false);
                endTutorialButton.SetActive(true);
            }
        }
        else
        {
            // If it's the last step, you might want to add functionality for ending the tutorial
            Debug.Log("Tutorial Completed");
            
        }
    }
    public void Replay()
    {
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
}
