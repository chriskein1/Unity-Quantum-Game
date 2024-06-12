using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideGateFunctionality : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<GameObject> Gates = new List<GameObject>();

    public void DisableAllGatePopups()
    {
        foreach (GameObject gate in Gates)
        {
            gate.SetActive(false);
        }
    }
}

