using System.Collections.Generic;
using UnityEngine;
using QubitType;
using System.Numerics;

public class CircuitManager : MonoBehaviour
{
    [SerializeField] private List<QubitWireController> qubitWireControllers;
    [SerializeField] private List<SingleQubitStateOptions> qubitInputs;
    [SerializeField] private BarChartManager barChartManager;
    private List<List<GameObject>> snapPointLists = new List<List<GameObject>>();
    private List<List<Qubit>> snapPointStates = new List<List<Qubit>>();
    private QubitOperations qubitOperations = new QubitOperations();

    private void Start()
    {
        InitializeSnapPointLists();
        SetInputs();
        Evaluate();
    }

    private void InitializeSnapPointLists()
    {
        snapPointLists.Clear();
        snapPointStates.Clear();

        foreach (QubitWireController controller in qubitWireControllers)
        {
            List<GameObject> snapPoints = controller.GetSnapPoints();
            snapPointLists.Add(snapPoints);
            Debug.Log($"Added {snapPoints.Count} snap points for controller {controller.name}");

            List<Qubit> qubitList = new List<Qubit>();
            for (int i = 0; i < snapPoints.Count; i++)
            {
                qubitList.Add(new Qubit());
            }
            snapPointStates.Add(qubitList);
        }

        // Ensure that all snapPointLists and snapPointStates are correctly populated
        for (int i = 0; i < snapPointLists.Count; i++)
        {
            Debug.Log($"snapPointLists[{i}] has {snapPointLists[i].Count} elements.");
        }
        for (int i = 0; i < snapPointStates.Count; i++)
        {
            Debug.Log($"snapPointStates[{i}] has {snapPointStates[i].Count} elements.");
        }
    }

    public void Evaluate()
    {
        if (snapPointLists.Count == 0 || snapPointStates.Count == 0)
        {
            Debug.LogError("snapPointLists or snapPointStates is not initialized.");
            return;
        }

        int numColumns = snapPointLists[0].Count;
        Debug.Log($"Evaluating circuit with {numColumns} columns and {qubitWireControllers.Count} rows.");

        for (int col = 0; col < numColumns; col++)
        {
            for (int row = 0; row < qubitWireControllers.Count; row++)
            {
                if (col >= snapPointLists[row].Count)
                {
                    Debug.LogError($"Column index {col} out of range for row {row}");
                    continue;
                }

                GameObject snapPoint = snapPointLists[row][col];
                Snap snapComp = snapPoint.GetComponent<Snap>();
                GameObject gateObject = snapComp.GetGateObject();

                if (gateObject != null)
                {
                    if (gateObject.CompareTag("ctrl"))
                    {
                        HandleCNOTGate(row, col, gateObject);
                    }
                    else if (gateObject.CompareTag("swap"))
                    {
                        HandleSWAPGate(row, col, gateObject);
                    }
                    else if (gateObject.CompareTag("target"))
                    {
                        // Target gates are handled by the control gate
                    }
                    else
                    {
                        ApplySingleQubitGate(row, col, gateObject);
                    }
                }
                else
                {
                    if (col == 0) // First column should get the input state
                    {
                        snapPointStates[row][col] = qubitOperations.ConvertToQubit(qubitInputs[row]);
                    }
                    else
                    {
                        snapPointStates[row][col] = snapPointStates[row][col - 1];
                    }
                }

                snapComp.SetState(snapPointStates[row][col]);
            }
        }

        // Update the final state for each qubit wire
        for (int row = 0; row < qubitWireControllers.Count; row++)
        {
            Qubit finalState = snapPointStates[row][numColumns - 1];
            Debug.Log($"Set outputs for row {row}: {finalState}");
            qubitWireControllers[row].SetOutput(finalState);
        }
        if (qubitWireControllers.Count >= 2)
        {
            Qubit finalStateQubit1 = snapPointStates[0][numColumns - 1];
            Qubit finalStateQubit2 = snapPointStates[1][numColumns - 1];
            UpdateBarChart(finalStateQubit1, finalStateQubit2);
        }
        else if (qubitWireControllers.Count == 1)
        {
            Qubit finalStateQubit = snapPointStates[0][numColumns - 1];
            UpdateBarChartSingle(finalStateQubit);
        }
    }

    private void HandleCNOTGate(int row, int col, GameObject gateObject)
    {
        Qubit controlQubit = snapPointStates[row][col - 1];

        // Check for target qubits in adjacent rows in the same column
        for (int targetRow = 0; targetRow < qubitWireControllers.Count; targetRow++)
        {
            if (targetRow == row) continue;

            GameObject targetSnapPoint = snapPointLists[targetRow][col];
            Snap targetSnapComp = targetSnapPoint.GetComponent<Snap>();
            GameObject targetGateObject = targetSnapComp.GetGateObject();

            if (targetGateObject != null)
            {
                Qubit targetQubit = snapPointStates[targetRow][col - 1];
                QuantumGates.ApplyCNOT(ref controlQubit, ref targetQubit);
                snapPointStates[targetRow][col] = targetQubit;
                targetSnapComp.SetState(targetQubit);
            }
        }

        snapPointStates[row][col] = controlQubit;
    }

    private void HandleSWAPGate(int row, int col, GameObject gateObject)
    {
        // Find the other qubit to swap with
        for (int swapRow = 0; swapRow < qubitWireControllers.Count; swapRow++)
        {
            if (swapRow == row) continue;

            GameObject swapSnapPoint = snapPointLists[swapRow][col];
            Snap swapSnapComp = swapSnapPoint.GetComponent<Snap>();
            GameObject swapGateObject = swapSnapComp.GetGateObject();

            if (swapGateObject != null && swapGateObject.CompareTag("swap"))
            {
                Qubit qubit1 = snapPointStates[row][col - 1];
                Qubit qubit2 = snapPointStates[swapRow][col - 1];
                QuantumGates.ApplySWAP(ref qubit1, ref qubit2);
                snapPointStates[row][col] = qubit1;
                snapPointStates[swapRow][col] = qubit2;
                swapSnapComp.SetState(qubit2);
                break;
            }
        }
    }

    private void ApplySingleQubitGate(int row, int col, GameObject gateObject)
    {
        Qubit state;
        if (col == 0)
        {
            state = qubitOperations.ConvertToQubit(qubitInputs[row]);
        }
        else
        {
            state = snapPointStates[row][col - 1];
        }

        qubitOperations.ApplyGateOperation(gateObject, ref state);
        snapPointStates[row][col] = state;
    }

    private void SetInputs()
    {
        for (int i = 0; i < qubitWireControllers.Count; i++)
        {
            if (snapPointStates[i].Count > 0)  // Ensure the list has been initialized
            {
                Qubit inputQubit = qubitOperations.ConvertToQubit(qubitInputs[i]);
                snapPointStates[i][0] = inputQubit;
                qubitWireControllers[i].SetInput(qubitInputs[i]);
            }
        }
    }

    private void UpdateBarChart(Qubit finalStateQubit1, Qubit finalStateQubit2)
    {
        // Calculate probabilities for the states 00, 01, 10, and 11
        float prob00 = MagnitudeSquared(finalStateQubit1.Alpha * finalStateQubit2.Alpha);
        float prob01 = MagnitudeSquared(finalStateQubit1.Alpha * finalStateQubit2.Beta);
        float prob10 = MagnitudeSquared(finalStateQubit1.Beta * finalStateQubit2.Alpha);
        float prob11 = MagnitudeSquared(finalStateQubit1.Beta * finalStateQubit2.Beta);

        barChartManager.UpdateBarChart(prob00, prob01, prob10, prob11);
    }
    private float MagnitudeSquared(Complex c)
    {
        return (float)(c.Real * c.Real + c.Imaginary * c.Imaginary);
    }
    private void UpdateBarChartSingle(Qubit finalStateQubit)
    {
        // Calculate probabilities for the states |0> and |1>
        float prob0 = MagnitudeSquared(finalStateQubit.Alpha);
        float prob1 = MagnitudeSquared(finalStateQubit.Beta);

        // Update the bar chart using the same method but with only two bars
        barChartManager.UpdateBarChart(prob0, prob1);
    }
}
