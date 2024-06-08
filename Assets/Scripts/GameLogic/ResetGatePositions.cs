using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGatePositions : MonoBehaviour
{
    [System.Serializable]
    public class Gate
    {
        public GameObject gateObject;
        [HideInInspector]
        public Vector3 initialPosition;
        [HideInInspector]
        public Quaternion initialRotation;
    }

    public List<Gate> gates;

    void Start()
    {
        if (gates == null || gates.Count == 0)
        {
            return;
        }

        foreach (Gate gate in gates)
        {
            if (gate.gateObject != null)
            {
                gate.initialPosition = gate.gateObject.transform.position;
                gate.initialRotation = gate.gateObject.transform.rotation;
                
            }
            else
            {
                
            }
        }
    }

    public void ResetGates()
    {
        
        foreach (Gate gate in gates)
        {
            if (gate.gateObject != null)
            {
                gate.gateObject.transform.position = gate.initialPosition;
                gate.gateObject.transform.rotation = gate.initialRotation;
                
            }
            else
            {
                Debug.LogError("Gate object is null");
            }
        }
    }
}
