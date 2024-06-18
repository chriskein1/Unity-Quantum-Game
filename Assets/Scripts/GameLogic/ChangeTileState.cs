using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using QubitType;
public class ChangeTileState : MonoBehaviour
{
    [Header("Starting Qubit State")]
    [SerializeField] Qubit qubit;
    [SerializeField] GameObject OutputTile;
    [SerializeField] GameObject SuperPositionTile;


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
                OutputTile.SetActive(false);
                SuperPositionTile.SetActive(true);
            }
            else
            {
                OutputTile.SetActive(false);
                SuperPositionTile.SetActive(true);
            }
        }
        else if (qubit.ImaginaryState)
        {
            OutputTile.SetActive(true);
            SuperPositionTile.SetActive(false);
            qubitStr = $"{sign}i|{qubit.state}>";
        }
        else
        {
            OutputTile.SetActive(true);
            SuperPositionTile.SetActive(false);
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
    }

    
}