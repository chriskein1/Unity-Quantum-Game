using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

using QubitType;
public class ChangeTileState : MonoBehaviour
{
    [Header("Starting Qubit State")]
    [SerializeField] public float alphaReal;
    [SerializeField] public float alphaImaginary;
    [SerializeField] public float betaReal;
    [SerializeField] public float betaImaginary;

    [SerializeField] TextMeshPro OutputTile;
    [SerializeField] GameObject SuperPositionTile;
    [SerializeField] GameObject Numerator;
    [SerializeField] GameObject States;
    private TextMeshPro text;
    private TextMeshPro numeratorText;
    private TextMeshPro statesText;

    private Qubit qubit;


    void Start()    
    {
        ToQubit();
        
        if (OutputTile == null || SuperPositionTile == null || Numerator == null || States == null)
        {
            return;
        }
            
        // text = OutputTile.GetComponentInChildren<TextMeshProUGUI>();
        // numeratorText = Numerator.GetComponentInChildren<TextMeshPro>();
        // statesText = States.GetComponentInChildren<TextMeshPro>();
        UpdateText();
    }

    private void ToQubit()
    {
        qubit = new Qubit(new Complex(alphaReal, alphaImaginary), new Complex(betaReal, betaImaginary));
    }
    
    void UpdateText()
    {
        string qubitStr = "";

        // Check for negative sign
        bool negativeSign = false;
        if (qubit.Alpha.Real < 0 || qubit.Beta.Real < 0
            || qubit.Alpha.Imaginary < 0 || qubit.Beta.Imaginary < 0)
        {
            negativeSign = true;
        }
        qubitStr += negativeSign ? "-" : "";

        // Check for imaginary part
        if (qubit.Alpha.Imaginary != 0 || qubit.Beta.Imaginary != 0)
        {
            qubitStr += "i";
        }

        // Check if state is 0 or 1
        if (Math.Abs(qubit.Alpha.Real) == 1 || Math.Abs(qubit.Alpha.Imaginary) == 1)
        {
            qubitStr += "|0>";
        }
        else if (Math.Abs(qubit.Beta.Real) == 1 || Math.Abs(qubit.Beta.Imaginary) == 1)
        {
            qubitStr += "|1>";
        }
        else
        {
            qubitStr = $"|ψ> = {qubit.Alpha} |0> + {qubit.Beta} |1>";
        }

        Debug.Log("State: " + qubitStr);

        if (text == null)
        {
            Debug.LogWarning("TextMeshPro component not found.");
            return;
        }
        OutputTile.text = qubitStr;
    }


    // Function to return the state and its sign
    public Qubit GetState()
    {
        return qubit;
    }

    // Function to set the state and its sign
    public void SetState(Qubit newState)
    {
        qubit = newState;
        UpdateText();
    }
}