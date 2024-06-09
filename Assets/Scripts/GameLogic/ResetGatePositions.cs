using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Reset all gate positions

public class Reset : MonoBehaviour
{
    // Lists of positions and rotations
    private List<Vector3> gatePositions = new List<Vector3>();
    private List<Quaternion> gateRotations = new List<Quaternion>();

    // Gate parent object
    private GameObject gates;

    // Get gate parent object
    void Start()
    {
        gates = GameObject.Find("Gates");
        initializeGatePos();
    }

    private void initializeGatePos()
    {
        // Get all actual gate objects
        if (gates != null)
        {
            Transform[] allChildren = gates.GetComponentsInChildren<Transform>();

            foreach (Transform childGate in allChildren)
            {
                // Skip parent object
                if (childGate == gates.transform)
                {
                    continue;
                }
                // Push to list
                gatePositions.Add(childGate.position);
                gateRotations.Add(childGate.rotation);
            }
        }
    }

    public void ResetGates()
    {
        int i = 0;
        // Get all actual gate objects
        if (gates != null)
        {
            Transform[] allChildren = gates.GetComponentsInChildren<Transform>();

            foreach (Transform childGate in allChildren)
            {
                // Skip parent object
                if (childGate == gates.transform)
                {
                    continue;
                }
                // Reset position and rotation
                childGate.position = gatePositions[i];
                childGate.rotation = gateRotations[i];
                i++;
            }
        }    
    }
}