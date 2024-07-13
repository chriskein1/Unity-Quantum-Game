using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class OutputbuttonLogic : MonoBehaviour , IPointerDownHandler
{
    public OutputHolderScript outputHolderScript; // Reference to the GateHolderScript
    public string gateType; // Type of the gate this button is responsible for

    private TutorialOverlay tutorialOverlay;
    private bool hasTutorialOverlay;
    private TextMeshProUGUI gateCountText;
    private Image buttonImage;

    void Awake()
    {
        tutorialOverlay = FindAnyObjectByType<TutorialOverlay>();
        hasTutorialOverlay = tutorialOverlay != null;

        gateCountText = GetComponentInChildren<TextMeshProUGUI>();
        buttonImage = GetComponent<Image>();
        UpdateButtonAppearance();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (Time.timeScale > 0)
        {
            if (hasTutorialOverlay)
            {
                if (!tutorialOverlay.GetStatus())
                {
                    outputHolderScript.SpawnAndDragGate(gateType);
                }
            }
            else
            {
                
                outputHolderScript.SpawnAndDragGate(gateType);
            }
            UpdateButtonAppearance();
        }
    }

    public void UpdateButtonAppearance()
    {
        if (gateCountText == null || buttonImage == null)
        {
            return;
        }

        int gateCount;
        if (int.TryParse(gateCountText.text, out gateCount) && gateCount == 0)
        {
            buttonImage.color = Color.gray;
        }
        else
        {
            buttonImage.color = Color.white;
        }
    }
}
