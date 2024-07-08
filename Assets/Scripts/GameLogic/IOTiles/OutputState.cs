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
    private Qubit qubit;

    void Start()
    {
        // Initialize the visual representation based on the initial qubit state
        UpdateVisualRepresentation();
    }

    // Function to set the state and its sign
    public void SetState(Qubit newState)
    {
        qubit = newState;
        UpdateVisualRepresentation();
    }

    private void UpdateVisualRepresentation()
    {
        // Determine the current state as an enum
        SingleQubitStateOptions currentState = qubitOperations.ConvertToStateOption(qubit);
        print($"Current state {currentState}");
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
                ShowBasisState("-|0>"); // Added handling for NegativeState0
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
                break;
            case SingleQubitStateOptions.SuperpositionMinus:
                ShowSuperpositionState("-", "1");
                break;
            case SingleQubitStateOptions.ImaginarySuperpositionPlus:
                ShowSuperpositionState("+", "i");
                break;
            case SingleQubitStateOptions.ImaginarySuperpositionMinus:
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
