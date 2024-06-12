using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GateHolderScript : MonoBehaviour
{
    [SerializeField] private List<GameObject> Gates = new List<GameObject>();
    [SerializeField] private RectTransform spawnLocation; // Assuming this is a UI element
    [SerializeField] private Camera mainCamera; // Reference to the main camera
    [SerializeField] private int amountOfXGates;
    [SerializeField] private int amountOfYGates;
    [SerializeField] private int amountOfZGates;
    [SerializeField] private int amountOfHGates;

    private Dictionary<string, int> gateCounts = new Dictionary<string, int>();

    private void Awake()
    {
        gateCounts["XGate"] = amountOfXGates;
        gateCounts["YGate"] = amountOfYGates;
        gateCounts["ZGate"] = amountOfZGates;
        gateCounts["HGate"] = amountOfHGates;
        UpdateGateCountTexts();
    }

    public void SpawnXGate()
    {
        SpawnGate("XGate");
    }

    public void SpawnYGate()
    {
        SpawnGate("YGate");
    }

    public void SpawnZGate()
    {
        SpawnGate("ZGate");
    }

    public void SpawnHGate()
    {
        SpawnGate("HGate");
    }

    private void SpawnGate(string gateTag)
    {
        if (gateCounts[gateTag] > 0)
        {
            GameObject gatePrefab = Gates.Find(gate => gate.CompareTag(gateTag));

            if (gatePrefab != null && spawnLocation != null)
            {
                // Convert the RectTransform position to screen space
                Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, spawnLocation.position);

                // Convert the screen space position to world space
                Vector3 worldPos = mainCamera.ScreenToWorldPoint(screenPos);

                // Adjust the z-coordinate to the appropriate depth in the game world
                worldPos.z = 0;

                Instantiate(gatePrefab, worldPos, spawnLocation.rotation);
                gateCounts[gateTag]--;
                Debug.Log($"{gateTag} spawned. New count: {gateCounts[gateTag]}");
                UpdateGateCountTexts();
            }
        }
    }

    public void AddGateBackToCount(string gateTag)
    {
        if (gateCounts.ContainsKey(gateTag))
        {
            gateCounts[gateTag]++;
            Debug.Log($"{gateTag} returned. New count: {gateCounts[gateTag]}");
            UpdateGateCountTexts();
        }
    }
    private void UpdateGateCountTexts()
    {
        UpdateText("XGateText", gateCounts["XGate"]);
        UpdateText("YGateText", gateCounts["YGate"]);
        UpdateText("ZGateText", gateCounts["ZGate"]);
        UpdateText("HGateText", gateCounts["HGate"]);
    }

    private void UpdateText(string tag, int count)
    {
        GameObject[] textObjects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject textObject in textObjects)
        {
            TMP_Text tmpText = textObject.GetComponent<TMP_Text>();
            if (tmpText != null)
            {
                tmpText.text =$"{count}";
            }
        }
    }
}
