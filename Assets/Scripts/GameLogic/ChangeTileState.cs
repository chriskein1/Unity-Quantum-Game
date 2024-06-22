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
    [SerializeField] GameObject Numerator;
    [SerializeField] GameObject States;
    private TextMeshPro text;
    private TextMeshPro numeratorText;
    private TextMeshPro statesText;


    void Start()
    {
        if (OutputTile == null || SuperPositionTile == null || Numerator == null || States == null)
        {
            return;
        }
        // Prevent -0
        if (qubit.state == 0 && !qubit.PositiveState && !qubit.ImaginaryState)
        {
            qubit.PositiveState = true;
        }

        text = GetComponentInChildren<TextMeshPro>();
        numeratorText = Numerator.GetComponentInChildren<TextMeshPro>();
        statesText = States.GetComponentInChildren<TextMeshPro>();
        UpdateText();
    }
    // updates sprites text 
    void UpdateText()
    {
        string sign = qubit.PositiveState ? "": "-";
        string qubitStr = "";
        
        if (qubit.HApplied)
        {
            numeratorText.text = qubit.ImaginaryState ? "i" : "1";
            OutputTile.SetActive(false);
            SuperPositionTile.SetActive(true);

            if (qubit.state == 0 && qubit.PositiveState)
            {
                statesText.text = "(|0>+|1>)";
            }
            // For -i|0>
            // -|0> is treated the same as |0>
            else if (qubit.state == 0 && !qubit.PositiveState)
            {
                statesText.text = "(|0>-|1>)";
            }
            else if (qubit.state == 1 && qubit.PositiveState)
            {
                statesText.text = "(|0>-|1>)";
            }
            // -1(|0> - |1>) = 1(|0> + |1>)
            else
            {
                statesText.text = "(|0>+|1>)";
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