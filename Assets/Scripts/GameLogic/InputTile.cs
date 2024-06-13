using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using QubitType;
public class InputTile : MonoBehaviour
{
    [Header("Starting Qubit State")]
    [SerializeField] Qubit qubit;
    [SerializeField] private ChangeState blochSphere;

    private TextMeshPro text;

    void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        UpdateText();
    }
    // updates sprites text 
    void UpdateText()
    {
        string sign = qubit.PositiveState ? "": "-";
        string qubitStr = "";
        
        if (qubit.HApplied)
        {
            if (qubit.PositiveState)
            {
                qubitStr = qubit.ImaginaryState ? "i/√2(|0> + |1>)" : "1/√2(|0> + |1>)";
            }
            else
            {
                qubitStr = qubit.ImaginaryState ? "i/√2(|0> - |1>)" : "1/√2(|0> - |1>)";
            }
        }
        else if (qubit.ImaginaryState)
        {
            qubitStr = $"{sign}i|{qubit.state}>";
        }
        else
        {
            qubitStr = $"{sign}|{qubit.state}>";
        }

        text.text = qubitStr;
    }


    // Function to return the state and its sign
    public Qubit GetState()
    {
        return qubit;
    }

    // Function to set the state and its sign
    public void SetState(Qubit newState)
    {
        qubit.state = newState.state;
        qubit.PositiveState = newState.PositiveState;
        qubit.ImaginaryState = newState.ImaginaryState;
        qubit.HApplied = newState.HApplied;
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

        if (qubit.state == 0)
        {
            if (qubit.PositiveState)
            {
                blochSphere.SetZeroState();
            }
            else
            {
                blochSphere.SetNegativeState();
            }
        }
        else if (qubit.state == 1)
        {
            if (qubit.PositiveState)
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