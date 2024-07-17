using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using System;
using QubitType;

public class OutputState : MonoBehaviour
{
    [SerializeField] private GameObject OutputTile;
    [SerializeField] private TextMeshPro OutputTileText;
    [SerializeField] private GameObject SuperPositionTile;
    [SerializeField] private TextMeshPro sign;
    [SerializeField] private TextMeshPro numerator;

    private QubitOperations qubitOperations = new QubitOperations();
    private Qubit qubit = new Qubit(new Complex(1, 0), new Complex(0, 0));

    void Start()
    {
        // Initialize the visual representation based on the initial qubit state
        //UpdateVisualRepresentation();
    }

    // Function to set the state and its sign
    public void SetState(Qubit newState)
    {
        qubit = newState;
        UpdateVisualRepresentation();
        Debug.Log("Final state is " + newState);
    }

    private void UpdateVisualRepresentation()
    {
        // Determine the current state as an enum
        SingleQubitStateOptions currentState = qubitOperations.ConvertToStateOption(qubit);
        Debug.Log($"current state {currentState}");
        Debug.Log($"---------{qubit}");
        // Update the visual representation based on the current state
        switch (currentState)
        {
            case SingleQubitStateOptions.State0:
                ShowBasisState("|0>");
                break;
            case SingleQubitStateOptions.State1:
                ShowBasisState("|1>");
                break;
            case SingleQubitStateOptions.Imaginary0:
                ShowBasisState("i|0>");
                break;
            case SingleQubitStateOptions.Imaginary1:
                ShowBasisState("i|1>");
                break;
            case SingleQubitStateOptions.NegativeState0:
                ShowBasisState("-|0>"); 
                break;
            case SingleQubitStateOptions.NegativeState1:
                ShowBasisState("-|1>");
                break;
            case SingleQubitStateOptions.NegativeImaginary1:
                ShowBasisState("-i|1>");
                break;
            case SingleQubitStateOptions.NegativeImaginary0:
                ShowBasisState("-i|0>");
                break;
            case SingleQubitStateOptions.SuperpositionPlus:
                ShowSuperpositionState("+", "1");
                Debug.Log("1---------");
                break;
            case SingleQubitStateOptions.SuperpositionMinus:
                Debug.Log("2---------");
                ShowSuperpositionState("-", "1");
                break;
            case SingleQubitStateOptions.ImaginarySuperpositionPlus:
                Debug.Log("3---------");
                ShowSuperpositionState("+", "i");
                break;
            case SingleQubitStateOptions.ImaginarySuperpositionMinus:
                Debug.Log("4---------");
                ShowSuperpositionState("-", "i");
                break;
            default:
                OutputTileText.text = "|ψ>";
                break;
        }
    }

    private void ShowBasisState(string stateText)
    {
        SuperPositionTile.SetActive(false);
        OutputTile.SetActive(true);
        OutputTileText.text = stateText;
    }

    private void ShowSuperpositionState(string stateSign, string stateNumerator)
    {
        SuperPositionTile.SetActive(true);
        OutputTile.SetActive(false);
        sign.text = stateSign;
        numerator.text = stateNumerator;
    }
}
