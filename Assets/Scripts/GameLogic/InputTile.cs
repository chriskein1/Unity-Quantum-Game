using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputTile : MonoBehaviour
{
    [Header("Starting Qubit State")]
    [SerializeField] private int state;
    [SerializeField] private bool PositiveState;
    [SerializeField] private bool SuperPosition;
    [SerializeField] private ChangeState blochSphere;
    private bool HApplied;
    private TextMeshPro text;

    void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        UpdateText();
    }
    // updates sprites text 
    void UpdateText()
    {
        string sign = PositiveState ? "": "-";
        string qubitStr = "";
        
        if (HApplied)
        {
            if (PositiveState)
            {
                qubitStr = SuperPosition ? "i/√2(|0> + |1>)" : "1/√2(|0> + |1>)";
            }
            else
            {
                qubitStr = SuperPosition ? "i/√2(|0> - |1>)" : "1/√2(|0> - |1>)";
            }
        }
        else if (SuperPosition)
        {
            qubitStr = $"{sign}i|{state}>";
        }
        else
        {
            qubitStr = $"{sign}|{state}>";
        }

        text.text = qubitStr;
    }


    // Function to return the state and its sign
    public (int, bool, bool, bool) GetState()
    {
        return (state, PositiveState, SuperPosition, HApplied);
    }

    // Function to set the state and its sign
    public void SetState((int, bool, bool, bool) newState)
    {
        state = newState.Item1;
        PositiveState = newState.Item2;
        SuperPosition = newState.Item3;
        HApplied = newState.Item4;
        UpdateText();
        UpdateBlochSphere();
    }

    public void UpdateBlochSphere()
    {
        if (blochSphere == null)
        {
            Debug.LogWarning("BlochSphere reference is not set.");
            return;
        }

        if (state == 0)
        {
            if (PositiveState)
            {
                blochSphere.SetZeroState();
                
            }
            else
            {
                blochSphere.SetNegativeState();
                
            }
        }
        else if (state == 1)
        {
            if (PositiveState)
            {
                blochSphere.SetOneState();
               
            }
            else
            {
                blochSphere.SetPositiveState();
               
            }

        }
    }
}