using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutputHolderScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> Outputs = new List<GameObject>();
    private Camera mainCamera; // Reference to the main camera
    [SerializeField] private int amountOfQubits;
  
    [SerializeField] private List<OutputbuttonLogic> buttons = new List<OutputbuttonLogic>();


    private Dictionary<string, int> gateCounts = new Dictionary<string, int>();
    private List<TMP_Text> buttonTexts = new List<TMP_Text>();
    private GameObject currentDraggedGate;

    private void Awake()
    {
        gateCounts["State0Circle"] = amountOfQubits;
        gateCounts["NegativeState0Circle"] = amountOfQubits;
        gateCounts["State1Circle"] = amountOfQubits;
        gateCounts["NegativeState1Circle"] = amountOfQubits;
        gateCounts["State0Square"] = amountOfQubits;
        gateCounts["NegativeState0Square"] = amountOfQubits;
        gateCounts["State1Square"] = amountOfQubits;
        gateCounts["NegativeState1Square"] = amountOfQubits;
        mainCamera = FindObjectOfType<Camera>();
        GetButtonTexts();
        UpdateGateCountTexts();
    }

    public void SpawnAndDragGate(string gateTag)
    {
       
        if (gateCounts[gateTag] > 0)
        {
            
            GameObject gatePrefab = Outputs.Find(gate => gate.CompareTag(gateTag));

            if (gatePrefab != null)
            {
                
                GrabGateSound();
                // Get the mouse position in world space
                Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;

                currentDraggedGate = Instantiate(gatePrefab, mousePos, Quaternion.identity);
               
                Drag dragComponent = currentDraggedGate.GetComponentInChildren<Drag>();
                
               
                if (dragComponent != null)
                {
                    
                    dragComponent.StartDraggingFromSpawn(mousePos);
                }
                else
                {
                    Debug.LogError("Drag component not found on the gate or its children.");
                }
                
                gateCounts[gateTag]--;

                UpdateGateCountTexts();

            }
        }
    }

    public void AddGateBackToCount(string gateTag)
    {
        if (gateCounts.ContainsKey(gateTag))
        {
            gateCounts[gateTag]++;
            UpdateGateCountTexts();
        }
    }

    private void UpdateGateCountTexts()
    {
        
        UpdateText(0, gateCounts["State0Circle"]);
        UpdateText(1, gateCounts["NegativeState0Circle"]);
        UpdateText(2, gateCounts["State1Circle"]);
        UpdateText(3, gateCounts["NegativeState1Circle"]);
        UpdateText(4, gateCounts["State0Square"]);
        UpdateText(5, gateCounts["NegativeState0Square"]);
        UpdateText(6, gateCounts["State1Square"]);
        UpdateText(7, gateCounts["NegativeState1Square"]);
        foreach (var button in buttons)
            button.UpdateButtonAppearance();
    }

    private void GetButtonTexts()
    {
        foreach (var button in buttons)
        {
            TMP_Text tmpText = button.GetComponentInChildren<TMP_Text>();
            if (tmpText != null)
            {
                buttonTexts.Add(tmpText);
            }
            else
            {
                Debug.LogError($"TMP_Text component not found on button {button.name} or its children.");
            }
        }
    }
    private void UpdateText(int index, int count)
    {
        //Debug.Log($"Updating index:{index} count:{count} ");
        if (index >= 0 && index < buttonTexts.Count)
        {
            buttonTexts[index].text = $"{count}";
        }
        else
        {
            Debug.LogError($"Text component for index {index} not found in  button texts.");
        }
    }
    private void GrabGateSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play("GrabbingGateFromGateholder");
        }
        else
        {
            Debug.LogError("AudioManager instance not found!");
        }
    }
}
