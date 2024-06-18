using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateDestroyer : MonoBehaviour
{

    [SerializeField] private GateHolderScript gateHolder;
    private string[] gateTags = new string[] { "XGate", "YGate", "ZGate", "HGate" };

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("XGate") || other.CompareTag("YGate") || other.CompareTag("ZGate") || other.CompareTag("HGate"))
        {
            string gateTag = other.tag;

            // Check if the other object has a parent with the same tag
            Transform objectToDestroy = other.transform;
            if (other.transform.parent != null && other.transform.parent.CompareTag(gateTag))
            {
                objectToDestroy = other.transform.parent;
            }

            gateHolder.AddGateBackToCount(gateTag);
            Destroy(objectToDestroy.gameObject);
        }
    }

    private void OnMouseDown()
    {
        if(Time.timeScale > 0)
        DestroyAllGates();
    }
    private void DestroyAllGates()
    {
        foreach (string tag in gateTags)
        {
            GameObject[] gates = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject gate in gates)
            {
                // Check if the gate has a parent and if the parent has the same tag
                if (gate.transform.parent == null || gate.transform.parent.tag != tag)
                {
                    gateHolder.AddGateBackToCount(gate.tag);
                    Destroy(gate);
                }
            }
        }
    }
}
