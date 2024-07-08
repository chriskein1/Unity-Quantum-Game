using System.Collections.Generic;
using UnityEngine;
using QubitType;
using System;
using System.Numerics;

public class CircuitManager : MonoBehaviour
{
    [SerializeField] private List<QubitWireController> qubitWireControllers;
    [SerializeField] private List<SingleQubitStateOptions> qubitInputs;
    [SerializeField] private List<SingleQubitStateOptions> winState;
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

        bool win = false;

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
            win = EvaluateWin(new List<Qubit> { finalStateQubit1, finalStateQubit2 });
        }
        else if (qubitWireControllers.Count == 1)
        {
            Qubit finalStateQubit = snapPointStates[0][numColumns - 1];
            UpdateBarChartSingle(finalStateQubit);
            win = EvaluateWin(new List<Qubit> { finalStateQubit });
        }

        if (win)
        {
            Debug.Log("YOU WIN!!!!!!");
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

    // Determine if the win condition is met for the qubits
    private bool EvaluateWin(List<Qubit> finalStates)
    {
        if (winState.Count == 0)
        {
            Debug.LogError("Win state is not initialized.");
            return false;
        }
        else if (finalStates.Count != winState.Count)
        {
            Debug.LogError("Final states and win state do not match in length.");
            return false;
        }
        // If either win qubit is "no state"
        if (winState.Contains(SingleQubitStateOptions.NoState))
        {
            return false;
        }

        List<Qubit> winStateQ = new List<Qubit>();
        foreach (SingleQubitStateOptions state in winState)
        {
            winStateQ.Add(qubitOperations.ConvertToQubit(state));
        }

        if (winStateQ.Count == 1)
        {
            // Check if they are approximately equal
            return finalStates[0].IsApproximatelyEqual(winStateQ[0]);
        }
        else if (winStateQ.Count == 2)
        {
            // Imaginary and negative values can be on either qubit, so check both possibilities
            bool imaginaryWin = false;
            bool imaginaryFinal = false;
            for (int i = 0; i < 2; i++)
            {
                Qubit q = winStateQ[i];
                Qubit qFinal = finalStates[i];
                if (q.Alpha.Imaginary != 0)
                {
                    imaginaryWin = !imaginaryWin;
                } 
                if (q.Beta.Imaginary != 0)
                {
                    imaginaryWin = !imaginaryWin;
                }
                if (qFinal.Alpha.Imaginary != 0)
                {
                    imaginaryFinal = !imaginaryFinal;
                }
                if (qFinal.Beta.Imaginary != 0)
                {
                    imaginaryFinal = !imaginaryFinal;
                }
            }
            bool negativeWin = false;
            bool negativeFinal = false;
            for (int i = 0; i < 2; i++)
            {
                Qubit q = winStateQ[i];
                Qubit finalQ = finalStates[i];
                if (q.Alpha.Real < 0 || q.Alpha.Imaginary < 0)
                {
                    negativeWin = !negativeWin;
                } 
                if (q.Beta.Real < 0 || q.Beta.Imaginary < 0)
                {
                    negativeWin = !negativeWin;
                }
                if (finalQ.Alpha.Real < 0 || finalQ.Alpha.Imaginary < 0)
                {
                    negativeFinal = !negativeFinal;
                }
                if (finalQ.Beta.Real < 0 || finalQ.Beta.Imaginary < 0)
                {
                    negativeFinal = !negativeFinal;
                }
            }

            // If both win states or final states are imaginary, flip negative values
            // i * i = -1
            if ((winStateQ[0].Alpha.Imaginary != 0 || winStateQ[0].Beta.Imaginary != 0)
                && (winStateQ[1].Alpha.Imaginary != 0 || winStateQ[1].Beta.Imaginary != 0))
                {
                    negativeWin = !negativeWin;
                }
            
            if ((finalStates[0].Alpha.Imaginary != 0 || finalStates[0].Beta.Imaginary != 0)
                && (finalStates[1].Alpha.Imaginary != 0 || finalStates[1].Beta.Imaginary != 0))
                {
                    negativeFinal = !negativeFinal;
                }

            // If the real parts are equal and the signs and imaginary parts are equal, the qubits are equal
            if (!imaginaryWin && !imaginaryFinal)
            return (CompareReal(finalStates[0], winStateQ[0]) && CompareReal(finalStates[1], winStateQ[1]) &&
                   imaginaryWin == imaginaryFinal && negativeWin == negativeFinal);
        }
        return false;
    }

    private bool CompareReal(Qubit q1, Qubit q2, double tolerance = 1e-10)
    {
        // Compare positive values for alpha and beta
        return (Math.Abs(q1.Alpha.Real - q2.Alpha.Real) < tolerance && Math.Abs(q1.Beta.Real - q2.Beta.Real) < tolerance);
    }
}
