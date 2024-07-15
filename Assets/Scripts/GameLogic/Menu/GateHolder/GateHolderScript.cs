using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GateHolderScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> Gates = new List<GameObject>();
     private Camera mainCamera; 
    [SerializeField] private int amountOfGates;
 
    [SerializeField] private List<HolderButtonLogic> buttons = new List<HolderButtonLogic>();
    private List<TMP_Text> buttonTexts = new List<TMP_Text>();
    private Dictionary<string, int> gateCounts = new Dictionary<string, int>();
    private GameObject currentDraggedGate;

    private void Awake()
    {
        InitializeGateCounts();
        GetButtonTexts();
        UpdateGateCountTexts();
        mainCamera = FindObjectOfType<Camera>();
    }
    private void InitializeGateCounts()
    {
        foreach (GameObject gate in Gates)
        {
            string gateTag = gate.tag;
            if (!gateCounts.ContainsKey(gateTag))
            {
                gateCounts[gateTag] = amountOfGates;
            }
        }
    }
    public void SpawnAndDragGate(string gateTag)
    {
        if (gateCounts[gateTag] > 0)
        {
            GameObject gatePrefab = Gates.Find(gate => gate.CompareTag(gateTag));

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
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i < buttonTexts.Count)
            {
                string gateTag = Gates[i].tag;
                if (gateCounts.ContainsKey(gateTag))
                {
                    buttonTexts[i].text = $"{gateCounts[gateTag]}";
                }
                else
                {
                    Debug.LogError($"Gate tag {gateTag} not found in gate counts.");
                }
            }
            else
            {
                Debug.LogError($"Text component for index {i} not found in button texts.");
            }
        }

        foreach (var button in buttons)
        {
            button.UpdateButtonAppearance();
        }
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
