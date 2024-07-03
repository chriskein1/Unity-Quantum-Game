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
        Debug.Log($"UpdateVisualRepresentation called with Qubit: {qubit}");

        // Update the visual representation based on the current qubit state
        if (qubit.IsInSuperposition())
        {
            Debug.Log("Qubit is in superposition");
            // Show superposition tile
            SuperPositionTile.SetActive(true);
            OutputTile.SetActive(false);

            if (qubit.Beta.Real < 0)
                sign.text = "-";
            else
                sign.text = "+";
        }
        else
        {
            Debug.Log("Qubit is in basis state");
            // Show basis state tile
            SuperPositionTile.SetActive(false);
            OutputTile.SetActive(true);

            double epsilon = 0.0001; // Small threshold for floating-point comparison
            if (Math.Abs(qubit.Alpha.Real - 1) < epsilon && Math.Abs(qubit.Beta.Real) < epsilon)
            {
                OutputTileText.text = "|0>";
            }
            else if (Math.Abs(qubit.Alpha.Real) < epsilon && Math.Abs(qubit.Beta.Real - 1) < epsilon)
            {
                OutputTileText.text = "|1>";
            }
            else
            {
                OutputTileText.text = "|ψ>";
            }
        }
    }
}
