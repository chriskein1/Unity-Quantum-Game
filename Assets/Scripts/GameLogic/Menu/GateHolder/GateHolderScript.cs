using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GateHolderScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> Gates = new List<GameObject>();
     private Camera mainCamera; // Reference to the main camera
    [SerializeField] private int amountOfXGates;
    [SerializeField] private int amountOfYGates;
    [SerializeField] private int amountOfZGates;
    [SerializeField] private int amountOfHGates;
    [SerializeField] private List<HolderButtonLogic> buttons = new List<HolderButtonLogic>();

    private Dictionary<string, int> gateCounts = new Dictionary<string, int>();
    private GameObject currentDraggedGate;

    private void Awake()
    {
        gateCounts["XGate"] = amountOfXGates;
        gateCounts["YGate"] = amountOfYGates;
        gateCounts["ZGate"] = amountOfZGates;
        gateCounts["HGate"] = amountOfHGates;
        UpdateGateCountTexts();
        mainCamera = FindObjectOfType<Camera>();
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
        UpdateText("XGateText", gateCounts["XGate"]);
        UpdateText("YGateText", gateCounts["YGate"]);
        UpdateText("ZGateText", gateCounts["ZGate"]);
        UpdateText("HGateText", gateCounts["HGate"]);
        foreach (var button in buttons)
            button.UpdateButtonAppearance();
    }

    private void UpdateText(string tag, int count)
    {
        GameObject[] textObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject textObject in textObjects)
        {
            TMP_Text tmpText = textObject.GetComponent<TMP_Text>();
            if (tmpText != null)
            {
                tmpText.text = $"{count}";
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
